// Corresponds to writer options in C++ mcap::McapWriter, specifically related to chunking behavior.
// See cpp/mcap/include/mcap/writer.hpp for C++ writer implementation details.
namespace Mcap.CSharp.Mcap
{
    public class McapWriterOptions
    {
        public string Profile { get; set; } = "";
        public ulong ChunkSize { get; set; } = 8 * 1024 * 1024; // Default to 8MB
        public Compression Compression { get; set; } = Compression.Zstd;
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Default;
        public bool NoChunking { get; set; } = false;
        public bool ForceCompression { get; set; } = false;
    }
}
