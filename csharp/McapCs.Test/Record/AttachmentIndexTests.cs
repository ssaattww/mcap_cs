
using McapCs.Record;
using McapCs.Writer;
using System.Text;

namespace McapCs.Test.Record;

public class AttachmentIndexTests
{
    [Fact]
    public void TestAttachmentIndexWrite()
    {
        // Arrange
        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var attachmentIndex = new AttachmentIndex
        {
            offset = 100,
            length = 200,
            logTime = 12345,
            createTime = 67890,
            dataSize = 5,
            name = "test_attachment",
            mediaType = "text/plain"
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.AttachmentIndex);
        ulong recordSize = (ulong)(8 + 8 + 8 + 8 + 8 + (ulong)4 + (ulong)Encoding.UTF8.GetByteCount(attachmentIndex.name) + (ulong)4 + (ulong)Encoding.UTF8.GetByteCount(attachmentIndex.mediaType));
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)100);
        expectedWriter.Write((ulong)200);
        expectedWriter.Write((ulong)12345);
        expectedWriter.Write((ulong)67890);
        expectedWriter.Write((ulong)5);
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(attachmentIndex.name));
        expectedWriter.Write(Encoding.UTF8.GetBytes(attachmentIndex.name));
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(attachmentIndex.mediaType));
        expectedWriter.Write(Encoding.UTF8.GetBytes(attachmentIndex.mediaType));
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, attachmentIndex);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}
