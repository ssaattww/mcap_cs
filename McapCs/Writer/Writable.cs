using McapCs.Crc;

namespace McapCs.Writer;

public abstract class Writable
{

    public abstract void End();
    /// <summary>
    /// Total number of bytes written so far.
    /// これまでに書き込んだ総バイト数。
    /// </summary>
    public abstract ulong Size{ get; }
    public abstract void Flush();
    protected abstract void HandleWrite(byte[] data);
    public void Write(byte[] data)
    {
        if (CrcEnabled)
        {
            crc = Crc32.Update(crc, data, 0);
        }

        HandleWrite(data);
    }
    public void ResetCrc()
    {
        crc = Crc32.InitialValue;
    }
    internal bool CrcEnabled { get; set; }
    /// <summary>
    /// Current CRC32 of written data when CRC is enabled; otherwise 0.
    /// CRC 有効時の現在の CRC32 値（無効時は 0）。
    /// </summary>
    public uint Crc
    {
        get
        {
            if (CrcEnabled)
            {
                return Crc32.Final(crc);
            }
            else
            {
                return 0;
            }
        }
    }
    protected uint crc = Crc32.InitialValue;
}
