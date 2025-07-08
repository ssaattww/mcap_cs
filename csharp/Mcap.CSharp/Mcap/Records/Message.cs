using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Message` struct (defined in `cpp/mcap/include/mcap/types.hpp`).
/// </summary>
public class Message : IRecordSerializable
{
    public ushort ChannelId { get; set; }
    public uint Sequence { get; set; }
    public ulong LogTime { get; set; }
    public ulong PublishTime { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();

    public void Write(BinaryWriter writer)
    {
        writer.Write(ChannelId);
        writer.Write(Sequence);
        writer.Write(LogTime);
        writer.Write(PublishTime);
        writer.Write((uint)Data.Length);
        writer.Write(Data);
    }
}
