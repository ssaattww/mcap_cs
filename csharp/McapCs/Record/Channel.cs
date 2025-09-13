namespace McapCs.Record;
/// <summary>
/// Describes a Channel that messages are written to.
/// メッセージの書き込み先となるチャネルを表します。
/// </summary>
public struct Channel {
  public ushort id;
  public string topic = string.Empty;
  public string messageEncoding = string.Empty;
  public ushort schemaId;
  public Dictionary<string, string> metadata = new Dictionary<string, string>();

  public Channel()
  {

  }

  public Channel(string topic, string messageEncoding, ushort schemaId, Dictionary<string, string>? metadata = null)
  {
    // Initialize with provided topic, message encoding, schema ID, and metadata
    this.topic = topic;
    this.messageEncoding = messageEncoding;
    this.schemaId = schemaId;
    this.metadata = metadata ?? new Dictionary<string, string>();
  }
};
