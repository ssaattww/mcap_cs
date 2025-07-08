using System.IO;

namespace Mcap.CSharp.Mcap.Interfaces
{
    public interface IRecordSerializable
    {
        void Write(BinaryWriter writer);
    }
}
