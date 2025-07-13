/**
 * @brief Appears at the beginning of every MCAP file (after the magic byte
 * sequence) and contains the recording profile (see
 * <https://github.com/foxglove/mcap/tree/main/docs/specification/profiles>) and
 * a string signature of the recording library.
 */

namespace McapCs.Record;

public struct Header
{
  public string profile;
  public string library;
}
