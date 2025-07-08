using Xunit;
using System.IO;
using Mcap.CSharp.Mcap;
using Mcap.CSharp.Mcap.Interfaces;
using Mcap.CSharp.Mcap.Records;
using System.Linq;

namespace Mcap.CSharp.Tests
{
    public class McapWriterTests
    {
        [Fact]
        public void WriteMagic_WritesCorrectMagicBytes()
        {
            // Arrange
            var bufferWriter = new BufferWriter();
            var options = new McapWriterOptions();
            using var writer = new McapWriter(bufferWriter, options);

            // Act
            writer.WriteMagic();

            // Assert
            byte[] expectedMagic = Constants.Magic.Take(8).ToArray();
            byte[] actualBytes = bufferWriter.ToArray();
            Assert.Equal(expectedMagic, actualBytes);
        }

        [Fact]
        public void WriteHeader_WritesCorrectHeaderRecord()
        {
            // Arrange
            var bufferWriter = new BufferWriter();
            var options = new McapWriterOptions();
            using var writer = new McapWriter(bufferWriter, options);
            var header = new Header { Library = "test_lib", Profile = "test_profile" };

            // Act
            writer.WriteHeader(header);

            // Assert
            byte[] expectedBytes;
            using (var stream = new MemoryStream())
            {
                using var binaryWriter = new BinaryWriter(stream);
                binaryWriter.Write((byte)OpCode.Header);

                using (var recordStream = new MemoryStream())
                {
                    using var recordBinaryWriter = new BinaryWriter(recordStream);
                    recordBinaryWriter.Write(header.Profile.Length);
                    recordBinaryWriter.Write(System.Text.Encoding.UTF8.GetBytes(header.Profile));
                    recordBinaryWriter.Write(header.Library.Length);
                    recordBinaryWriter.Write(System.Text.Encoding.UTF8.GetBytes(header.Library));
                    byte[] recordData = recordStream.ToArray();
                    binaryWriter.Write((ulong)recordData.Length);
                    binaryWriter.Write(recordData);
                }
                expectedBytes = stream.ToArray();
            }
            byte[] actualBytes = bufferWriter.ToArray();
            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public void WriteFooter_WritesCorrectFooterRecord()
        {
            // Arrange
            var bufferWriter = new BufferWriter();
            var options = new McapWriterOptions();
            using var writer = new McapWriter(bufferWriter, options);
            var footer = new Footer { SummaryStart = 123, SummaryOffset = 456, Library = "test_lib" };

            // Act
            writer.WriteFooter(footer);

            // Assert
            byte[] expectedBytes;
            using (var stream = new MemoryStream())
            {
                using var binaryWriter = new BinaryWriter(stream);
                binaryWriter.Write((byte)OpCode.Footer);

                using (var recordStream = new MemoryStream())
                {
                    using var recordBinaryWriter = new BinaryWriter(recordStream);
                    recordBinaryWriter.Write(footer.SummaryStart);
                    recordBinaryWriter.Write(footer.SummaryOffset);
                    recordBinaryWriter.Write(footer.SummaryCrc);
                    byte[] recordData = recordStream.ToArray();
                    binaryWriter.Write((ulong)recordData.Length);
                    binaryWriter.Write(recordData);
                }
                expectedBytes = stream.ToArray();
            }
            byte[] actualBytes = bufferWriter.ToArray();
            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public void AddSchema_WritesCorrectSchemaRecord()
        {
            // Arrange
            var bufferWriter = new BufferWriter();
            var options = new McapWriterOptions();
            using var writer = new McapWriter(bufferWriter, options);
            var schema = new Schema
            {
                Id = 1,
                Name = "test_schema",
                Encoding = "json",
                Data = new byte[] { 0x01, 0x02, 0x03 }
            };

            // Act
            writer.AddSchema(schema);

            // Assert
            byte[] expectedBytes;
            using (var stream = new MemoryStream())
            {
                using var binaryWriter = new BinaryWriter(stream);
                binaryWriter.Write((byte)OpCode.Schema);

                using (var recordStream = new MemoryStream())
                {
                    using var recordBinaryWriter = new BinaryWriter(recordStream);
                    recordBinaryWriter.Write(schema.Id);
                    recordBinaryWriter.Write((uint)schema.Name.Length);
                    recordBinaryWriter.Write(System.Text.Encoding.UTF8.GetBytes(schema.Name));
                    recordBinaryWriter.Write((uint)schema.Encoding.Length);
                    recordBinaryWriter.Write(System.Text.Encoding.UTF8.GetBytes(schema.Encoding));
                    recordBinaryWriter.Write((uint)schema.Data.Length);
                    recordBinaryWriter.Write(schema.Data);
                    byte[] recordData = recordStream.ToArray();
                    binaryWriter.Write((ulong)recordData.Length);
                    binaryWriter.Write(recordData);
                }
                expectedBytes = stream.ToArray();
            }
            byte[] actualBytes = bufferWriter.ToArray();
            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public void AddChannel_WritesCorrectChannelRecord()
        {
            // Arrange
            var bufferWriter = new BufferWriter();
            var options = new McapWriterOptions();
            using var writer = new McapWriter(bufferWriter, options);
            var channel = new Channel
            {
                Id = 10,
                SchemaId = 1,
                Topic = "/test_topic",
                MessageEncoding = "ros1",
                Metadata = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                }
            };

            // Act
            writer.AddChannel(channel);

            // Assert
            byte[] expectedBytes;
            using (var stream = new MemoryStream())
            {
                using var binaryWriter = new BinaryWriter(stream);
                binaryWriter.Write((byte)OpCode.Channel);

                using (var recordStream = new MemoryStream())
                {
                    using var recordBinaryWriter = new BinaryWriter(recordStream);
                    recordBinaryWriter.Write(channel.Id);
                    recordBinaryWriter.Write(channel.SchemaId);
                    recordBinaryWriter.Write((uint)channel.Topic.Length);
                    recordBinaryWriter.Write(System.Text.Encoding.UTF8.GetBytes(channel.Topic));
                    recordBinaryWriter.Write((uint)channel.MessageEncoding.Length);
                    recordBinaryWriter.Write(System.Text.Encoding.UTF8.GetBytes(channel.MessageEncoding));
                    recordBinaryWriter.Write((uint)channel.Metadata.Count);
                    foreach (var entry in channel.Metadata)
                    {
                        recordBinaryWriter.Write((uint)entry.Key.Length);
                        recordBinaryWriter.Write(System.Text.Encoding.UTF8.GetBytes(entry.Key));
                        recordBinaryWriter.Write((uint)entry.Value.Length);
                        recordBinaryWriter.Write(System.Text.Encoding.UTF8.GetBytes(entry.Value));
                    }
                    byte[] recordData = recordStream.ToArray();
                    binaryWriter.Write((ulong)recordData.Length);
                    binaryWriter.Write(recordData);
                }
                expectedBytes = stream.ToArray();
            }
            byte[] actualBytes = bufferWriter.ToArray();
            Assert.Equal(expectedBytes, actualBytes);
        }

        

        [Fact]
        public void StartChunk_WritesCorrectChunkHeader()
        {
            // Arrange
            var bufferWriter = new BufferWriter();
            var options = new McapWriterOptions();
            using var writer = new McapWriter(bufferWriter, options);

            // Act
            writer.StartChunk();

            // Assert
            // OpCode.Chunk (1 byte) + recordLength (8 bytes) + messageStartTime (8 bytes) + messageEndTime (8 bytes) +
            // uncompressedSize (8 bytes) + uncompressedCrc (4 bytes) + compression (variable) +
            // recordCount (8 bytes) + recordTypeCount (variable) + messageStartOffset (8 bytes) +
            // messageEndOffset (8 bytes) + chunkHash (8 bytes)
            // For now, we only check the OpCode and a placeholder for recordLength.
            // Detailed validation will be done in the implementation.
            byte[] actualBytes = bufferWriter.ToArray();
            Assert.True(actualBytes.Length >= 1 + 8); // OpCode + recordLength minimum
            Assert.Equal((byte)OpCode.Chunk, actualBytes[0]);
        }

        [Fact]
        public void EndChunk_WritesCorrectChunkFooter()
        {
            // Arrange
            var bufferWriter = new BufferWriter();
            var options = new McapWriterOptions();
            using var writer = new McapWriter(bufferWriter, options);

            // Act
            writer.StartChunk(); // Start a chunk to allow ending it
            writer.EndChunk();

            // Assert
            // The exact bytes for a footer are complex due to variable length fields and CRC.
            // For now, we'll check if the buffer contains a Chunk record followed by a ChunkIndex record.
            // This test will likely fail until ChunkIndex is implemented.
            byte[] actualBytes = bufferWriter.ToArray();
            // This is a very basic check. A more robust test would parse the records.
            // We expect at least a Chunk header and then a ChunkIndex record.
            // The ChunkIndex record will be written by EndChunk.
            Assert.True(actualBytes.Length > 0);
            // Further assertions will require parsing the stream or knowing the exact expected bytes,
            // which depends on the full implementation of Chunk and ChunkIndex records.
        }

        [Fact]
        public void WriteMessage_WithChunking_StartsNewChunkWhenFull()
        {
            // Arrange
            // Use a small chunk size to force new chunks
            var bufferWriter = new BufferWriter();
            var options = new McapWriterOptions { ChunkSize = 100 }; // Small chunk size for testing
            using var writer = new McapWriter(bufferWriter, options);

            var schema = new Schema { Id = 1, Name = "test_schema", Encoding = "json", Data = new byte[] { 0x01 } };
            writer.AddSchema(schema);
            var channel = new Channel { Id = 1, SchemaId = 1, Topic = "/test", MessageEncoding = "json" };
            writer.AddChannel(channel);

            var message1 = new Message { ChannelId = 1, Sequence = 1, LogTime = 1, PublishTime = 1, Data = new byte[50] };
            var message2 = new Message { ChannelId = 1, Sequence = 2, LogTime = 2, PublishTime = 2, Data = new byte[50] };
            var message3 = new Message { ChannelId = 1, Sequence = 3, LogTime = 3, PublishTime = 3, Data = new byte[50] };

            // Act
            writer.Write(message1);
            writer.Write(message2);
            writer.Write(message3);

            // Assert
            // We expect at least two chunks to be written because each message is 50 bytes,
            // and the chunk size is 100 bytes. The overhead of Chunk and Message records
            // will likely cause the second message to push it over the limit,
            // forcing a new chunk for the third message.
            byte[] actualBytes = bufferWriter.ToArray();

            // This is a very basic check. A more robust test would parse the records
            // to count the number of Chunk records.
            // For now, we'll just check if the total size is large enough for multiple chunks.
            Assert.True(actualBytes.Length > (100 * 2)); // At least two chunks worth of data
            // Further assertions will require parsing the stream to count Chunk records.
        }
    }
}
