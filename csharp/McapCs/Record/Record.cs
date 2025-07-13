/**
 * @brief A generic Type-Length-Value record using a uint8 type and uint64
 * length. This is the generic form of all MCAP records.
 */
namespace McapCs.Record;
public struct Record
{
  public EOpCode opcode;
  public ulong dataSize;
  public byte[] data;

  public ulong RecordSize
  {
    get
    {
      return sizeof(EOpCode) + sizeof(ulong) + dataSize;
    }
  }
}
