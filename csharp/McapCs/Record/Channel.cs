/**
 * @brief Describes a Channel that messages are written to. A Channel represents
 * a single connection from a publisher to a topic, so each topic will have one
 * Channel per publisher. Channels optionally reference a Schema, for message
 * encodings that are not self-describing (e.g. JSON) or when schema information
 * is available (e.g. JSONSchema).
 */

namespace McapCs.Record;
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
