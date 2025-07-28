using McapCs.Record;
using McapCs.Writer;

namespace McapCs.Test.Record;

public class DataEndTests
{
    [Fact]
    public void TestDataEndWrite()
    {
        // Arrange
        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var dataEnd = new DataEnd
        {
            dataSectionCrc = 0x12345678
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.DataEnd);
        expectedWriter.Write(4UL); // recordSize is 4 bytes
        expectedWriter.Write((uint)0x12345678);
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, dataEnd);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}