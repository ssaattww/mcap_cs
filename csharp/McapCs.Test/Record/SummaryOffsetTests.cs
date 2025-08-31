using McapCs.Record;
using McapCs.Writer;

namespace McapCs.Test.Record;

public class SummaryOffsetTests
{
    [Fact]
    public void TestSummaryOffsetWrite()
    {
        // Arrange
        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var summaryOffset = new SummaryOffset
        {
            groupOpCode = EOpCode.Chunk,
            groupStart = 100,
            groupLength = 200
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.SummaryOffset);
        // recordSize: group_opcode (1 byte) + group_start (8 bytes) + group_length (8 bytes)
        ulong recordSize = 1UL + 8UL + 8UL;
        expectedWriter.Write(recordSize);
        expectedWriter.Write((byte)EOpCode.Chunk); // groupOpCode
        expectedWriter.Write((ulong)100); // groupStart
        expectedWriter.Write((ulong)200); // groupLength
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, summaryOffset);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}