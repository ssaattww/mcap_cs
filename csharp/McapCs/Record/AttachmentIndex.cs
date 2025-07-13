/**
 * @brief Attachment Index records are found in the Summary section, providing
 * summary information for a single Attachment.
 */
namespace McapCs.Record;

public struct AttachmentIndex
{
  public ulong offset;
  public ulong length;
  public ulong logTime;
  public ulong createTime;
  public ulong dataSize;
  public string name = string.Empty;
  public string mediaType = string.Empty;

  public AttachmentIndex()
  {

  }

  public AttachmentIndex(Attachment attachment, ulong fileOffset)
  {
    offset = fileOffset;
    length = 9 + /* name */ 4 + (ulong)attachment.name.Length + /* log_time */ 8 + /* create_time */ 8 + /* media_type */ 4 + (ulong)attachment.mediaType.Length + /* data */ 8 + attachment.dataSize + /* crc */ 4;
    logTime = attachment.logTime;
    createTime = attachment.createTime;
    dataSize = attachment.dataSize;
    name = attachment.name;
    mediaType = attachment.mediaType;
  }
}
