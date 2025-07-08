namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Metadata` struct.
/// </summary>
public class Metadata
{
    public string Name { get; set; } = "";
    public IReadOnlyDictionary<string, string> MetadataMap { get; set; } = new Dictionary<string, string>();
}
