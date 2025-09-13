using McapCs.Record;
using McapCs.Writer;
using System.Text;

namespace McapCs.Test.Record;

public class MetadataIndexTests
{
    [Fact]
    public void TestMetadataIndexWrite()
    {
        // Arrange
        const string METADATA_NAME = "test_metadata";

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var metadataIndex = new MetadataIndex
        {
            offset = 100,
            length = 200,
            name = METADATA_NAME
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.MetadataIndex);
        // recordSize: offset (8 bytes) + length (8 bytes) + name length (4 bytes) + name string size
        ulong recordSize = 8UL + 8UL + 4UL + (ulong)Encoding.UTF8.GetByteCount(METADATA_NAME);
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)100); // offset
        expectedWriter.Write((ulong)200); // length
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(METADATA_NAME)); // name length
        expectedWriter.Write(Encoding.UTF8.GetBytes(METADATA_NAME)); // name data
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, metadataIndex);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}