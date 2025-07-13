/**
 * @brief The final record in the Data section, signaling the end of Data and
 * beginning of Summary. Optionally contains a CRC of the entire Data section.
 */
namespace McapCs.Record;

public struct DataEnd
{
  public uint dataSectionCrc;
  public DataEnd()
  {

  }
}
