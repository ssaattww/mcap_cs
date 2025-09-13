namespace McapCs.Record;

/// <summary>
/// Points to ranges (offset/length) of grouped Summary records by type.
/// サマリ内の各種レコードのオフセット/長さを示す。
/// </summary>
public struct SummaryOffset
{
  public EOpCode groupOpCode;
  public ulong groupStart;
  public ulong groupLength;
}
