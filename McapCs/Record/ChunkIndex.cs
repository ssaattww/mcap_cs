namespace McapCs.Record;

/// <summary>
/// Summary info for a chunk and pointers to its message indexes.
/// チャンクのサマリ情報とメッセージインデックスへの参照。
/// </summary>
public struct ChunkIndex
{
  public ulong messageStartTime;
  public ulong messageEndTime;
  public ulong chunkStartOffset;
  public ulong chunkLength;
  public Dictionary<ushort, ulong> messageIndexOffsets = new Dictionary<ushort, ulong>();
  public ulong messageIndexLength;
  public string compression = string.Empty;
  public ulong compressedSize;
  public ulong uncompressedSize;

  public ChunkIndex()
  {
  }
}
