/**
 * @brief The Statistics record is found in the Summary section, providing
 * counts and timestamp ranges for the entire file.
 */
namespace McapCs.Record;

public struct Statistics
{
  public ulong messageCount;
  public ushort schemaCount;
  public uint channelCount;
  public uint attachmentCount;
  public uint metadataCount;
  public uint chunkCount;
  public ulong messageStartTime;
  public ulong messageEndTime;
  public Dictionary<ushort, ulong> channelMessageCounts = new Dictionary<ushort, ulong>();

  public Statistics()
  {
  }
}
