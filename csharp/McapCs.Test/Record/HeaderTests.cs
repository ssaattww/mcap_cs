using McapCs.Record;
using McapCs.Writer;
using System.Text;

namespace McapCs.Test.Record;

public class HeaderTests
{
    [Fact]
    public void TestHeaderWrite()
    {
        // Arrange
        const string PROFILE = "test_profile";
        const string LIBRARY = "test_library";

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var header = new Header
        {
            profile = PROFILE,
            library = LIBRARY
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.Header);
        ulong recordSize = 4UL + (ulong)Encoding.UTF8.GetByteCount(PROFILE) + 4UL + (ulong)Encoding.UTF8.GetByteCount(LIBRARY);
        expectedWriter.Write(recordSize);
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(PROFILE));
        expectedWriter.Write(Encoding.UTF8.GetBytes(PROFILE));
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(LIBRARY));
        expectedWriter.Write(Encoding.UTF8.GetBytes(LIBRARY));
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, header);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}