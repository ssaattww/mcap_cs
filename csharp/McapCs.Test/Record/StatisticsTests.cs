
using McapCs.Record;
using McapCs.Writer;
using System.Collections.Generic;

namespace McapCs.Test.Record;

public class StatisticsTests
{
    [Fact]
    public void TestStatisticsWrite()
    {
        // Arrange
        const ushort CHANNEL_ID_1 = 1;
        const ulong MESSAGE_COUNT_1 = 50;
        const ushort CHANNEL_ID_2 = 2;
        const ulong MESSAGE_COUNT_2 = 50;

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var statistics = new Statistics
        {
            messageCount = 100,
            schemaCount = 5,
            channelCount = 10,
            attachmentCount = 2,
            metadataCount = 3,
            chunkCount = 4,
            messageStartTime = 1000,
            messageEndTime = 2000,
            channelMessageCounts = new Dictionary<ushort, ulong>
            {
                { CHANNEL_ID_1, MESSAGE_COUNT_1 },
                { CHANNEL_ID_2, MESSAGE_COUNT_2 }
            }
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.Statistics);

        // Manually calculate channelMessageCountsSize
        ulong channelMessageCountsSize = (
            2UL + 8UL + // CHANNEL_ID_1 + MESSAGE_COUNT_1
            2UL + 8UL   // CHANNEL_ID_2 + MESSAGE_COUNT_2
        );

        ulong recordSize = (
            8UL + // messageCount
            2UL + // schemaCount
            4UL + // channelCount
            4UL + // attachmentCount
            4UL + // metadataCount
            4UL + // chunkCount
            8UL + // messageStartTime
            8UL + // messageEndTime
            4UL + channelMessageCountsSize // channelMessageCounts length + data
        );
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)100);
        expectedWriter.Write((ushort)5);
        expectedWriter.Write((uint)10);
        expectedWriter.Write((uint)2);
        expectedWriter.Write((uint)3);
        expectedWriter.Write((uint)4);
        expectedWriter.Write((ulong)1000);
        expectedWriter.Write((ulong)2000);
        expectedWriter.Write((uint)channelMessageCountsSize);
        // Manually write sorted channelMessageCounts
        // Note: The order is by key in MCAP spec
        expectedWriter.Write(CHANNEL_ID_1);
        expectedWriter.Write(MESSAGE_COUNT_1);
        expectedWriter.Write(CHANNEL_ID_2);
        expectedWriter.Write(MESSAGE_COUNT_2);
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, statistics);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}
