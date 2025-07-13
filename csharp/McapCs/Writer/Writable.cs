using McapCs.Crc;

namespace McapCs.Writer;

public abstract class Writable
{

    public abstract void End();
    public abstract ulong Size();
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
