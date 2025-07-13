/**
 * @brief Summary Offset records are found in the Summary Offset section.
 * Records in the Summary section are grouped together, and for each record type
 * found in the Summary section, a Summary Offset references the file offset and
 * length where that type of Summary record can be found.
 */
namespace McapCs.Record;

public struct SummaryOffset
{
  public EOpCode groupOpCode;
  public ulong groupStart;
  public ulong groupLength;
}
