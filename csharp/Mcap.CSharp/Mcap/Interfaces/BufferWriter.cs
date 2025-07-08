using System.IO;

namespace Mcap.CSharp.Mcap.Interfaces;

/// <summary>
/// Corresponds to the C++ `mcap::BufferWriter` class.
/// </summary>
public class BufferWriter : IStreamWriter
{
    private List<byte> _buffer = new List<byte>();
    private ulong _size = 0;
    private long _position = 0; // Current position in the buffer

    public bool CrcEnabled { get; set; }

    public void Write(byte[] data, ulong size)
    {
        // Ensure buffer is large enough
        if (_position + (long)size > _buffer.Count)
        {
            _buffer.Capacity = (int)(_position + (long)size);
            // Fill with zeros if extending beyond current size
            while (_buffer.Count < _position + (long)size)
            {
                _buffer.Add(0);
            }
        }

        // Write data at current position
        for (int i = 0; i < (int)size; i++)
        {
            _buffer[(int)_position + i] = data[i];
        }
        _position += (long)size;
        _size = (ulong)Math.Max((long)_size, _position);
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

    public void Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                _position = offset;
                break;
            case SeekOrigin.Current:
                _position += offset;
                break;
            case SeekOrigin.End:
                _position = (long)_size + offset;
                break;
        }
        // Ensure position is not negative
        if (_position < 0) _position = 0;
        // Ensure buffer is large enough for the new position if seeking beyond current size
        if (_position > _buffer.Count)
        {
            _buffer.Capacity = (int)_position;
            while (_buffer.Count < _position)
            {
                _buffer.Add(0);
            }
        }
    }

    public byte[] ToArray()
    {
        return _buffer.Take((int)_size).ToArray();
    }
}
