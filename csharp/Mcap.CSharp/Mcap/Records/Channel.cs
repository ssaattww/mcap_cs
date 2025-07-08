using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Channel` struct (defined in `cpp/mcap/include/mcap/types.hpp`).
/// </summary>
public class Channel : IWritable, IRecordSerializable
{
    public ushort Id { get; set; }
    public string Topic { get; set; } = "";
    public string MessageEncoding { get; set; } = "";
    public ushort SchemaId { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

    // IWritable implementation
    public bool CrcEnabled { get; set; }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write(SchemaId);
        writer.Write((uint)Topic.Length);
        writer.Write(System.Text.Encoding.UTF8.GetBytes(Topic));
        writer.Write((uint)MessageEncoding.Length);
        writer.Write(System.Text.Encoding.UTF8.GetBytes(MessageEncoding));
        writer.Write((uint)Metadata.Count);
        foreach (var entry in Metadata)
        {
            writer.Write((uint)entry.Key.Length);
            writer.Write(System.Text.Encoding.UTF8.GetBytes(entry.Key));
            writer.Write((uint)entry.Value.Length);
            writer.Write(System.Text.Encoding.UTF8.GetBytes(entry.Value));
        }
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
