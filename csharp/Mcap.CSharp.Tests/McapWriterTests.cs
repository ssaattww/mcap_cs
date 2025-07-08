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
        public void WriteMessage_WritesCorrectMessageRecord()
        {
            // Arrange
            var bufferWriter = new BufferWriter();
            var options = new McapWriterOptions();
            using var writer = new McapWriter(bufferWriter, options);
            var message = new Message
            {
                ChannelId = 100,
                Sequence = 1,
                LogTime = 1234567890123456789UL,
                PublishTime = 9876543210987654321UL,
                Data = new byte[] { 0x04, 0x05, 0x06 }
            };

            // Act
            writer.Write(message);

            // Assert
            byte[] expectedBytes;
            using (var stream = new MemoryStream())
            {
                using var binaryWriter = new BinaryWriter(stream);
                binaryWriter.Write((byte)OpCode.Message);

                using (var recordStream = new MemoryStream())
                {
                    using var recordBinaryWriter = new BinaryWriter(recordStream);
                    recordBinaryWriter.Write(message.ChannelId);
                    recordBinaryWriter.Write(message.Sequence);
                    recordBinaryWriter.Write(message.LogTime);
                    recordBinaryWriter.Write(message.PublishTime);
                    recordBinaryWriter.Write((uint)message.Data.Length);
                    recordBinaryWriter.Write(message.Data);
                    byte[] recordData = recordStream.ToArray();
                    binaryWriter.Write((ulong)recordData.Length);
                    binaryWriter.Write(recordData);
                }
                expectedBytes = stream.ToArray();
            }
            byte[] actualBytes = bufferWriter.ToArray();
            Assert.Equal(expectedBytes, actualBytes);
        }
    }
}
