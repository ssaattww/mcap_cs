using McapCs.Record;
using McapCs.Writer;
using System.Text;
using System.Collections.Generic;

namespace McapCs.Test.Record;

public class MetadataTests
{
    [Fact]
    public void TestMetadataWrite()
    {
        // Arrange
        const string METADATA_NAME = "test_metadata";
        const string KEY1 = "key1";
        const string VALUE1 = "value1";
        const string KEY2 = "key2";
        const string VALUE2 = "value2";

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var metadata = new Metadata
        {
            name = METADATA_NAME,
            metadata = new Dictionary<string, string>
            {
                { KEY1, VALUE1 },
                { KEY2, VALUE2 }
            }
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.Metadata);

        // Manually calculate metadataContentSize based on hardcoded strings
        ulong metadataContentSize = (
            4UL + (ulong)Encoding.UTF8.GetByteCount(KEY1) + 4UL + (ulong)Encoding.UTF8.GetByteCount(VALUE1) +
            4UL + (ulong)Encoding.UTF8.GetByteCount(KEY2) + 4UL + (ulong)Encoding.UTF8.GetByteCount(VALUE2)
        );

        ulong recordSize = (
            4UL + (ulong)Encoding.UTF8.GetByteCount(METADATA_NAME) + // name length + name data
            4UL + metadataContentSize // metadata length + metadata data
        );
        expectedWriter.Write(recordSize);
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(METADATA_NAME)); // name length
        expectedWriter.Write(Encoding.UTF8.GetBytes(METADATA_NAME)); // name data
        expectedWriter.Write((uint)metadataContentSize); // metadata length
        // Manually write sorted metadata
        // Note: The order of metadata is lexicographical by key in MCAP spec
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(KEY1));
        expectedWriter.Write(Encoding.UTF8.GetBytes(KEY1));
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(VALUE1));
        expectedWriter.Write(Encoding.UTF8.GetBytes(VALUE1));
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(KEY2));
        expectedWriter.Write(Encoding.UTF8.GetBytes(KEY2));
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(VALUE2));
        expectedWriter.Write(Encoding.UTF8.GetBytes(VALUE2));
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, metadata);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}