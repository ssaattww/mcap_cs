namespace Mcap.CSharp.Mcap;

public enum OpCode : byte
{
    Header = 0x01,
    Footer = 0x02,
    Schema = 0x03,
    Channel = 0x04,
    Message = 0x05,
    Chunk = 0x06,
    MessageIndex = 0x07,
    ChunkIndex = 0x08,
    Attachment = 0x09,
    AttachmentIndex = 0x0A,
    Statistics = 0x0B,
    Metadata = 0x0C,
    MetadataIndex = 0x0D,
    SummaryOffset = 0x0E,
    DataEnd = 0x0F,
}

public enum Compression
{
    None,
    Lz4,
    Zstd,
}

public enum CompressionLevel
{
    Default,
    Fastest,
    Fast,
    Slow,
    Slowest,
}
