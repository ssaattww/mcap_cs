namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Footer` struct.
/// </summary>
public class Footer
{
    public ulong SummaryStart { get; set; }
    public ulong SummaryOffsetStart { get; set; }
    public uint SummaryCrc { get; set; }
}
