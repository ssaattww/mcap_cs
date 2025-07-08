using System.IO;

namespace Mcap.CSharp.Mcap.Interfaces;

/// <summary>
/// Corresponds to the C++ `mcap::FileWriter` class.
/// </summary>
public class FileWriter : IWritable
{
    private FileStream? _fileStream;
    private ulong _size = 0;

    public bool CrcEnabled { get; set; }

    public void Open(string filePath)
    {
        _fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
    }

    public void Write(byte[] data, ulong size)
    {
        if (_fileStream == null)
        {
            throw new InvalidOperationException("FileWriter is not open.");
        }
        _fileStream.Write(data, 0, (int)size);
        _size += size;
    }

    public void End()
    {
        _fileStream?.Dispose();
        _fileStream = null;
    }

    public ulong Size()
    {
        return _size;
    }

    public uint Crc()
    {
        // TBD: Implement CRC calculation
        return 0;
    }

    public void ResetCrc()
    {
        // TBD: Implement CRC reset
    }

    public void Flush()
    {
        _fileStream?.Flush();
    }
}
