namespace McapCs.Record;

/// <summary>
/// Collection of schemas/channels/messages supporting compression and indexing.
/// 圧縮と索引付けを備えたレコード集合。
/// </summary>
public struct Chunk
{
  public ulong messageStartTime;
  public ulong messageEndTime;
  public ulong uncompressedSize;
  public uint uncompressedCrc;
  public string compression = string.Empty;
  public ulong compressedSize;
  public List<byte> records = new List<byte>();

  public Chunk()
  {
  }
}
