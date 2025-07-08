namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::MessageIndex` struct.
/// </summary>
public class MessageIndex
{
    public ushort ChannelId { get; set; }
    public List<Tuple<ulong, ulong>> Records { get; set; } = new List<Tuple<ulong, ulong>>();
}
