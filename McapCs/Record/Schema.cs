namespace McapCs.Record;
/// <summary>
/// Schema used for encoding/decoding or describing message shape.
/// メッセージのエンコード/デコードや形状記述のためのスキーマ。
/// </summary>
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
