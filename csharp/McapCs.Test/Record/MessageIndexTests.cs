using McapCs.Record;
using McapCs.Writer;
using System;
using System.Collections.Generic;

namespace McapCs.Test.Record;

public class MessageIndexTests
{
    [Fact]
    public void TestMessageIndexWrite()
    {
        // Arrange
        const ulong LOG_TIME_1 = 10;
        const ulong OFFSET_1 = 100;
        const ulong LOG_TIME_2 = 20;
        const ulong OFFSET_2 = 200;

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var messageIndex = new MessageIndex
        {
            channelId = 1,
            records = new List<Tuple<ulong, ulong>>
            {
                new Tuple<ulong, ulong>(LOG_TIME_1, OFFSET_1),
                new Tuple<ulong, ulong>(LOG_TIME_2, OFFSET_2)
            }
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.MessageIndex);

        // Manually calculate recordsSize: number of records * (timestamp (8 bytes) + offset (8 bytes))
        ulong recordsSize = 2UL * (8UL + 8UL);

        // recordSize: channelId (2 bytes) + recordsSize length (4 bytes) + records bytes size
        ulong recordSize = 2UL + 4UL + recordsSize;
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ushort)1); // channelId
        expectedWriter.Write((uint)recordsSize); // recordsSize length
        expectedWriter.Write(LOG_TIME_1); // timestamp
        expectedWriter.Write(OFFSET_1); // offset
        expectedWriter.Write(LOG_TIME_2); // timestamp
        expectedWriter.Write(OFFSET_2); // offset
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, messageIndex);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}