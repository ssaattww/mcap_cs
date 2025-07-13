/**
 * @brief Chunk Index records are found in the Summary section, providing
 * summary information for a single Chunk and pointing to each Message Index
 * record associated with that Chunk.
 */
namespace McapCs.Record;

public struct ChunkIndex
{
  public ulong messageStartTime;
  public ulong messageEndTime;
  public ulong chunkStartOffset;
  public ulong chunkLength;
  public Dictionary<ushort, ulong> messageIndexOffsets;
  public ulong messageIndexLength;
  public string compression;
  public ulong compressedSize;
  public ulong uncompressedSize;

  public ChunkIndex()
  {
  }
}
