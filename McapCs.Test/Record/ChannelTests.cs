using McapCs.Record;
using McapCs.Writer;
using System.Text;
using System.Collections.Generic;

namespace McapCs.Test.Record;

public class ChannelTests
{
    [Fact]
    public void TestChannelWrite()
    {
        // Arrange
        const string TOPIC = "test_topic";
        const string MESSAGE_ENCODING = "test_encoding";
        const string KEY1 = "key1";
        const string VALUE1 = "value1";
        const string KEY2 = "key2";
        const string VALUE2 = "value2";

        var stream = new MemoryStream();
        var writable = new McapCs.Writer.StreamWriter(stream);
        var channel = new Channel
        {
            id = 1,
            schemaId = 2,
            topic = TOPIC,
            messageEncoding = MESSAGE_ENCODING,
            metadata = new Dictionary<string, string>
            {
                { KEY1, VALUE1 },
                { KEY2, VALUE2 }
            }
        };

        var expectedStream = new MemoryStream();
        var expectedWriter = new BinaryWriter(expectedStream);
        expectedWriter.Write((byte)EOpCode.Channel);

        // Manually calculate metadataSize based on hardcoded strings
        ulong metadataSize = (
            4UL + (ulong)Encoding.UTF8.GetByteCount(KEY1) + 4UL + (ulong)Encoding.UTF8.GetByteCount(VALUE1) +
            4UL + (ulong)Encoding.UTF8.GetByteCount(KEY2) + 4UL + (ulong)Encoding.UTF8.GetByteCount(VALUE2)
        );

        // Calculate recordSize using hardcoded string byte counts
        ulong recordSize = (
            2UL + // id
            2UL + // schemaId
            4UL + (ulong)Encoding.UTF8.GetByteCount(TOPIC) + // topic length + topic data
            4UL + (ulong)Encoding.UTF8.GetByteCount(MESSAGE_ENCODING) + // messageEncoding length + messageEncoding data
            4UL + metadataSize // metadata length + metadata data
        );
        expectedWriter.Write(recordSize);
        expectedWriter.Write((ushort)1); // id
        expectedWriter.Write((ushort)2); // schemaId
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(TOPIC)); // topic length
        expectedWriter.Write(Encoding.UTF8.GetBytes(TOPIC)); // topic data
        expectedWriter.Write((uint)Encoding.UTF8.GetByteCount(MESSAGE_ENCODING)); // messageEncoding length
        expectedWriter.Write(Encoding.UTF8.GetBytes(MESSAGE_ENCODING)); // messageEncoding data
        expectedWriter.Write((uint)metadataSize); // metadata length
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
        McapWriter.Write(writable, channel);
        var actualBytes = stream.ToArray();

        // Assert
        Assert.Equal(expectedBytes, actualBytes);
    }
}