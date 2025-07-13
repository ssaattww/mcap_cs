/**
 * @brief Holds a named map of key/value strings containing arbitrary user data.
 * Metadata records are found in the Data section, outside of Chunks.
 */
namespace McapCs.Record;

public struct Metadata
{
  public string name = string.Empty;
  public Dictionary<string, string> metadata = new Dictionary<string, string>();

  public Metadata()
  {
  }
}
