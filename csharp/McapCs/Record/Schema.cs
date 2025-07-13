/**
 * @brief Describes a schema used for message encoding and decoding and/or
 * describing the shape of messages. One or more Channel records map to a single
 * Schema.
 */
namespace McapCs.Record;
public struct Schema {
  public ushort id;
  public string name;
  public string encoding;
  public List<byte> data = new List<byte>();

  public Schema( string name, string encoding, string data)
  {
    // Initialize with provided name, encoding, and data
    this.name = name;
    this.encoding = encoding;
    this.data = new List<byte>(data.Length);
    foreach (var b in data)
    {
      this.data.Add((byte)b);
    }
  }

  public Schema(string name, string encoding, List<byte> data)
  {
    // Initialize with provided name, encoding, and data
    this.name = name.ToString();
    this.encoding = encoding.ToString();
    foreach (var b in data)
    {
      this.data.Add((byte)b);
    }
  }
}
