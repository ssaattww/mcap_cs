namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Schema` struct.
/// </summary>
public class Schema
{
    public ushort Id { get; set; }
    public string Name { get; set; } = "";
    public string Encoding { get; set; } = "";
    public byte[] Data { get; set; } = Array.Empty<byte>();
}
