namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Message` struct.
/// </summary>
public class Message
{
    public ushort ChannelId { get; set; }
    public uint Sequence { get; set; }
    public ulong LogTime { get; set; }
    public ulong PublishTime { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}
