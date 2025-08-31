namespace McapCs.Util;

public static class StaticMethoads
{
    public static ulong KeyValueMapSize(Dictionary<string, string> map)
    {
        ulong size = 0;
        foreach (var entry in map)
        {
            size += 4; // Size of key length (uint32)
            size += (ulong)entry.Key.Length; // Size of key string bytes
            size += 4; // Size of value length (uint32)
            size += (ulong)System.Text.Encoding.UTF8.GetByteCount(entry.Value); // Size of value string bytes
        }
        return size;
    }
}