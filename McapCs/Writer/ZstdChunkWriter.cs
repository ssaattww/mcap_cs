using System;
using System.Collections.Generic;
using System.IO;
using ZstdNet;

namespace McapCs.Writer;

public class ZstdChunkWriter : ChunkWriter
{
  private readonly List<byte> _buffer = new();
  private byte[] _compressed = Array.Empty<byte>();
  private bool _finalized = false;
  private readonly int _level;

  public ZstdChunkWriter(int level)
  {
    _level = level;
  }

  public override void End()
  {
    if (_finalized) return;
    var data = _buffer.ToArray();
    using (var compressor = new Compressor(new CompressionOptions(_level)))
    {
      _compressed = compressor.Wrap(data);
    }
    _finalized = true;
  }

  public override ulong Size => (ulong)_buffer.Count;

  public override void Flush() { }

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
