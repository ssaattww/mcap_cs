using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records
{
    /// <summary>
    /// Corresponds to the C++ `mcap::Header` struct (defined in `cpp/mcap/include/mcap/types.hpp`).
    /// </summary>
    public class Header : IRecordSerializable
    {
        public string Profile { get; set; } = "";
        public string Library { get; set; } = "";

        public void Write(BinaryWriter writer)
        {
            writer.Write((uint)Profile.Length);
            writer.Write(System.Text.Encoding.UTF8.GetBytes(Profile));
            writer.Write((uint)Library.Length);
            writer.Write(System.Text.Encoding.UTF8.GetBytes(Library));
        }
    }
}
