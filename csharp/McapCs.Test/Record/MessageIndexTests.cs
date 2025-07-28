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

        // Manually calculate recordsSize
        ulong recordsSize = (ulong)(
            8 + 8 + // LOG_TIME_1 + OFFSET_1
            8 + 8   // LOG_TIME_2 + OFFSET_2
        );

        ulong recordSize = 2UL + 4UL + recordsSize;
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ushort)1);
        expectedWriter.Write((uint)recordsSize);
        expectedWriter.Write(LOG_TIME_1);
        expectedWriter.Write(OFFSET_1);
        expectedWriter.Write(LOG_TIME_2);
        expectedWriter.Write(OFFSET_2);
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, messageIndex);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}