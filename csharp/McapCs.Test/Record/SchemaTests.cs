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
        ulong recordSize = 2UL + 4UL + (ulong)Encoding.UTF8.GetByteCount(SCHEMA_NAME) + 4UL + (ulong)Encoding.UTF8.GetByteCount(ENCODING) + 4UL + (ulong)DATA_SIZE;
        expectedWriter.Write(recordSize);
        expectedWriter.Write(SCHEMA_ID);
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(SCHEMA_NAME));
        expectedWriter.Write(Encoding.UTF8.GetBytes(SCHEMA_NAME));
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(ENCODING));
        expectedWriter.Write(Encoding.UTF8.GetBytes(ENCODING));
        expectedWriter.Write((uint)DATA_SIZE);
        expectedWriter.Write(DATA);
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, schema);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}