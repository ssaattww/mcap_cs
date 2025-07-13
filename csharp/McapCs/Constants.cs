namespace McapCs;

public static class Constants
{
  public const string MCAP_LIBRARY_VERSION = "2.0.2";
  public const ulong DefaultChunkSize = 1024 * 768;
  public const char SpecVersion = '0';
  public static readonly byte[] Magic = {137, 77, 67, 65, 80, (byte)SpecVersion, 13, 10};
  public const ulong EndOffset = ulong.MaxValue;
  public const ulong MaxTime = ulong.MaxValue;
}