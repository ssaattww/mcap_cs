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
    }
}
