namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Statistics` struct.
/// </summary>
public class Statistics
{
    public ulong MessageCount { get; set; }
    public ushort SchemaCount { get; set; }
    public uint ChannelCount { get; set; }
    public uint AttachmentCount { get; set; }
    public uint MetadataCount { get; set; }
    public uint ChunkCount { get; set; }
    public ulong MessageStartTime { get; set; }
    public ulong MessageEndTime { get; set; }
    public IReadOnlyDictionary<ushort, ulong> ChannelMessageCounts { get; set; } = new Dictionary<ushort, ulong>();
}
