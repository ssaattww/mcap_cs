/**
 * @brief An collection of Schemas, Channels, and Messages that supports
 * compression and indexing.
 */
namespace McapCs.Record;

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
