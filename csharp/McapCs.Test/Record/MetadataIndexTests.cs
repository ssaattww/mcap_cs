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
        const string METADATA_NAME = "test_metadata"; // Add this line
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
        ulong recordSize = 8UL + 8UL + 4UL + (ulong)Encoding.UTF8.GetByteCount(METADATA_NAME);
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)100);
        expectedWriter.Write((ulong)200);
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(METADATA_NAME));
        expectedWriter.Write(Encoding.UTF8.GetBytes(METADATA_NAME));
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, metadataIndex);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}