namespace McapCs.Record;

/// <summary>
/// View returned when iterating: message, its channel, and optional schema.
/// メッセージと対応チャネル、任意のスキーマをまとめたビュー。
/// </summary>
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
