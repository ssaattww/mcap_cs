namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Chunk` struct.
/// </summary>
public class Chunk
{
    public ulong MessageStartTime { get; set; }
    public ulong MessageEndTime { get; set; }
    public ulong UncompressedSize { get; set; }
    public uint UncompressedCrc { get; set; }
    public string Compression { get; set; } = "";
    public ulong CompressedSize { get; set; }
    public byte[] Records { get; set; } = Array.Empty<byte>();
}
