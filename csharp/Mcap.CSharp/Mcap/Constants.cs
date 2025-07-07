namespace Mcap.CSharp.Mcap;

public static class Constants
{
    public const string LibraryVersion = "2.0.2"; // Corresponds to MCAP_LIBRARY_VERSION
    public const char SpecVersion = '0'; // Corresponds to SpecVersion
    public static readonly byte[] Magic = { 137, 77, 67, 65, 80, (byte)SpecVersion, 13, 10 }; // Corresponds to Magic
    public const ulong DefaultChunkSize = 1024 * 768; // Corresponds to DefaultChunkSize
    public const ulong EndOffset = ulong.MaxValue; // Corresponds to EndOffset
    public const ulong MaxTime = ulong.MaxValue; // Corresponds to MaxTime
}
