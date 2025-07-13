namespace McapCs.Types;
public enum StatusCode
{
  Success = 0,
  NotOpen,
  InvalidSchemaId,
  InvalidChannelId,
  FileTooSmall,
  ReadFailed,
  MagicMismatch,
  InvalidFile,
  InvalidRecord,
  InvalidOpCode,
  InvalidChunkOffset,
  InvalidFooter,
  DecompressionFailed,
  DecompressionSizeMismatch,
  UnrecognizedCompression,
  OpenFailed,
  MissingStatistics,
  InvalidMessageReadOptions,
  NoMessageIndexesAvailable,
  UnsupportedCompression,
};

public struct Status {
  public StatusCode Code { get;}
  public string Message {get;}

  public Status()
  {
    Code = StatusCode.Success;
    Message = string.Empty;
  }

  public Status(StatusCode code)
  {
    Code = code;
    Message = string.Empty;
    switch (Code)
    {
      case StatusCode.Success:
        break;
      case StatusCode.NotOpen:
        Message = "not open";
        break;
      case StatusCode.InvalidSchemaId:
        Message = "invalid schema id";
        break;
      case StatusCode.InvalidChannelId:
        Message = "invalid channel id";
        break;
      case StatusCode.FileTooSmall:
        Message = "file too small";
        break;
      case StatusCode.ReadFailed:
        Message = "read failed";
        break;
      case StatusCode.MagicMismatch:
        Message = "magic mismatch";
        break;
      case StatusCode.InvalidFile:
        Message = "invalid file";
        break;
      case StatusCode.InvalidRecord:
        Message = "invalid record";
        break;
      case StatusCode.InvalidOpCode:
        Message = "invalid opcode";
        break;
      case StatusCode.InvalidChunkOffset:
        Message = "invalid chunk offset";
        break;
      case StatusCode.InvalidFooter:
        Message = "invalid footer";
        break;
      case StatusCode.DecompressionFailed:
        Message = "decompression failed";
        break;
      case StatusCode.DecompressionSizeMismatch:
        Message = "decompression size mismatch";
        break;
      case StatusCode.UnrecognizedCompression:
        Message = "unrecognized compression";
        break;
      case StatusCode.OpenFailed:
        Message = "open failed";
        break;
      case StatusCode.MissingStatistics:
        Message = "missing statistics";
        break;
      case StatusCode.InvalidMessageReadOptions:
        Message = "message read options conflict";
        break;
      case StatusCode.NoMessageIndexesAvailable:
        Message = "file has no message indices";
        break;
      case StatusCode.UnsupportedCompression:
        Message = "unsupported compression";
        break;
      default:
        Message = "unknown";
        break;
    }
  }

  public Status(StatusCode code, string message)
  {
    Code = code;
    Message = message;
  }
  public bool Ok{
    get { return Code == StatusCode.Success; }
  }
};