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
        // recordSize: channelId (2 bytes) + sequence (4 bytes) + logTime (8 bytes) + publishTime (8 bytes) + data bytes size
        ulong recordSize = 2UL + 4UL + 8UL + 8UL + (ulong)DATA_SIZE;
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ushort)1); // channelId
        expectedWriter.Write((uint)2); // sequence
        expectedWriter.Write((ulong)1234567890); // logTime
        expectedWriter.Write((ulong)9876543210); // publishTime
        expectedWriter.Write(DATA); // data bytes（データ長は書かない仕様）
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, message);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}
