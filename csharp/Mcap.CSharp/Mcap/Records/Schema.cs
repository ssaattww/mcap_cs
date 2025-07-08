using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Schema` struct (defined in `cpp/mcap/include/mcap/types.hpp`).
/// </summary>
public class Schema : IWritable, IRecordSerializable
{
    public ushort Id { get; set; }
    public string Name { get; set; } = "";
    public string Encoding { get; set; } = "";
    public byte[] Data { get; set; } = Array.Empty<byte>();

    // IWritable implementation
    public bool CrcEnabled { get; set; }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write((uint)Name.Length);
        writer.Write(System.Text.Encoding.UTF8.GetBytes(Name));
        writer.Write((uint)Encoding.Length);
        writer.Write(System.Text.Encoding.UTF8.GetBytes(Encoding));
        writer.Write((uint)Data.Length);
        writer.Write(Data);
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