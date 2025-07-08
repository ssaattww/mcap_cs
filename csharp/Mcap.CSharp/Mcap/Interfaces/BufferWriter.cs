using System.IO;

namespace Mcap.CSharp.Mcap.Interfaces;

/// <summary>
/// Corresponds to the C++ `mcap::BufferWriter` class.
/// </summary>
public class BufferWriter : IWritable
{
    private List<byte> _buffer = new List<byte>();
    private ulong _size = 0;

    public bool CrcEnabled { get; set; }

    public void Write(byte[] data, ulong size)
    {
        _buffer.AddRange(data.Take((int)size));
        _size += size;
    }

    public void Write(BinaryWriter writer)
    {
        throw new NotImplementedException("BufferWriter does not directly write to a BinaryWriter. This method is part of IWritable for records to write themselves.");
    }

    public void End()
    {
        // No-op for in-memory buffer
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
        // No-op for in-memory buffer
    }

    public byte[] ToArray()
    {
        return _buffer.ToArray();
    }
}
