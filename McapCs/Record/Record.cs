namespace McapCs.Record;
/// <summary>
/// Generic TLV record (uint8 type, uint64 length) used by all MCAP records.
/// すべての MCAP レコードの汎用 TLV 形式。
/// </summary>
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
