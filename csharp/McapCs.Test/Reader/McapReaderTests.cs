using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using McapCs;
using McapCs.Reader;
using McapCs.Record;
using McapCs.Writer;

namespace McapCs.Test.Reader;

public class McapReaderTests
{
    [Fact]
    public void Reads_Unchunked_File()
    {
        // 手動で「非チャンク」レコード列を構築
        using var ms = new MemoryStream();
        var payload = Encoding.UTF8.GetBytes("{\"hello\":\"world\"}");
        using (var bw = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true))
        {
            // Magic + Header
            bw.Write(Constants.Magic);
            bw.Write((byte)EOpCode.Header);
            var profile = "test_profile";
            var library = "mcap_cs_test";
            ulong headerSize = 4UL + (ulong)Encoding.UTF8.GetByteCount(profile) + 4UL + (ulong)Encoding.UTF8.GetByteCount(library);
            bw.Write(headerSize);
            WriteString(bw, profile);
            WriteString(bw, library);

            // Schema
            bw.Write((byte)EOpCode.Schema);
            var schemaName = "Example";
            var schemaEncoding = "json";
            var schemaData = Encoding.UTF8.GetBytes("{}");
            ulong schemaSize = 2UL + 4UL + (ulong)Encoding.UTF8.GetByteCount(schemaName) + 4UL + (ulong)Encoding.UTF8.GetByteCount(schemaEncoding) + 4UL + (ulong)schemaData.Length;
            bw.Write(schemaSize);
            bw.Write((ushort)1);
            WriteString(bw, schemaName);
            WriteString(bw, schemaEncoding);
            bw.Write((uint)schemaData.Length);
            bw.Write(schemaData);

            // Channel
            bw.Write((byte)EOpCode.Channel);
            var topic = "/example";
            var encoding = "json";
            ulong mdSize = 0UL;
            ulong channelSize = 2UL + 4UL + (ulong)Encoding.UTF8.GetByteCount(topic) + 4UL + (ulong)Encoding.UTF8.GetByteCount(encoding) + 2UL + 4UL + mdSize;
            bw.Write(channelSize);
            bw.Write((ushort)1); // id
            bw.Write((ushort)1); // schemaId
            WriteString(bw, topic);
            WriteString(bw, encoding);
            bw.Write((uint)mdSize);

            // Message
            bw.Write((byte)EOpCode.Message);
            ulong msgSize = 2 + 4 + 8 + 8 + 4 + (ulong)payload.Length;
            bw.Write(msgSize);
            bw.Write((ushort)1);
            bw.Write((uint)42);
            bw.Write((ulong)100);
            bw.Write((ulong)100);
            bw.Write((uint)payload.Length);
            bw.Write(payload);

            // Footer（最小）
            bw.Write((byte)EOpCode.Footer);
            ulong footerSize = 8 + 8 + 4;
            bw.Write(footerSize);
            bw.Write((ulong)0);
            bw.Write((ulong)0);
            bw.Write((uint)0);
        }

        ms.Position = 0;
        var reader = new McapReader();
        reader.Open(ms);

        Assert.NotNull(reader.Header);
        Assert.Equal("test_profile", reader.Header?.profile);
        Assert.Equal("mcap_cs_test", reader.Header?.library);
        Assert.Single(reader.Schemas);
        Assert.Single(reader.Channels);
        var msgs = reader.ReadMessages().ToList();
        Assert.Single(msgs);
        Assert.Equal((ushort)1, msgs[0].channelId);
        Assert.Equal((uint)42, msgs[0].sequence);
        Assert.Equal((ulong)100, msgs[0].logTime);
        Assert.Equal(payload.Length, msgs[0].data.Count);
        Assert.Equal(payload, msgs[0].data.ToArray());
    }

    [Fact]
    public void Reads_Uncompressed_Chunk_File()
    {
        var options = new McapWriterOptions("test_profile", "mcap_cs_test")
        {
            // チャンクは有効のまま、圧縮は無効（"none"）にする
            compression = McapCs.Types.Compression.None,
        };

        var writer = new McapWriter();
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".mcap");
        try
        {
            writer.Open(path, options);

            var channel = new Channel(topic: "/chunked", messageEncoding: "json", schemaId: 0, metadata: new Dictionary<string, string>());
            writer.AddChannel(channel);

            var payload = Encoding.UTF8.GetBytes("{\"a\":1}");
            var message = new Message
            {
                channelId = 1,
                sequence = 1,
                logTime = 10,
                publishTime = 10,
                data = payload.ToList(),
                dataSize = (ulong)payload.Length,
            };
            writer.Write(message);
            writer.Close(); // write chunk with compression="none"

            var reader = new McapReader();
            reader.Open(path);
            var msgs = reader.ReadMessages().ToList();
            Assert.Single(msgs);
            Assert.Equal((ushort)1, msgs[0].channelId);
            Assert.Equal(payload, msgs[0].data.ToArray());
        }
        finally
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Throws_On_Compressed_Chunk()
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

        // Magic
        bw.Write(Constants.Magic);

        // Header record with simple strings
        WriteRecordHeader(bw);

        // Chunk record with compression = "zstd" (no records body needed for this test)
        WriteCompressedChunkRecord(bw, "zstd", compressedSize: 0, uncompressedSize: 0, uncompressedCrc: 0);

        ms.Position = 0;
        var reader = new McapReader();
        Assert.Throws<NotSupportedException>(() => reader.Open(ms));
    }

    private static void WriteRecordHeader(BinaryWriter bw)
    {
        const string profile = "test";
        const string library = "lib";
        bw.Write((byte)EOpCode.Header);
        ulong recordSize = 4UL + (ulong)Encoding.UTF8.GetByteCount(profile)
                         + 4UL + (ulong)Encoding.UTF8.GetByteCount(library);
        bw.Write(recordSize);
        WriteString(bw, profile);
        WriteString(bw, library);
    }

    private static void WriteCompressedChunkRecord(BinaryWriter bw, string compression, ulong compressedSize, ulong uncompressedSize, uint uncompressedCrc)
    {
        bw.Write((byte)EOpCode.Chunk);
        ulong recordSize = 8 + 8 + 8 + 4
                         + 4 + (ulong)Encoding.UTF8.GetByteCount(compression)
                         + 8
                         + compressedSize; // records bytes
        bw.Write(recordSize);
        bw.Write((ulong)0); // messageStartTime
        bw.Write((ulong)0); // messageEndTime
        bw.Write(uncompressedSize);
        bw.Write(uncompressedCrc);
        WriteString(bw, compression);
        bw.Write(compressedSize);
        if (compressedSize > 0)
        {
            bw.Write(new byte[compressedSize]);
        }
    }

    private static void WriteString(BinaryWriter bw, string s)
    {
        var bytes = Encoding.UTF8.GetBytes(s);
        bw.Write((uint)bytes.Length);
        bw.Write(bytes);
    }
}
