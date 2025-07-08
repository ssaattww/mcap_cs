using System;
using System.IO;
using Mcap.CSharp.Mcap.Interfaces;

namespace Mcap.CSharp.Mcap.Records
{
    // Corresponds to C++ mcap::Chunk in cpp/mcap/include/mcap/records.hpp
    public class Chunk : IRecordSerializable
    {
        public ulong MessageStartTime { get; set; }
        public ulong MessageEndTime { get; set; }
        public ulong UncompressedSize { get; set; }
        public uint UncompressedCrc { get; set; }
        public string Compression { get; set; } = "";
        public ulong CompressedSize { get; set; }
        public byte[] Records { get; set; } = Array.Empty<byte>();

        public void Write(BinaryWriter writer)
        {
            writer.Write(MessageStartTime);
            writer.Write(MessageEndTime);
            writer.Write(UncompressedSize);
            writer.Write(UncompressedCrc);
            writer.Write((uint)Compression.Length);
            writer.Write(System.Text.Encoding.UTF8.GetBytes(Compression));
            writer.Write(CompressedSize);
            writer.Write((uint)Records.Length);
            writer.Write(Records);
        }

        public ulong GetRecordLength()
        {
            // Calculate the size of the record payload
            ulong length = sizeof(ulong) * 4 + sizeof(uint); // MessageStartTime, MessageEndTime, UncompressedSize, CompressedSize, UncompressedCrc
            length += (ulong)System.Text.Encoding.UTF8.GetBytes(Compression).Length + sizeof(uint); // Compression string length + string data
            length += (ulong)Records.Length + sizeof(uint); // Records data length + data
            return length;
        }
    }
}