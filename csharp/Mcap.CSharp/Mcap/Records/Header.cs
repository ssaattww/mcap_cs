using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records
{
    /// <summary>
    /// Corresponds to the C++ `mcap::Header` struct (defined in `cpp/mcap/include/mcap/types.hpp`).
    /// </summary>
    public class Header : IWritable
    {
    public string Profile { get; set; } = "";
    public string Library { get; set; } = "";

    // IWritable implementation
    public bool CrcEnabled { get; set; }

    public void Write(BinaryWriter writer)
    {
        writer.Write((uint)Profile.Length);
        writer.Write(System.Text.Encoding.UTF8.GetBytes(Profile));
        writer.Write((uint)Library.Length);
        writer.Write(System.Text.Encoding.UTF8.GetBytes(Library));
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
