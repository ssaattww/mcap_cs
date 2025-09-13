namespace McapCs.Record;
/// <summary>
/// Final record (before trailing magic) with offsets to Summary and Summary Offset.
/// 末尾マジック直前のレコード。サマリとサマリオフセットへの位置を保持。
/// </summary>
public record Footer {
  public ulong summaryStart;
  public ulong summaryOffsetStart;
  public uint summaryCrc;

  public Footer()
  {

  }
  public Footer(ulong summaryStart, ulong summaryOffsetStart)
  {
    // Initialize with provided offsets and default CRC value
    this.summaryStart = summaryStart;
    this.summaryOffsetStart = summaryOffsetStart;
    summaryCrc = 0;
  }
}
