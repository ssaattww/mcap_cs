namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Attachment` struct.
/// </summary>
public class Attachment
{
    public ulong LogTime { get; set; }
    public ulong CreateTime { get; set; }
    public string Name { get; set; } = "";
    public string MediaType { get; set; } = "";
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public uint Crc { get; set; }
}
