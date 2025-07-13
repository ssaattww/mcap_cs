/**
 * @brief A list of timestamps to byte offsets for a single Channel. This record
 * appears after each Chunk, one per Channel that appeared in that Chunk.
 */
namespace McapCs.Record;

public struct MessageIndex
{
  public ushort channelId;
  public List<Tuple<ulong, ulong>> records = new List<Tuple<ulong, ulong>>();

  public MessageIndex()
  {
  }
}
