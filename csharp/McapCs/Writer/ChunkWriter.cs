using McapCs.Crc;
using System.IO;

namespace McapCs.Writer;

abstract class ChunkWriter : Writable
{
    public abstract ulong CompressedSize { get; }
    public abstract bool Empty { get; }
    public abstract byte[] Data { get; }
    public abstract byte[] CompressedData { get; }
    abstract protected void HandleClear();

    public void Clear()
    {
        HandleClear();
        ResetCrc();
    }
}