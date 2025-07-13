
namespace McapCs.Crc;

public static class Crc32
{
    public const uint InitialValue = 0xFFFFFFFF;
    private const uint Polynomial = 0xedb88320;
    private static readonly uint[] Table;

    static Crc32()
    {
        // Slicing-by-8 table generation from the C++ implementation
        Table = new uint[256 * 8];
        for (uint i = 0; i < 256; i++)
        {
            uint r = i;
            r = ((r & 1) * Polynomial) ^ (r >> 1);
            r = ((r & 1) * Polynomial) ^ (r >> 1);
            r = ((r & 1) * Polynomial) ^ (r >> 1);
            r = ((r & 1) * Polynomial) ^ (r >> 1);
            r = ((r & 1) * Polynomial) ^ (r >> 1);
            r = ((r & 1) * Polynomial) ^ (r >> 1);
            r = ((r & 1) * Polynomial) ^ (r >> 1);
            r = ((r & 1) * Polynomial) ^ (r >> 1);
            Table[i] = r;
        }
        for (int i = 256; i < Table.Length; i++)
        {
            uint value = Table[i - 256];
            Table[i] = Table[value & 0xff] ^ (value >> 8);
        }
    }

    public static uint Update(uint runningCrc, byte[] data, int offset)
    {
        // In C++, data manipulation often uses pointer arithmetic (e.g., `data + offset`).
        // In C#, the `offset` parameter serves a similar purpose, allowing CRC calculation
        // to start from a specific index within the `data` byte array.
        // C++では、データ操作にポインタ演算（例: `data + offset`）がよく使用されます。
        // C#では、`offset` パラメータが同様の目的を果たし、`data` バイト配列内の特定のインデックスからCRC計算を開始できます。
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        int count = data.Length;
        if (offset < 0 || count < 0 || offset + count > data.Length)
        {
            throw new ArgumentOutOfRangeException();
        }


        uint r = runningCrc;
        int end = offset + count;

        // Process 8 bytes at a time.
        for (; count >= 8; offset += 8, count -= 8)
        {
            r ^= GetUint32LE(data, offset);
            uint r2 = GetUint32LE(data, offset + 4);
            r = Table[0 * 256 + ((r2 >> 24) & 0xff)] ^ Table[1 * 256 + ((r2 >> 16) & 0xff)] ^
                Table[2 * 256 + ((r2 >> 8) & 0xff)] ^ Table[3 * 256 + ((r2 >> 0) & 0xff)] ^
                Table[4 * 256 + ((r >> 24) & 0xff)] ^ Table[5 * 256 + ((r >> 16) & 0xff)] ^
                Table[6 * 256 + ((r >> 8) & 0xff)] ^ Table[7 * 256 + ((r >> 0) & 0xff)];
        }

        // Process any remaining bytes one by one.
        for (; offset < end; offset++)
        {
            r = Table[(r ^ data[offset]) & 0xff] ^ (r >> 8);
        }
        return r;
    }

    public static uint Final(uint crc)
    {
        return crc ^ 0xFFFFFFFF;
    }

    private static uint GetUint32LE(byte[] data, int offset)
    {
        return (uint)(data[offset] | (data[offset + 1] << 8) | (data[offset + 2] << 16) | (data[offset + 3] << 24));
    }
}
