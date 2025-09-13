using McapCs.Crc;
using System.IO;

namespace McapCs.Writer;

public abstract class ChunkWriter : Writable
{
    /// <summary>
    /// Size of the compressed data in bytes (or uncompressed size if not compressed).
    /// 圧縮データのサイズ（非圧縮時は非圧縮サイズ）。
    /// </summary>
    public abstract ulong CompressedSize { get; }
    /// <summary>
    /// True if the chunk holds no data.
    /// チャンクが空の場合に true。
    /// </summary>
    public abstract bool Empty { get; }
    /// <summary>
    /// Uncompressed bytes accumulated for this chunk.
    /// このチャンクに蓄積された非圧縮のバイト列。
    /// </summary>
    public abstract byte[] Data { get; }
    /// <summary>
    /// Compressed byte representation of the chunk (may equal <see cref="Data"/>).
    /// 圧縮後のバイト列（<see cref="Data"/> と同一の場合あり）。
    /// </summary>
    public abstract byte[] CompressedData { get; }
    abstract protected void HandleClear();

    public void Clear()
    {
        HandleClear();
        ResetCrc();
    }
}
