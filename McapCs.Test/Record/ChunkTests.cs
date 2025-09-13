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
        // recordSize: messageStartTime (8 bytes) + messageEndTime (8 bytes) + uncompressedSize (8 bytes) + uncompressedCrc (4 bytes) + compression length (4 bytes) + compression string size + compressedSize (8 bytes) + records bytes size
        ulong recordSize = 8UL + 8UL + 8UL + 4UL + 4UL + (ulong)Encoding.UTF8.GetByteCount(COMPRESSION) + 8UL + (ulong)RECORDS_SIZE;
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)100); // messageStartTime
        expectedWriter.Write((ulong)200); // messageEndTime
        expectedWriter.Write((ulong)1024); // uncompressedSize
        expectedWriter.Write((uint)0x12345678); // uncompressedCrc
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(COMPRESSION)); // compression length
        expectedWriter.Write(Encoding.UTF8.GetBytes(COMPRESSION)); // compression string
        expectedWriter.Write((ulong)RECORDS_SIZE); // compressedSize (Note: this is actually records size in this test)
        expectedWriter.Write(RECORDS); // records bytes
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, chunk);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}