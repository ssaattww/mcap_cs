using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using McapCs.Record;
using McapCs.Types;
using McapCs.Writer;
using Xunit;

namespace McapCs.Test.Writer;

public class CompressionWriterTests
{
  private static (string compression, ulong uncompressed, ulong compressed) ParseFirstChunk(string path)
  {
    using var fs = File.OpenRead(path);
    using var br = new BinaryReader(fs, Encoding.UTF8, leaveOpen: false);

    // Read leading magic (8 bytes)
    var magic = br.ReadBytes(Constants.Magic.Length);
    Assert.Equal(Constants.Magic, magic);

    while (br.BaseStream.Position < br.BaseStream.Length)
    {
      int op = br.Read();
      if (op == -1) break;
      var opcode = (EOpCode)(byte)op;
      ulong dataSize = br.ReadUInt64();
      if (opcode == EOpCode.Chunk)
      {
        // Chunk layout: u64 msgStart, u64 msgEnd, u64 uncompressedSize, u32 uncompressedCrc,
        // string compression, u64 compressedSize, bytes[compressedSize]
        _ = br.ReadUInt64(); // msgStart
        _ = br.ReadUInt64(); // msgEnd
        ulong uncompressed = br.ReadUInt64();
        _ = br.ReadUInt32(); // uncompressedCrc
        string compression = ReadString(br);
        ulong compressed = br.ReadUInt64();
        // Skip payload
        br.BaseStream.Seek((long)compressed, SeekOrigin.Current);
        return (compression, uncompressed, compressed);
      }
      // skip payload of other records
      br.BaseStream.Seek((long)dataSize, SeekOrigin.Current);
    }
    throw new InvalidDataException("Chunk not found in MCAP file");
  }

  private static string ReadString(BinaryReader br)
  {
    uint len = br.ReadUInt32();
    if (len == 0) return string.Empty;
    var bytes = br.ReadBytes((int)len);
    return Encoding.UTF8.GetString(bytes);
  }

  private static string WriteSimpleFile(string compressionName, bool forceCompression)
  {
    string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
      $"mcap_test_{Guid.NewGuid():N}.mcap");

    var options = new McapWriterOptions(profile: "default", library: $"libmcap {Constants.MCAP_LIBRARY_VERSION}")
    {
      // Enable chunking with default size
      noChunking = false,
      compression = compressionName switch
      {
        "none" => Compression.None,
        "lz4" => Compression.Lz4,
        "zstd" => Compression.Zstd,
        _ => Compression.None
      },
      // Make sure we compress even for small chunks
      forceCompression = forceCompression,
      // Keep CRCs enabled by default for Chunk
      noChunkCRC = false,
      // Keep other defaults
    };

    // Build a very compressible payload
    var payloadBytes = Encoding.UTF8.GetBytes(new string('A', 16_384));

    using (var writer = new McapWriter())
    {
      var st = writer.Open(path, options);
      Assert.True(st.Ok, st.Message);

      // Register schema and channel
      var schema = new Schema { name = "foo", encoding = "text/plain", data = new List<byte>() };
      writer.AddSchema(schema);
      var channel = new Channel { schemaId = schema.id, topic = "/test", messageEncoding = "text/plain", metadata = new Dictionary<string, string>() };
      writer.AddChannel(channel);

      // Write one message on channel 1
      var msg = new Message
      {
        channelId = 1,
        sequence = 1,
        logTime = 1,
        publishTime = 1,
        data = new List<byte>(payloadBytes),
        dataSize = (ulong)payloadBytes.Length,
      };
      var stMsg = writer.Write(msg);
      Assert.True(stMsg.Ok, stMsg.Message);

      writer.Close();
    }

    return path;
  }

  [Fact]
  public void WriteChunk_NoCompression_WritesNone()
  {
    string path = WriteSimpleFile("none", forceCompression: false);
    try
    {
      var (compression, uncompressed, compressed) = ParseFirstChunk(path);
      Assert.Equal("none", compression);
      Assert.Equal(uncompressed, compressed);
    }
    finally
    {
      if (File.Exists(path)) File.Delete(path);
    }
  }

  [Fact]
  public void WriteChunk_Lz4_CompressesAndLabels()
  {
    // Expect this to fail before LZ4 is implemented
    string path = WriteSimpleFile("lz4", forceCompression: true);
    try
    {
      var (compression, uncompressed, compressed) = ParseFirstChunk(path);
      Assert.Equal("lz4", compression);
      Assert.True(compressed < uncompressed, $"expected compressed < uncompressed but got {compressed} >= {uncompressed}");
    }
    finally
    {
      if (File.Exists(path)) File.Delete(path);
    }
  }

  [Fact]
  public void WriteChunk_Zstd_CompressesAndLabels()
  {
    // Expect this to fail before Zstd is implemented
    string path = WriteSimpleFile("zstd", forceCompression: true);
    try
    {
      var (compression, uncompressed, compressed) = ParseFirstChunk(path);
      Assert.Equal("zstd", compression);
      Assert.True(compressed < uncompressed, $"expected compressed < uncompressed but got {compressed} >= {uncompressed}");
    }
    finally
    {
      if (File.Exists(path)) File.Delete(path);
    }
  }
}

