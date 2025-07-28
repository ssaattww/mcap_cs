using McapCs.Record;
using McapCs.Writer;
using System.Text;

namespace McapCs.Test.Record;

public class ChunkTests
{
    [Fact]
    public void TestChunkWrite()
    {
        // Arrange
        const string COMPRESSION = "zstd";
        const int RECORDS_SIZE = 10;
        byte[] RECORDS = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var chunk = new Chunk
        {
            messageStartTime = 100,
            messageEndTime = 200,
            uncompressedSize = 1024,
            uncompressedCrc = 0x12345678,
            compression = COMPRESSION,
            compressedSize = 10,
            records = new List<byte>(RECORDS)
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.Chunk);
        ulong recordSize = 8UL + 8UL + 8UL + 4UL + 4UL + (ulong)Encoding.UTF8.GetByteCount(COMPRESSION) + 8UL + (ulong)RECORDS_SIZE;
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)100);
        expectedWriter.Write((ulong)200);
        expectedWriter.Write((ulong)1024);
        expectedWriter.Write((uint)0x12345678);
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(COMPRESSION));
        expectedWriter.Write(Encoding.UTF8.GetBytes(COMPRESSION));
        expectedWriter.Write((ulong)RECORDS_SIZE);
        expectedWriter.Write(RECORDS);
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, chunk);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}