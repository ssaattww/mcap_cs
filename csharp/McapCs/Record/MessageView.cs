/**
 * @brief Returned when iterating over Messages in a file, MessageView contains
 * a reference to one Message, a pointer to its Channel, and an optional pointer
 * to that Channel's Schema. The Channel pointer is guaranteed to be valid,
 * while the Schema pointer may be null if the Channel references schema_id 0.
 */
namespace McapCs.Record;

public struct MessageView
{
  public Message message;
  public Channel channel;
  public Schema? schema;
  public RecordOffset messageOffset;

  public MessageView()
  {

  }
  public MessageView(Message message, Channel channel, Schema? schema, RecordOffset offset)
  {
    this.message = message;
    this.channel = channel;
    this.schema = schema;
    this.messageOffset = offset;
  }
}
