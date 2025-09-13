
using McapCs.Record;
using McapCs.Writer;
using System.Text;
using System.Collections.Generic;

namespace McapCs.Test.Record;

public class ChunkIndexTests
{
    [Fact]
    public void TestChunkIndexWrite()
    {
        // Arrange
        const ushort CHANNEL_ID_1 = 1;
        const ulong OFFSET_1 = 3000;
        const ushort CHANNEL_ID_2 = 2;
        const ulong OFFSET_2 = 4000;
        const string COMPRESSION = "zstd";

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var chunkIndex = new ChunkIndex
        {
            messageStartTime = 100,
            messageEndTime = 200,
            chunkStartOffset = 1024,
            chunkLength = 2048,
            messageIndexOffsets = new Dictionary<ushort, ulong>
            {
                { CHANNEL_ID_1, OFFSET_1 },
                { CHANNEL_ID_2, OFFSET_2 }
            },
            messageIndexLength = 1000,
            compression = COMPRESSION,
            compressedSize = 512,
            uncompressedSize = 1024
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.ChunkIndex);

        // Manually calculate messageIndexOffsetsSize: number of message index offsets * (channelId (2 bytes) + offset (8 bytes))
        ulong messageIndexOffsetsSize = 2UL * (2UL + 8UL);

        // recordSize: messageStartTime (8 bytes) + messageEndTime (8 bytes) + chunkStartOffset (8 bytes) + chunkLength (8 bytes) + messageIndexOffsets length (4 bytes) + messageIndexOffsets bytes size + messageIndexLength (8 bytes) + compression length (4 bytes) + compression string size + compressedSize (8 bytes) + uncompressedSize (8 bytes)
        ulong recordSize = (
            8UL + // messageStartTime
            8UL + // messageEndTime
            8UL + // chunkStartOffset
            8UL + // chunkLength
            4UL + messageIndexOffsetsSize + // messageIndexOffsets length + data
            8UL + // messageIndexLength
            4UL + (ulong)Encoding.UTF8.GetByteCount(COMPRESSION) + // compression length + data
            8UL + // compressedSize
            8UL   // uncompressedSize
        );
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)100); // messageStartTime
        expectedWriter.Write((ulong)200); // messageEndTime
        expectedWriter.Write((ulong)1024); // chunkStartOffset
        expectedWriter.Write((ulong)2048); // chunkLength
        expectedWriter.Write((uint)messageIndexOffsetsSize); // messageIndexOffsets length
        // Manually write sorted messageIndexOffsets
        // Note: The order is by key in MCAP spec
        expectedWriter.Write(CHANNEL_ID_1); // channelId
        expectedWriter.Write(OFFSET_1); // offset
        expectedWriter.Write(CHANNEL_ID_2); // channelId
        expectedWriter.Write(OFFSET_2); // offset
        expectedWriter.Write((ulong)1000); // messageIndexLength
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(COMPRESSION)); // compression length
        expectedWriter.Write(Encoding.UTF8.GetBytes(COMPRESSION)); // compression string
        expectedWriter.Write((ulong)512); // compressedSize
        expectedWriter.Write((ulong)1024); // uncompressedSize
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, chunkIndex);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}
