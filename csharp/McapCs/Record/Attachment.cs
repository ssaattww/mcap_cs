namespace McapCs.Record;

/// <summary>
/// Arbitrary file embedded in the MCAP (name, media type, timestamps, optional CRC).
/// 任意のファイルを埋め込むレコード（名前/メディア種別/タイムスタンプ/CRC）。
/// </summary>
public struct Attachment
{
  public ulong logTime;
  public ulong createTime;
  public string name = string.Empty;
  public string mediaType = string.Empty;
  public ulong dataSize;
  public List<byte> data = new List<byte>();
  public uint crc;

  public Attachment()
  {
  }
}
