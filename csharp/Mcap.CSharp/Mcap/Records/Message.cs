using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Message` struct (defined in `cpp/mcap/include/mcap/types.hpp`).
/// </summary>
public class Message : IWritable
{
    public ushort ChannelId { get; set; }
    public uint Sequence { get; set; }
    public ulong LogTime { get; set; }
    public ulong PublishTime { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();

    // IWritable implementation
    public bool CrcEnabled { get; set; }

    public void Write(BinaryWriter writer)
    {
        writer.Write(ChannelId);
        writer.Write(Sequence);
        writer.Write(LogTime);
        writer.Write(PublishTime);
        writer.Write((uint)Data.Length);
        writer.Write(Data);
    }

    public void Write(byte[] data, ulong size)
    {
        throw new NotImplementedException("This method is not used for serialization. Use Write(BinaryWriter writer) instead.");
    }

    public void End()
    {
        throw new NotImplementedException();
    }

    public ulong Size()
    {
        return 0;
    }

    public uint Crc()
    {
        throw new NotImplementedException();
    }

    public void ResetCrc()
    {
        throw new NotImplementedException();
    }

    public void Flush()
    {
        throw new NotImplementedException();
    }
}
