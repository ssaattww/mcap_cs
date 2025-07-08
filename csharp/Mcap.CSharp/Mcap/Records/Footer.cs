using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Footer` struct (defined in `cpp/mcap/include/mcap/types.hpp`).
/// </summary>
public class Footer : IWritable
{
    public ulong SummaryStart { get; set; }
    public ulong SummaryOffset { get; set; }
    public uint SummaryCrc { get; set; }
    public string Library { get; set; } = "";

    // IWritable implementation
    public bool CrcEnabled { get; set; }

    public void Write(BinaryWriter writer)
    {
        writer.Write(SummaryStart);
        writer.Write(SummaryOffset);
        writer.Write(SummaryCrc);
    }

    public void Write(byte[] data, ulong size)
    {
        throw new NotImplementedException("This method is not used for serialization. Use Write(BinaryWriter writer) instead.");
    }

    public void End()
    {
        throw new NotImplementedException();
    }

    public ulong Size()
    {
        return 0;
    }

    public uint Crc()
    {
        throw new NotImplementedException();
    }

    public void ResetCrc()
    {
        throw new NotImplementedException();
    }

    public void Flush()
    {
        throw new NotImplementedException();
    }
}
