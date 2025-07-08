using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records
{
    public class Header : IWritable
    {
    public string Profile { get; set; } = "";
    public string Library { get; set; } = "";

    // IWritable implementation
    public bool CrcEnabled { get; set; }

    public void Write(byte[] data, ulong size)
    {
        throw new NotImplementedException();
    }

    public void End()
    {
        throw new NotImplementedException();
    }

    public ulong Size()
    {
        // TBD: Calculate actual size of Header
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
}
