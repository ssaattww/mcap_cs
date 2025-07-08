using System;
using System.IO;
using Mcap.CSharp.Mcap.Interfaces;
using Mcap.CSharp.Mcap.Records;
using Mcap.CSharp.Mcap.Util;

namespace Mcap.CSharp.Mcap
{
    // Corresponds to C++ mcap::McapWriter in cpp/mcap/include/mcap/writer.hpp
    public class McapWriter : IDisposable
    {
        private readonly IStreamWriter _writer;
        private readonly McapWriterOptions _options;
        private MemoryStream _chunkBuffer; // Corresponds to C++ mcap::McapWriter::Chunk::buffer_
        private BinaryWriter _chunkBinaryWriter;
        private ulong _chunkStartOffset; // Corresponds to C++ mcap::McapWriter::Chunk::chunkStartOffset_
        private ulong _messageStartTime; // Corresponds to C++ mcap::McapWriter::Chunk::message_start_time_
        private ulong _messageEndTime; // Corresponds to C++ mcap::McapWriter::Chunk::message_end_time_

        public McapWriter(IStreamWriter writer, McapWriterOptions options)
        {
            _writer = writer;
            _options = options;
            _chunkBuffer = new MemoryStream();
            _chunkBinaryWriter = new BinaryWriter(_chunkBuffer);
            _messageStartTime = ulong.MaxValue;
            _messageEndTime = ulong.MinValue;
        }

        public void Dispose()
        {
            // Ensure any open chunk is ended before disposing
            EndChunk();
            // Write the footer before disposing
            WriteFooter(new Footer { SummaryStart = _writer.Size(), SummaryOffset = 0, SummaryCrc = 0 }); // TBD: Populate actual values
            _writer.End();
            _chunkBinaryWriter.Dispose();
            _chunkBuffer.Dispose();
        }

        public void WriteMagic()
        {
            _writer.Write(Constants.Magic, (ulong)Constants.Magic.Length);
        }

        public void WriteHeader(Header header)
        {
            WriteRecord(OpCode.Header, header);
        }

        public void WriteFooter(Footer footer)
        {
            WriteRecord(OpCode.Footer, footer);
        }

        public void AddSchema(Schema schema)
        {
            WriteRecord(OpCode.Schema, schema);
        }

        public void AddChannel(Channel channel)
        {
            WriteRecord(OpCode.Channel, channel);
        }

        public void Write(Message message)
        {
            // Corresponds to C++ mcap::McapWriter::write(const Message& message)
            // Check if current chunk is full or if no chunk is started
            if (_chunkBuffer.Length == 0 || _chunkBuffer.Length >= (long)_options.ChunkSize)
            {
                EndChunk(); // End current chunk if any
                StartChunk(); // Start a new chunk
            }
            WriteRecord(OpCode.Message, message);

            // Update message start and end times for the current chunk
            _messageStartTime = Math.Min(_messageStartTime, message.LogTime);
            _messageEndTime = Math.Max(_messageEndTime, message.LogTime);
        }

        // Corresponds to C++ mcap::McapWriter::startChunk()
        public void StartChunk()
        {
            EndChunk(); // Ensure any existing chunk is flushed

            _chunkStartOffset = _writer.Size();
            // Write a placeholder for the Chunk record. Actual data will be filled in EndChunk.
            // OpCode (1 byte) + recordLength (8 bytes) + Chunk record payload (variable)
            _writer.Write(new byte[] { (byte)OpCode.Chunk }, 1);
            _writer.Write(BitConverter.GetBytes((ulong)0), 8); // Placeholder for recordLength

            _chunkBuffer = new MemoryStream();
            _chunkBinaryWriter = new BinaryWriter(_chunkBuffer);

            // Reset message timestamps for the new chunk
            _messageStartTime = ulong.MaxValue;
            _messageEndTime = ulong.MinValue;
        }

        // Corresponds to C++ mcap::McapWriter::endChunk()
        public void EndChunk()
        {
            if (_chunkBuffer.Length == 0) return; // No active chunk to end

            // Get the chunk data
            byte[] chunkData = _chunkBuffer.ToArray();

            // Create a Chunk record (TBD: populate all fields correctly)
            var chunk = new Chunk
            {
                MessageStartTime = _messageStartTime,
                MessageEndTime = _messageEndTime,
                UncompressedSize = (ulong)chunkData.Length,
                UncompressedCrc = Crc32.Compute(chunkData),
                Compression = _options.Compression.ToString(), // Use configured compression
                CompressedSize = (ulong)chunkData.Length, // No compression yet, so compressed size is uncompressed size
                Records = chunkData
            };

            // Write the Chunk record payload to the main writer
            // This part is tricky as we need to go back and update the recordLength of the Chunk record
            // For now, we'll write it directly and fix the recordLength later or use a different strategy.
            // This will require seeking in the underlying stream or buffering the entire MCAP file.
            // For initial implementation, we'll just write the chunk data after the placeholder.

            // Seek back to the recordLength placeholder and write the actual length
            ulong currentPosition = _writer.Size();
            _writer.Seek((long)_chunkStartOffset + 1, SeekOrigin.Begin); // +1 for OpCode
            _writer.Write(BitConverter.GetBytes((ulong)chunk.GetRecordLength()), 8); // Write actual recordLength
            _writer.Seek((long)currentPosition, SeekOrigin.Begin); // Go back to current position

            // Write the chunk payload
            WriteRecord(OpCode.Chunk, chunk);

            // Reset chunk buffer
            _chunkBinaryWriter.Dispose();
            _chunkBuffer.Dispose();
            _chunkBuffer = new MemoryStream();
            _chunkBinaryWriter = new BinaryWriter(_chunkBuffer);
        }

        private void WriteRecord<T>(OpCode opCode, T record) where T : IRecordSerializable
        {
            using var recordStream = new MemoryStream();
            using var recordBinaryWriter = new BinaryWriter(recordStream);

            record.Write(recordBinaryWriter);
            byte[] recordBytes = recordStream.ToArray();

            // If we are writing to a chunk, write to the chunk buffer
            if (_chunkBuffer.Length > 0 && opCode != OpCode.Chunk) // Don't write Chunk record itself to chunk buffer
            {
                _chunkBinaryWriter.Write((byte)opCode);
                _chunkBinaryWriter.Write((ulong)recordBytes.Length);
                _chunkBinaryWriter.Write(recordBytes);
            }
            else // Write directly to the main writer (for non-chunk records or Chunk record itself)
            {
                _writer.Write(new byte[] { (byte)opCode }, 1); // Write OpCode
                byte[] recordLengthBytes = BitConverter.GetBytes((ulong)recordBytes.Length);
                _writer.Write(recordLengthBytes, (ulong)recordLengthBytes.Length); // Write record size
                _writer.Write(recordBytes, (ulong)recordBytes.Length); // Write record data
            }
        }
    }
}
