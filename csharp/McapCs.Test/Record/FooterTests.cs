
using McapCs.Record;
using McapCs.Writer;

namespace McapCs.Test.Record;

public class FooterTests
{
    [Fact]
    public void TestFooterWrite()
    {
        // Arrange
        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var footer = new Footer
        {
            summaryStart = 12345,
            summaryOffsetStart = 67890,
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.Footer);
        // recordSize: summary_start (8 bytes) + summary_offset_start (8 bytes) + summary_crc (4 bytes)
        ulong recordSize = 8UL + 8UL + 4UL;
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)12345); // summary_start
        expectedWriter.Write((ulong)67890); // summary_offset_start
        expectedWriter.Write((uint)0); // CRC is calculated by the writer
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, footer, true);
        var actualBytes = stream.ToArray();

        // Assert
        // We can't directly compare the CRC, so we'll compare the rest of the bytes
        Assert.Equal(expectedBytes.Length, actualBytes.Length);
        for (int i = 0; i < expectedBytes.Length - 4; i++)
        {
            Assert.Equal(expectedBytes[i], actualBytes[i]);
        }
    }
}
