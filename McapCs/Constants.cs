namespace McapCs;

public static class Constants
{
  public const string MCAP_LIBRARY_VERSION = "2.0.2";
  public const ulong DefaultChunkSize = 1024 * 768;
  public const char SpecVersion = '0';
  // MCAP の先頭/末尾マジック: 0x89, 'M', 'C', 'A', 'P', '0', '\r', '\n'
  public static readonly byte[] Magic = {137, 77, 67, 65, 80, 48, 0x0D, 0x0A};
  public const ulong EndOffset = ulong.MaxValue;
  public const ulong MaxTime = ulong.MaxValue;
}
