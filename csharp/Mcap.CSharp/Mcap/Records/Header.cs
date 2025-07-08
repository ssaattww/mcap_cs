namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Header` struct.
/// </summary>
public class Header
{
    public string Profile { get; set; } = "";
    public string Library { get; set; } = "";
}
