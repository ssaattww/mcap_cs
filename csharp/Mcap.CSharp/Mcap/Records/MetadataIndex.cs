namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::MetadataIndex` struct.
/// </summary>
public class MetadataIndex
{
    public ulong Offset { get; set; }
    public ulong Length { get; set; }
    public string Name { get; set; } = "";
}
