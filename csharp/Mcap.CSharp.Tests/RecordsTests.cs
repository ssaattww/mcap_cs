using Mcap.CSharp.Mcap.Records;
using Xunit;
using Mcap.CSharp.Mcap;

namespace Mcap.CSharp.Tests;

public class RecordsTests
{
    [Fact]
    public void CanCreateHeader()
    {
        var header = new Header();
        Assert.NotNull(header);
    }

    [Fact]
    public void Header_CanSetProperties()
    {
        var header = new Header
        {
            Profile = "test_profile",
            Library = "test_library"
        };
        Assert.Equal("test_profile", header.Profile);
        Assert.Equal("test_library", header.Library);
    }

    [Fact]
    public void CanCreateFooter()
    {
        var footer = new Footer();
        Assert.NotNull(footer);
    }

    [Fact]
    public void Footer_CanSetProperties()
    {
        var footer = new Footer
        {
            SummaryStart = 100UL,
            SummaryOffsetStart = 200UL,
            SummaryCrc = 0x12345678U
        };
        Assert.Equal(100UL, footer.SummaryStart);
        Assert.Equal(200UL, footer.SummaryOffsetStart);
        Assert.Equal(0x12345678U, footer.SummaryCrc);
    }

    [Fact]
    public void CanCreateSchema()
    {
        var schema = new Schema();
        Assert.NotNull(schema);
    }

    [Fact]
    public void Schema_CanSetProperties()
    {
        var schema = new Schema
        {
            Id = 1,
            Name = "test_schema",
            Encoding = "json",
            Data = new byte[] { 0x01, 0x02, 0x03 }
        };
        Assert.Equal(1, schema.Id);
        Assert.Equal("test_schema", schema.Name);
        Assert.Equal("json", schema.Encoding);
        Assert.Equal(new byte[] { 0x01, 0x02, 0x03 }, schema.Data);
    }

    [Fact]
    public void CanCreateChannel()
    {
        var channel = new Channel();
        Assert.NotNull(channel);
    }

