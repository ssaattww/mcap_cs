/**
 * @brief The final record in an MCAP file (before the trailing magic byte
 * sequence). Contains byte offsets from the start of the file to the Summary
 * and Summary Offset sections, along with an optional CRC of the combined
 * Summary and Summary Offset sections. A `summaryStart` and
 * `summaryOffsetStart` of zero indicates no Summary section is available.
 */
namespace McapCs.Record;
public record Footer {
  public ulong summaryStart;
  public ulong summaryOffsetStart;
  public uint summaryCrc;

  public Footer()
  {

  }
  public Footer(ulong summaryStart, ulong summaryOffsetStart)
  {
    // Initialize with provided offsets and default CRC value
    this.summaryStart = summaryStart;
    this.summaryOffsetStart = summaryOffsetStart;
    summaryCrc = 0;
  }
}
