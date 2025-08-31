using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using McapCs.Record;

namespace McapCs.Reader;

public class McapReader : IDisposable
{
  private Stream? _stream;
  private BinaryReader? _reader;

  public Header? Header { get; private set; }
  public Footer? Footer { get; private set; }
  public Dictionary<ushort, Schema> Schemas { get; } = new();
  public Dictionary<ushort, Channel> Channels { get; } = new();
  public List<Message> Messages { get; } = new();

  public void Open(string filePath)
  {
    Close();
    _stream = File.OpenRead(filePath);
    _reader = new BinaryReader(_stream, Encoding.UTF8, leaveOpen: true);
    ReadMagic(_reader);
    // Top-level records follow
    ReadRecords(_reader, lengthLimit: -1);
  }

  public void Open(Stream stream)
  {
    Close();
    _stream = stream;
    _reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
    ReadMagic(_reader);
    ReadRecords(_reader, lengthLimit: -1);
  }

  public IEnumerable<Message> ReadMessages() => Messages;

  public void Close()
  {
    _reader?.Dispose();
    _reader = null;
    _stream?.Dispose();
    _stream = null;
    Header = null;
    Footer = null;
    Schemas.Clear();
    Channels.Clear();
    Messages.Clear();
  }

  public void Dispose() => Close();

  private static void ReadMagic(BinaryReader reader)
  {
    var magic = reader.ReadBytes(Constants.Magic.Length);
    if (magic.Length != Constants.Magic.Length)
    {
      throw new InvalidDataException("MCAP: failed to read magic header");
    }
    for (int i = 0; i < magic.Length; i++)
    {
      if (magic[i] != Constants.Magic[i])
      {
        throw new InvalidDataException("MCAP: invalid magic header");
      }
    }
  }

  private void ReadRecords(BinaryReader reader, long lengthLimit)
  {
    long startPos = reader.BaseStream.Position;
    while (true)
    {
      if (lengthLimit >= 0)
      {
        long readSoFar = reader.BaseStream.Position - startPos;
        if (readSoFar >= lengthLimit) break;
      }
      if (reader.BaseStream.Position >= reader.BaseStream.Length) break;

      int op = reader.Read();
      if (op == -1) break;
      var opcode = (EOpCode)(byte)op;
      ulong dataSize = reader.ReadUInt64();

      switch (opcode)
      {
        case EOpCode.Header:
          Header = new Header
          {
            profile = ReadString(reader),
            library = ReadString(reader),
          };
          break;

        case EOpCode.Schema:
        {
          Schema schema = new Schema();
          schema.id = reader.ReadUInt16();
          schema.name = ReadString(reader);
          schema.encoding = ReadString(reader);
          schema.data = new List<byte>(ReadBytesWithLength(reader));
          Schemas[schema.id] = schema;
          break;
        }

        case EOpCode.Channel:
        {
          Channel channel = new Channel();
          channel.id = reader.ReadUInt16();
          channel.schemaId = reader.ReadUInt16();
          channel.topic = ReadString(reader);
          channel.messageEncoding = ReadString(reader);
          uint mdSize = reader.ReadUInt32();
          channel.metadata = ReadKeyValueMap(reader, mdSize);
          Channels[channel.id] = channel;
          break;
        }

        case EOpCode.Message:
        {
          Message msg = new Message();
          msg.channelId = reader.ReadUInt16();
          msg.sequence = reader.ReadUInt32();
          msg.logTime = reader.ReadUInt64();
          msg.publishTime = reader.ReadUInt64();
          uint dataLen = reader.ReadUInt32();
          var payload = reader.ReadBytes((int)dataLen);
          msg.data = new List<byte>(payload);
          msg.dataSize = dataLen;
          Messages.Add(msg);
          break;
        }

        case EOpCode.Chunk:
        {
          // Read chunk header
          ulong messageStartTime = reader.ReadUInt64();
          ulong messageEndTime = reader.ReadUInt64();
          ulong uncompressedSize = reader.ReadUInt64();
          uint uncompressedCrc = reader.ReadUInt32();
          string compression = ReadString(reader);
          ulong compressedSize = reader.ReadUInt64();
          // Only support non-compressed chunks for now
          if (!string.IsNullOrEmpty(compression) && !compression.Equals("none", StringComparison.OrdinalIgnoreCase))
          {
            throw new NotSupportedException($"MCAP: compressed chunk '{compression}' is not supported");
          }
          var recordsBytes = reader.ReadBytes((int)compressedSize);
          using var ms = new MemoryStream(recordsBytes, writable: false);
          using var subReader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
          ReadRecords(subReader, (long)compressedSize);
          break;
        }

        case EOpCode.Footer:
        {
          Footer = new Footer
          {
            summaryStart = reader.ReadUInt64(),
            summaryOffsetStart = reader.ReadUInt64(),
            summaryCrc = reader.ReadUInt32(),
          };
          break;
        }

        case EOpCode.Attachment:
        case EOpCode.AttachmentIndex:
        case EOpCode.MessageIndex:
        case EOpCode.ChunkIndex:
        case EOpCode.Statistics:
        case EOpCode.Metadata:
        case EOpCode.MetadataIndex:
        case EOpCode.SummaryOffset:
        case EOpCode.DataEnd:
          // Skip unsupported records for now
          reader.BaseStream.Seek((long)dataSize, SeekOrigin.Current);
          break;

        default:
          // Unknown record type — skip payload
          reader.BaseStream.Seek((long)dataSize, SeekOrigin.Current);
          break;
      }
    }
  }

  private static string ReadString(BinaryReader reader)
  {
    uint len = reader.ReadUInt32();
    if (len == 0) return string.Empty;
    var bytes = reader.ReadBytes((int)len);
    return Encoding.UTF8.GetString(bytes);
  }

  private static byte[] ReadBytesWithLength(BinaryReader reader)
  {
    uint len = reader.ReadUInt32();
    if (len == 0) return Array.Empty<byte>();
    return reader.ReadBytes((int)len);
  }

  private static Dictionary<string, string> ReadKeyValueMap(BinaryReader reader, uint totalSize)
  {
    var map = new Dictionary<string, string>();
    if (totalSize == 0) return map;
    long start = reader.BaseStream.Position;
    while (reader.BaseStream.Position - start < totalSize)
    {
      string key = ReadString(reader);
      string value = ReadString(reader);
      map[key] = value;
    }
    // If the encoding contained padding (shouldn't), ensure we align to totalSize
    long toSkip = (start + totalSize) - reader.BaseStream.Position;
    if (toSkip > 0) reader.BaseStream.Seek(toSkip, SeekOrigin.Current);
    return map;
  }
}

