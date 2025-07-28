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
        expectedWriter.Write(20UL); // recordSize is 20 bytes (8 + 8 + 4)
        expectedWriter.Write((ulong)12345);
        expectedWriter.Write((ulong)67890);
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