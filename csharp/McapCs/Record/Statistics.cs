namespace McapCs.Record;

/// <summary>
/// Statistics stored in Summary section (counts and time ranges).
/// サマリに保存される統計（件数と時刻範囲）。
/// </summary>
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
