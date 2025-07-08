namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::ChunkIndex` struct.
/// </summary>
public class ChunkIndex
{
    public ulong MessageStartTime { get; set; }
    public ulong MessageEndTime { get; set; }
    public ulong ChunkStartOffset { get; set; }
    public ulong ChunkLength { get; set; }
    public IReadOnlyDictionary<ushort, ulong> MessageIndexOffsets { get; set; } = new Dictionary<ushort, ulong>();
    public ulong MessageIndexLength { get; set; }
    public string Compression { get; set; } = "";
    public ulong CompressedSize { get; set; }
    public ulong UncompressedSize { get; set; }
}
