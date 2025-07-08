namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::SummaryOffset` struct.
/// </summary>
public class SummaryOffset
{
    public OpCode GroupOpCode { get; set; }
    public ulong GroupStart { get; set; }
    public ulong GroupLength { get; set; }
}
