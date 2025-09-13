namespace McapCs.Record;

/// <summary>
/// Final record in the Data section; may include CRC for the whole section.
/// データセクションの最後のレコード。全体 CRC を含む場合あり。
/// </summary>
public struct DataEnd
{
  public uint dataSectionCrc;
  public DataEnd()
  {

  }
}
