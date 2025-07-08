namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::AttachmentIndex` struct.
/// </summary>
public class AttachmentIndex
{
    public ulong Offset { get; set; }
    public ulong Length { get; set; }
    public ulong LogTime { get; set; }
    public ulong CreateTime { get; set; }
    public ulong DataSize { get; set; }
    public string Name { get; set; } = "";
    public string MediaType { get; set; } = "";
}
