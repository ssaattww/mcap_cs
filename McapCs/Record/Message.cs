namespace McapCs.Record;
/// <summary>
/// A single message published to a channel.
/// チャネルに発行された 1 件のメッセージ。
/// </summary>
public struct Message
{
  public ushort channelId;
  /**
   * @brief An optional sequence number. If non-zero, sequence numbers should be
   * unique per channel and increasing over time.
   */
  public uint sequence;
  /**
   * @brief Nanosecond timestamp when this message was recorded or received for
   * recording.
   */
  public ulong logTime;
  /**
   * @brief Nanosecond timestamp when this message was initially published. If
   * not available, this should be set to `logTime`.
   */
  public ulong publishTime;
  /**
   * @brief Size of the message payload in bytes, pointed to via `data`.
   */
  public ulong dataSize;
  /**
   * @brief A pointer to the message payload. For readers, this pointer is only
   * valid for the lifetime of an onMessage callback or before the message
   * iterator is advanced.
   */
  public List<byte> data = new List<byte>();

  public Message()
  {
  }
};
