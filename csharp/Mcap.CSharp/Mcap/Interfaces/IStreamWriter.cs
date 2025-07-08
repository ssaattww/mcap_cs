using System.IO;

namespace Mcap.CSharp.Mcap.Interfaces;

/// <summary>
/// Represents a writer that can seek within the underlying stream.
/// </summary>
public interface IStreamWriter : IWritable
{
    void Seek(long offset, SeekOrigin origin);
}
