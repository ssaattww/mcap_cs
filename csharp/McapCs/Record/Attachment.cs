/**
 * @brief An Attachment is an arbitrary file embedded in an MCAP file, including
 * a name, media type, timestamps, and optional CRC. Attachment records are
 * written in the Data section, outside of Chunks.
 */
namespace McapCs.Record;

public struct Attachment
{
  public ulong logTime;
  public ulong createTime;
  public string name;
  public string mediaType;
  public ulong dataSize;
  public List<byte>? data;
  public uint crc;

  public Attachment()
  {
  }
}
