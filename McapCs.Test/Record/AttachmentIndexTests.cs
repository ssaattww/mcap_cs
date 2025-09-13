
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
        const string ATTACHMENT_NAME = "test_attachment";
        const string MEDIA_TYPE = "text/plain";

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var attachmentIndex = new AttachmentIndex
        {
            offset = 100,
            length = 200,
            logTime = 12345,
            createTime = 67890,
            dataSize = 5,
            name = ATTACHMENT_NAME,
            mediaType = MEDIA_TYPE
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.AttachmentIndex);
        // recordSize: offset (8 bytes) + length (8 bytes) + log_time (8 bytes) + create_time (8 bytes) + data_size (8 bytes) + name length (4 bytes) + name string size + media_type length (4 bytes) + media_type string size
        ulong recordSize = 8UL + 8UL + 8UL + 8UL + 8UL + 4UL + (ulong)Encoding.UTF8.GetByteCount(ATTACHMENT_NAME) + 4UL + (ulong)Encoding.UTF8.GetByteCount(MEDIA_TYPE);
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)100); // offset
        expectedWriter.Write((ulong)200); // length
        expectedWriter.Write((ulong)12345); // logTime
        expectedWriter.Write((ulong)67890); // createTime
        expectedWriter.Write((ulong)5); // dataSize
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(ATTACHMENT_NAME)); // name length
        expectedWriter.Write(Encoding.UTF8.GetBytes(ATTACHMENT_NAME)); // name data
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(MEDIA_TYPE)); // mediaType length
        expectedWriter.Write(Encoding.UTF8.GetBytes(MEDIA_TYPE)); // mediaType data
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, attachmentIndex);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}
