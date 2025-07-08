using System.IO;

namespace Mcap.CSharp.Mcap.Interfaces;

/// <summary>
/// Corresponds to the C++ `mcap::IWritable` interface.
/// </summary>
public interface IWritable
{
    bool CrcEnabled { get; set; }
    void Write(byte[] data, ulong size);
    void Write(BinaryWriter writer);
    void End();
    ulong Size();
    uint Crc();
    void ResetCrc();
    void Flush();
}
