using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records;

/// <summary>
/// Corresponds to the C++ `mcap::Channel` struct (defined in `cpp/mcap/include/mcap/types.hpp`).
/// </summary>
public class Channel : IRecordSerializable
{
    public ushort Id { get; set; }
    public string Topic { get; set; } = "";
    public string MessageEncoding { get; set; } = "";
    public ushort SchemaId { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

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
}
