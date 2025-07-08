using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Footer` struct (defined in `cpp/mcap/include/mcap/types.hpp`).
/// </summary>
public class Footer : IRecordSerializable
{
    public ulong SummaryStart { get; set; }
    public ulong SummaryOffset { get; set; }
    public uint SummaryCrc { get; set; }
    public string Library { get; set; } = "";

    public void Write(BinaryWriter writer)
    {
        writer.Write(SummaryStart);
        writer.Write(SummaryOffset);
        writer.Write(SummaryCrc);
    }
}
