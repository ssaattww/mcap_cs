namespace Mcap.CSharp.Mcap.Util
{
    // Corresponds to C++ crc32.hpp in cpp/mcap/include/mcap/crc32.hpp
    public static class Crc32
    {
        private static readonly uint[] Table;

        static Crc32()
        {
            Table = new uint[256];
            uint poly = 0xEDB88320;
            for (uint i = 0; i < 256; i++)
            {
                uint entry = i;
                for (int j = 0; j < 8; j++)
                {
                    if ((entry & 1) == 1)
                    {
                        entry = (entry >> 1) ^ poly;
                    }
                    else
                    {
                        entry = entry >> 1;
                    }
                }
                Table[i] = entry;
            }
        }

        public static uint Compute(byte[] data)
        {
            return Compute(0xFFFFFFFF, data);
        }

        public static uint Compute(uint initialCrc, byte[] data)
        {
            uint crc = initialCrc ^ 0xFFFFFFFF;
            foreach (byte b in data)
            {
                crc = (crc >> 8) ^ Table[(crc & 0xFF) ^ b];
            }
            return crc ^ 0xFFFFFFFF;
        }
    }
}
