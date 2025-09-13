namespace McapCs.Writer;
using System.Collections.Generic;
public class BufferWriter : ChunkWriter
{
  public BufferWriter()
  {
  }

  public override void End()
  {
    // No specific end logic for buffer writer
  }

  /// <inheritdoc/>
  public override ulong Size
  {
    get
    {
      return (ulong)buffer.Count;
    }
  }

  public override void Flush()
  {
    // No specific flush logic for buffer writer
  }

  protected override void HandleWrite(byte[] data)
  {
    buffer.AddRange(data);
  }

  /// <inheritdoc/>
  public override bool Empty
  {
    get { return buffer.Count == 0; }
  }

  /// <inheritdoc/>
  public override ulong CompressedSize => Size;
  /// <inheritdoc/>
  public override byte[] Data => buffer.ToArray();
  /// <inheritdoc/>
  public override byte[] CompressedData => buffer.ToArray();

  protected override void HandleClear()
  {
    buffer.Clear();
  }

  public byte[] GetBuffer()
  {
    return buffer.ToArray();
  }

    private List<byte> buffer  = new List<byte>();
}
