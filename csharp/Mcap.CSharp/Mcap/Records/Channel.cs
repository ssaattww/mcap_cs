namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Channel` struct.
/// </summary>
public class Channel
{
    public ushort Id { get; set; }
    public string Topic { get; set; } = "";
    public string MessageEncoding { get; set; } = "";
    public ushort SchemaId { get; set; }
    public IReadOnlyDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
}
