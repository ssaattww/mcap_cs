
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

        // Manually calculate channelMessageCountsSize: number of channel message counts * (channelId (2 bytes) + messageCount (8 bytes))
        ulong channelMessageCountsSize = 2UL * (2UL + 8UL);

        // recordSize: message_count (8 bytes) + schema_count (2 bytes) + channel_count (4 bytes) + attachment_count (4 bytes) + metadata_count (4 bytes) + chunk_count (4 bytes) + message_start_time (8 bytes) + message_end_time (8 bytes) + channel_message_counts length (4 bytes) + channel_message_counts bytes size
        ulong recordSize = 
            8UL + // messageCount
            2UL + // schemaCount
            4UL + // channelCount
            4UL + // attachmentCount
            4UL + // metadataCount
            4UL + // chunkCount
            8UL + // messageStartTime
            8UL + // messageEndTime
            4UL + channelMessageCountsSize; // channelMessageCounts length + data
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)100); // messageCount
        expectedWriter.Write((ushort)5); // schemaCount
        expectedWriter.Write((uint)10); // channelCount
        expectedWriter.Write((uint)2); // attachmentCount
        expectedWriter.Write((uint)3); // metadataCount
        expectedWriter.Write((uint)4); // chunkCount
        expectedWriter.Write((ulong)1000); // messageStartTime
        expectedWriter.Write((ulong)2000); // messageEndTime
        expectedWriter.Write((uint)channelMessageCountsSize); // channelMessageCounts length
        // Manually write sorted channelMessageCounts
        // Note: The order is by key in MCAP spec
        expectedWriter.Write(CHANNEL_ID_1); // channelId
        expectedWriter.Write(MESSAGE_COUNT_1); // messageCount
        expectedWriter.Write(CHANNEL_ID_2); // channelId
        expectedWriter.Write(MESSAGE_COUNT_2); // messageCount
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, statistics);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}
