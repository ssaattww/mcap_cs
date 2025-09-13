namespace McapCs.Record;

/// <summary>
/// List of (logTime, offset) pairs for a single channel; follows each chunk.
/// 単一チャネルの (時刻, オフセット) の一覧。各チャンクの後に出現。
/// </summary>
public struct MessageIndex
{
  public ushort channelId;
  public List<Tuple<ulong, ulong>> records = new List<Tuple<ulong, ulong>>();

  public MessageIndex()
  {
  }
}
