using McapCs.Record;
using McapCs.Writer;

namespace McapCs.Test.Record;

public class MessageTests
{
    [Fact]
    public void TestMessageWrite()
    {
        // Arrange
        const int DATA_SIZE = 5; // Define data size as a constant
        byte[] DATA = new byte[] { 1, 2, 3, 4, 5 };

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var message = new Message
        {
            channelId = 1,
            sequence = 2,
            logTime = 1234567890,
            publishTime = 9876543210,
            data = new List<byte>(DATA)
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.Message);
        ulong recordSize = 2UL + 4UL + 8UL + 8UL + 4UL + (ulong)DATA_SIZE; // Use DATA_SIZE here
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ushort)1);
        expectedWriter.Write((uint)2);
        expectedWriter.Write((ulong)1234567890);
        expectedWriter.Write((ulong)9876543210);
        expectedWriter.Write((uint)DATA_SIZE); // Use DATA_SIZE here
        expectedWriter.Write(DATA);
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, message);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}