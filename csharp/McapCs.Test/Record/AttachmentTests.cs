
using McapCs.Record;
using McapCs.Writer;
using System.Text;

namespace McapCs.Test.Record;

public class AttachmentTests
{
    [Fact]
    public void TestAttachmentWrite()
    {
        // Arrange
        const int DATA_SIZE = 5;
        const string ATTACHMENT_NAME = "test_attachment";
        const string MEDIA_TYPE = "text/plain";

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var attachment = new Attachment
        {
            logTime = 12345,
            createTime = 67890,
            name = ATTACHMENT_NAME,
            mediaType = MEDIA_TYPE,
            data = new List<byte>(new byte[] { 1, 2, 3, 4, 5 }),
            dataSize = DATA_SIZE,
            crc = 0x12345678
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.Attachment);

        // Calculate recordSize using hardcoded string byte counts
        ulong recordSize = (
            8UL + // logTime
            8UL + // createTime
            4UL + (ulong)Encoding.UTF8.GetByteCount(ATTACHMENT_NAME) + // name length + name data
            4UL + (ulong)Encoding.UTF8.GetByteCount(MEDIA_TYPE) +     // mediaType length + mediaType data
            8UL + // dataSize
            (ulong)DATA_SIZE + // data bytes
            4UL   // crc
        );
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ulong)12345); // logTime
        expectedWriter.Write((ulong)67890); // createTime
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(ATTACHMENT_NAME)); // name length
        expectedWriter.Write(Encoding.UTF8.GetBytes(ATTACHMENT_NAME)); // name data
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(MEDIA_TYPE)); // mediaType length
        expectedWriter.Write(Encoding.UTF8.GetBytes(MEDIA_TYPE)); // mediaType data
        expectedWriter.Write((ulong)DATA_SIZE); // dataSize
        expectedWriter.Write(attachment.data.ToArray()); // data bytes
        expectedWriter.Write((uint)0x12345678); // crc
        var expectedBytes = expectedStream.ToArray();

        // Act
        McapWriter.Write(writable, attachment);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}
