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
            WriteRecord(OpCode.Message, message);
        }

        private void WriteRecord<T>(OpCode opCode, T record) where T : IWritable, IRecordSerializable
        {
            using var recordStream = new MemoryStream();
            using var recordBinaryWriter = new BinaryWriter(recordStream);

            record.Write(recordBinaryWriter);
            byte[] recordBytes = recordStream.ToArray();

            _writer.Write(new byte[] { (byte)opCode }, 1); // Write OpCode
            byte[] recordLengthBytes = BitConverter.GetBytes((ulong)recordBytes.Length);
            _writer.Write(recordLengthBytes, (ulong)recordLengthBytes.Length); // Write record size
            _writer.Write(recordBytes, (ulong)recordBytes.Length); // Write record data
        }
    }
}
