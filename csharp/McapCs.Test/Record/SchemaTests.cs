using McapCs.Record;
using McapCs.Writer;
using System.Text;

namespace McapCs.Test.Record;

public class SchemaTests
{
    [Fact]
    public void TestSchemaWrite()
    {
        // Arrange
        const ushort SCHEMA_ID = 1;
        const string SCHEMA_NAME = "test_schema";
        const string ENCODING = "test_encoding";
        const int DATA_SIZE = 5;
        byte[] DATA = new byte[] { 1, 2, 3, 4, 5 };

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var schema = new Schema
        {
            id = SCHEMA_ID,
            name = SCHEMA_NAME,
            encoding = ENCODING,
            data = new List<byte>(DATA)
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.Schema);
        // recordSize: id (2 bytes) + name length (4 bytes) + name string size + encoding length (4 bytes) + encoding string size + data length (4 bytes) + data bytes size
        ulong recordSize = 2UL + 4UL + (ulong)Encoding.UTF8.GetByteCount(SCHEMA_NAME) + 4UL + (ulong)Encoding.UTF8.GetByteCount(ENCODING) + 4UL + (ulong)DATA_SIZE;
        expectedWriter.Write(recordSize);
        expectedWriter.Write(SCHEMA_ID); // id
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(SCHEMA_NAME)); // name length
        expectedWriter.Write(Encoding.UTF8.GetBytes(SCHEMA_NAME)); // name data
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(ENCODING)); // encoding length
        expectedWriter.Write(Encoding.UTF8.GetBytes(ENCODING)); // encoding data
        expectedWriter.Write((uint)DATA_SIZE); // data length
        expectedWriter.Write(DATA); // data bytes
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, schema);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}