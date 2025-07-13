/**
 * @brief MCAP record types.
 */
namespace McapCs.Record;
public enum EOpCode : byte
{
  Header = 0x01,
  Footer = 0x02,
  Schema = 0x03,
  Channel = 0x04,
  Message = 0x05,
  Chunk = 0x06,
  MessageIndex = 0x07,
  ChunkIndex = 0x08,
  Attachment = 0x09,
  AttachmentIndex = 0x0A,
  Statistics = 0x0B,
  Metadata = 0x0C,
  MetadataIndex = 0x0D,
  SummaryOffset = 0x0E,
  DataEnd = 0x0F,
};
public static class OpCode
{
  public static string OpCodeToString(EOpCode opCode)
  {
    return opCode switch
    {
      EOpCode.Header => "Header",
      EOpCode.Footer => "Footer",
      EOpCode.Schema => "Schema",
      EOpCode.Channel => "Channel",
      EOpCode.Message => "Message",
      EOpCode.Chunk => "Chunk",
      EOpCode.MessageIndex => "MessageIndex",
      EOpCode.ChunkIndex => "ChunkIndex",
      EOpCode.Attachment => "Attachment",
      EOpCode.AttachmentIndex => "AttachmentIndex",
      EOpCode.Statistics => "Statistics",
      EOpCode.Metadata => "Metadata",
      EOpCode.MetadataIndex => "MetadataIndex",
      EOpCode.SummaryOffset => "SummaryOffset",
      EOpCode.DataEnd => "DataEnd",
      _ => "Unknown"
    };
  }
}
