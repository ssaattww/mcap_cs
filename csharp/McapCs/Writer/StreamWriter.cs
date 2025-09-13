using System.IO;

namespace McapCs.Writer;

public class StreamWriter : Writable, IDisposable
{
  private readonly Stream? _stream;
  private ulong _size = 0;

  public StreamWriter(Stream stream)
  {
    _stream = stream;
    _size = 0;
  }

  protected override void HandleWrite(byte[] data)
  {
    if (data == null || data.Length == 0)
    {
      return;
    }

    var size = (ulong)data.Length;
    if (_stream == null)
    {
      throw new InvalidOperationException("StreamWriter is not initialized with a stream.");
    }

    _stream.Write(data, 0, (int)size);
    _size += size;


  }
  public void Dispose()
  {
    End();
  }
  public override void Flush()
  {
    if (_stream == null)
    {
      throw new InvalidOperationException("StreamWriter is not initialized with a stream.");
    }
    _stream.Flush();
  }

  public override void End()
  {
    if (_stream == null)
    {
      throw new InvalidOperationException("StreamWriter is not initialized with a stream.");
    }
    Flush();
    _stream?.Close();
    _size = 0;
    _stream?.Dispose();
  }

  /// <inheritdoc/>
  public override ulong Size
  {
    get { return _size; }
  }
}
