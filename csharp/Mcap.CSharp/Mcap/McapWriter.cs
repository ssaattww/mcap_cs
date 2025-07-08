using System;
using System.IO;
using Mcap.CSharp.Mcap.Interfaces;
using Mcap.CSharp.Mcap.Records;

namespace Mcap.CSharp.Mcap
{
    public class McapWriter : IDisposable
    {
        private readonly IWritable _writer;
        private readonly McapWriterOptions _options;

        public McapWriter(IWritable writer, McapWriterOptions options)
        {
            _writer = writer;
            _options = options;
        }

        public void Dispose()
        {
            // Write the footer before disposing
            WriteFooter(new Footer { SummaryStart = _writer.Size(), SummaryOffset = 0, SummaryCrc = 0 }); // TBD: Populate actual values
            _writer.End();
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

        private void WriteRecord<T>(OpCode opCode, T record) where T : IWritable
        {
            // TBD: Serialize the record into a byte array
            using var recordStream = new MemoryStream();
            using var recordBinaryWriter = new BinaryWriter(recordStream);

            // Serialize the record content into the MemoryStream
            // This part needs to be specific to each record type, or use a common serialization mechanism
            // For now, we'll assume 'record' has a method to write its content to a BinaryWriter
            // For Header, we'll manually serialize it here for demonstration
            if (record is Header headerRecord)
            {
                recordBinaryWriter.Write(headerRecord.Profile.Length);
                recordBinaryWriter.Write(System.Text.Encoding.UTF8.GetBytes(headerRecord.Profile));
                recordBinaryWriter.Write(headerRecord.Library.Length);
                recordBinaryWriter.Write(System.Text.Encoding.UTF8.GetBytes(headerRecord.Library));
            }
            else if (record is Footer footerRecord)
            {
                recordBinaryWriter.Write(footerRecord.SummaryStart);
                recordBinaryWriter.Write(footerRecord.SummaryOffset);
                recordBinaryWriter.Write(footerRecord.SummaryCrc);
            }
            else
            {
                throw new NotImplementedException($"Serialization for record type {record.GetType().Name} is not implemented.");
            }

            byte[] recordBytes = recordStream.ToArray();

            _writer.Write(new byte[] { (byte)opCode }, 1); // Write OpCode
            byte[] recordLengthBytes = BitConverter.GetBytes((ulong)recordBytes.Length);
            _writer.Write(recordLengthBytes, (ulong)recordLengthBytes.Length); // Write record size
            _writer.Write(recordBytes, (ulong)recordBytes.Length); // Write record data
        }
    }
}
