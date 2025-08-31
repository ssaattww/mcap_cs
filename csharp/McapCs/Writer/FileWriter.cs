using System;
using System.IO;
using McapCs.Types;
namespace McapCs.Writer;

public class FileWriter : Writable, IDisposable
{
  private BinaryWriter? _writer;
  private ulong _size = 0;

  public FileWriter()
  {
    // コンストラクタは空
  }

  public void Dispose()
  {
    End();
  }

  public override void End()
  {
    if (_writer != null)
    {
      _writer.Close();
      _writer = null;
    }
    _size = 0;
  }

  public override ulong Size
  {
    get
    {
      return _size;
    }
  }

  public override void Flush()
  {
    _writer?.Flush();
  }

  protected override void HandleWrite(byte[] data)
  {
    if (_writer == null)
    {
      throw new InvalidOperationException("FileWriter is not open.");
    }
    var size = (ulong)data.Length;
#if DEBUG
    Console.WriteLine($"Writing {size} bytes to file.");
#endif
    _writer.Write(data, 0, (int)size);
    _size += size;
  }

  public Status Open(string filePath)
  {
    try
    {
      End(); // 既に開いている場合は閉じる
      _writer = new BinaryWriter(File.Open(filePath, FileMode.Create, FileAccess.Write));
      _size = 0;
      return new Status(StatusCode.Success);
    }
    catch (Exception ex)
    {
      return new Status(StatusCode.OpenFailed, ex.Message);
    }
  }
}
