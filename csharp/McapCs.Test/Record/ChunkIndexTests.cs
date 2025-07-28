
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

        // Manually calculate messageIndexOffsetsSize
        ulong messageIndexOffsetsSize = (
            2UL + 8UL + // CHANNEL_ID_1 + OFFSET_1
            2UL + 8UL   // CHANNEL_ID_2 + OFFSET_2
        );

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
        expectedWriter.Write((ulong)100);
        expectedWriter.Write((ulong)200);
        expectedWriter.Write((ulong)1024);
        expectedWriter.Write((ulong)2048);
        expectedWriter.Write((uint)messageIndexOffsetsSize);
        // Manually write sorted messageIndexOffsets
        // Note: The order is by key in MCAP spec
        expectedWriter.Write(CHANNEL_ID_1);
        expectedWriter.Write(OFFSET_1);
        expectedWriter.Write(CHANNEL_ID_2);
        expectedWriter.Write(OFFSET_2);
        expectedWriter.Write((ulong)1000);
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(COMPRESSION));
        expectedWriter.Write(Encoding.UTF8.GetBytes(COMPRESSION));
        expectedWriter.Write((ulong)512);
        expectedWriter.Write((ulong)1024);
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, chunkIndex);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}
