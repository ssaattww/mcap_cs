using System;
using System.Collections.Generic;
using System.IO;
using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Streams;

namespace McapCs.Writer;

public class Lz4ChunkWriter : ChunkWriter
{
  private readonly List<byte> _buffer = new();
  private byte[] _compressed = Array.Empty<byte>();
  private bool _finalized = false;
  private readonly LZ4Level _level;

  public Lz4ChunkWriter(LZ4Level level)
  {
    _level = level;
  }

  public override void End()
  {
    if (_finalized) return;
    // Compress using LZ4 frame format
    using var ms = new MemoryStream();
    using (var lz4 = LZ4Stream.Encode(ms, new LZ4EncoderSettings() { CompressionLevel = _level }, leaveOpen: true))
    {
      var data = _buffer.ToArray();
      lz4.Write(data, 0, data.Length);
    }
    _compressed = ms.ToArray();
    _finalized = true;
  }

  public override ulong Size => (ulong)_buffer.Count;

  public override void Flush()
  {
    // nothing
  }

  protected override void HandleWrite(byte[] data)
  {
    if (data is { Length: > 0 }) _buffer.AddRange(data);
  }

  public override bool Empty => _buffer.Count == 0;

  public override ulong CompressedSize
  {
    get
    {
      if (!_finalized) End();
      return (ulong)_compressed.LongLength;
    }
  }

  public override byte[] Data => _buffer.ToArray();

  public override byte[] CompressedData
  {
    get
    {
      if (!_finalized) End();
      return _compressed;
    }
  }

  protected override void HandleClear()
  {
    _buffer.Clear();
    _compressed = Array.Empty<byte>();
    _finalized = false;
  }
}

