namespace McapCs.Record;

/// <summary>
/// Named map of key/value strings in Data section (outside chunks).
/// データセクションにある名前付きメタデータ。
/// </summary>
public struct Metadata
{
  public string name = string.Empty;
  public Dictionary<string, string> metadata = new Dictionary<string, string>();

  public Metadata()
  {
  }
}
