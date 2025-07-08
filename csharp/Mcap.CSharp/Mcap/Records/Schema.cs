using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Schema` struct (defined in `cpp/mcap/include/mcap/types.hpp`).
/// </summary>
public class Schema : IRecordSerializable
{
    public ushort Id { get; set; }
    public string Name { get; set; } = "";
    public string Encoding { get; set; } = "";
    public byte[] Data { get; set; } = Array.Empty<byte>();

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
}