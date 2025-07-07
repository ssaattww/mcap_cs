namespace Mcap.CSharp.Mcap;

public readonly struct Status
{
    // TBD
}

/// <summary>
/// Corresponds to the C++ `mcap::McapWriterOptions` struct.
/// </summary>
public class McapWriterOptions
{
    public string Profile { get; set; } = "";
    public ulong ChunkSize { get; set; } = 8 * 1024 * 1024; // Default to 8MB
    public Compression Compression { get; set; } = Compression.Zstd;
    public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Default;
    public bool NoChunking { get; set; } = false;
    public bool ForceCompression { get; set; } = false;
}

public class ReadMessageOptions
{
    // TBD
}

public class MessageView
{
    // TBD
}