    [Fact]
    public void Channel_CanSetProperties()
    {
        var channel = new Channel
        {
            Id = 10,
            Topic = "/test_topic",
            MessageEncoding = "ros1",
            SchemaId = 1,
            Metadata = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" }
            }
        };
        Assert.Equal(10, channel.Id);
        Assert.Equal("/test_topic", channel.Topic);
        Assert.Equal("ros1", channel.MessageEncoding);
        Assert.Equal(1, channel.SchemaId);
        Assert.Equal("value1", channel.Metadata["key1"]);
        Assert.Equal("value2", channel.Metadata["key2"]);
    }

    [Fact]
    public void CanCreateMessage()
    {
        var message = new Message();
        Assert.NotNull(message);
    }

    [Fact]
    public void Message_CanSetProperties()
    {
        var message = new Message
        {
            ChannelId = 100,
            Sequence = 1,
            LogTime = 1234567890123456789UL,
            PublishTime = 9876543210987654321UL,
            Data = new byte[] { 0x04, 0x05, 0x06 }
        };
        Assert.Equal(100, message.ChannelId);
        Assert.Equal(1U, message.Sequence);
        Assert.Equal(1234567890123456789UL, message.LogTime);
        Assert.Equal(9876543210987654321UL, message.PublishTime);
        Assert.Equal(new byte[] { 0x04, 0x05, 0x06 }, message.Data);
    }

    [Fact]
    public void CanCreateChunk()
    {
        var chunk = new Chunk();
        Assert.NotNull(chunk);
    }

    [Fact]
    public void Chunk_CanSetProperties()
    {
        var chunk = new Chunk
        {
            MessageStartTime = 1000UL,
            MessageEndTime = 2000UL,
            UncompressedSize = 500UL,
            UncompressedCrc = 0xABCDEF01U,
            Compression = "lz4",
            CompressedSize = 300UL,
            Records = new byte[] { 0x07, 0x08, 0x09 }
        };
        Assert.Equal(1000UL, chunk.MessageStartTime);
        Assert.Equal(2000UL, chunk.MessageEndTime);
        Assert.Equal(500UL, chunk.UncompressedSize);
        Assert.Equal(0xABCDEF01U, chunk.UncompressedCrc);
        Assert.Equal("lz4", chunk.Compression);
        Assert.Equal(300UL, chunk.CompressedSize);
        Assert.Equal(new byte[] { 0x07, 0x08, 0x09 }, chunk.Records);
    }

    [Fact]
    public void CanCreateMessageIndex()
    {
        var messageIndex = new MessageIndex();
        Assert.NotNull(messageIndex);
    }

    [Fact]
    public void MessageIndex_CanSetProperties()
    {
        var messageIndex = new MessageIndex
        {
            ChannelId = 1,
            Records = new List<Tuple<ulong, ulong>>
            {
                Tuple.Create(100UL, 10UL),
                Tuple.Create(200UL, 20UL)
            }
        };
        Assert.Equal(1, messageIndex.ChannelId);
        Assert.Equal(2, messageIndex.Records.Count);
        Assert.Equal(100UL, messageIndex.Records[0].Item1);
        Assert.Equal(10UL, messageIndex.Records[0].Item2);
        Assert.Equal(200UL, messageIndex.Records[1].Item1);
        Assert.Equal(20UL, messageIndex.Records[1].Item2);
    }

    [Fact]
    public void CanCreateChunkIndex()
    {
        var chunkIndex = new ChunkIndex();
        Assert.NotNull(chunkIndex);
    }

    [Fact]
    public void ChunkIndex_CanSetProperties()
    {
        var chunkIndex = new ChunkIndex
        {
            MessageStartTime = 1000UL,
            MessageEndTime = 2000UL,
            ChunkStartOffset = 100UL,
            ChunkLength = 500UL,
            MessageIndexOffsets = new Dictionary<ushort, ulong>
            {
                { 1, 10UL },
                { 2, 20UL }
            },
            MessageIndexLength = 30UL,
            Compression = "zstd",
            CompressedSize = 400UL,
            UncompressedSize = 600UL
        };
        Assert.Equal(1000UL, chunkIndex.MessageStartTime);
        Assert.Equal(2000UL, chunkIndex.MessageEndTime);
        Assert.Equal(100UL, chunkIndex.ChunkStartOffset);
        Assert.Equal(500UL, chunkIndex.ChunkLength);
        Assert.Equal(10UL, chunkIndex.MessageIndexOffsets[1]);
        Assert.Equal(20UL, chunkIndex.MessageIndexOffsets[2]);
        Assert.Equal(30UL, chunkIndex.MessageIndexLength);
        Assert.Equal("zstd", chunkIndex.Compression);
        Assert.Equal(400UL, chunkIndex.CompressedSize);
        Assert.Equal(600UL, chunkIndex.UncompressedSize);
    }

    [Fact]
    public void CanCreateAttachment()
    {
        var attachment = new Attachment();
        Assert.NotNull(attachment);
    }

    [Fact]
    public void Attachment_CanSetProperties()
    {
        var attachment = new Attachment
        {
            LogTime = 1000UL,
            CreateTime = 2000UL,
            Name = "test_attachment.txt",
            MediaType = "text/plain",
            Data = new byte[] { 0x01, 0x02, 0x03 },
            Crc = 0xABCDEF01U
        };
        Assert.Equal(1000UL, attachment.LogTime);
        Assert.Equal(2000UL, attachment.CreateTime);
        Assert.Equal("test_attachment.txt", attachment.Name);
        Assert.Equal("text/plain", attachment.MediaType);
        Assert.Equal(new byte[] { 0x01, 0x02, 0x03 }, attachment.Data);
        Assert.Equal(0xABCDEF01U, attachment.Crc);
    }

    [Fact]
    public void CanCreateAttachmentIndex()
    {
        var attachmentIndex = new AttachmentIndex();
        Assert.NotNull(attachmentIndex);
    }

    [Fact]
    public void AttachmentIndex_CanSetProperties()
    {
        var attachmentIndex = new AttachmentIndex
        {
            Offset = 100UL,
            Length = 200UL,
            LogTime = 1000UL,
            CreateTime = 2000UL,
            DataSize = 500UL,
            Name = "test_attachment_index.txt",
            MediaType = "text/plain"
        };
        Assert.Equal(100UL, attachmentIndex.Offset);
        Assert.Equal(200UL, attachmentIndex.Length);
        Assert.Equal(1000UL, attachmentIndex.LogTime);
        Assert.Equal(2000UL, attachmentIndex.CreateTime);
        Assert.Equal(500UL, attachmentIndex.DataSize);
        Assert.Equal("test_attachment_index.txt", attachmentIndex.Name);
        Assert.Equal("text/plain", attachmentIndex.MediaType);
    }

    [Fact]
    public void CanCreateStatistics()
    {
        var statistics = new Statistics();
        Assert.NotNull(statistics);
    }

    [Fact]
    public void Statistics_CanSetProperties()
    {
        var statistics = new Statistics
        {
            MessageCount = 100UL,
            SchemaCount = 10,
            ChannelCount = 20,
            AttachmentCount = 5,
            MetadataCount = 3,
            ChunkCount = 2,
            MessageStartTime = 1000UL,
            MessageEndTime = 2000UL,
            ChannelMessageCounts = new Dictionary<ushort, ulong>
            {
                { 1, 50UL },
                { 2, 50UL }
            }
        };
        Assert.Equal(100UL, statistics.MessageCount);
        Assert.Equal(10, statistics.SchemaCount);
        Assert.Equal(20U, statistics.ChannelCount);
        Assert.Equal(5U, statistics.AttachmentCount);
        Assert.Equal(3U, statistics.MetadataCount);
        Assert.Equal(2U, statistics.ChunkCount);
        Assert.Equal(1000UL, statistics.MessageStartTime);
        Assert.Equal(2000UL, statistics.MessageEndTime);
        Assert.Equal(50UL, statistics.ChannelMessageCounts[1]);
        Assert.Equal(50UL, statistics.ChannelMessageCounts[2]);
    }

    [Fact]
    public void CanCreateMetadata()
    {
        var metadata = new Metadata();
        Assert.NotNull(metadata);
    }

    [Fact]
    public void Metadata_CanSetProperties()
    {
        var metadata = new Metadata
        {
            Name = "test_metadata",
            MetadataMap = new Dictionary<string, string>
            {
                { "keyA", "valueA" },
                { "keyB", "valueB" }
            }
        };
        Assert.Equal("test_metadata", metadata.Name);
        Assert.Equal("valueA", metadata.MetadataMap["keyA"]);
        Assert.Equal("valueB", metadata.MetadataMap["keyB"]);
    }

    [Fact]
    public void CanCreateMetadataIndex()
    {
        var metadataIndex = new MetadataIndex();
        Assert.NotNull(metadataIndex);
    }

    [Fact]
    public void MetadataIndex_CanSetProperties()
    {
        var metadataIndex = new MetadataIndex
        {
            Offset = 100UL,
            Length = 200UL,
            Name = "test_metadata_index"
        };
        Assert.Equal(100UL, metadataIndex.Offset);
        Assert.Equal(200UL, metadataIndex.Length);
        Assert.Equal("test_metadata_index", metadataIndex.Name);
    }

    [Fact]
    public void CanCreateSummaryOffset()
    {
        var summaryOffset = new SummaryOffset();
        Assert.NotNull(summaryOffset);
    }

    [Fact]
    public void SummaryOffset_CanSetProperties()
    {
        var summaryOffset = new SummaryOffset
        {
            GroupOpCode = OpCode.Schema,
            GroupStart = 100UL,
            GroupLength = 200UL
        };
        Assert.Equal(OpCode.Schema, summaryOffset.GroupOpCode);
        Assert.Equal(100UL, summaryOffset.GroupStart);
        Assert.Equal(200UL, summaryOffset.GroupLength);
    }

    [Fact]
    public void CanCreateDataEnd()
    {
        var dataEnd = new DataEnd();
        Assert.NotNull(dataEnd);
    }

    [Fact]
    public void DataEnd_CanSetProperties()
    {
        var dataEnd = new DataEnd
        {
            DataSectionCrc = 0xABCDEF01U
        };
        Assert.Equal(0xABCDEF01U, dataEnd.DataSectionCrc);
    }
}
