/**
 * @brief Returned when iterating over Messages in a file, MessageView contains
 * a reference to one Message, a pointer to its Channel, and an optional pointer
 * to that Channel's Schema. The Channel pointer is guaranteed to be valid,
 * while the Schema pointer may be null if the Channel references schema_id 0.
 */
namespace McapCs.Record;

public struct RecordOffset
{
  public ulong offset;
  public ulong? chunkOffset;

  public RecordOffset()
  {
    offset = 0;
    chunkOffset = null;
  }

  public RecordOffset(ulong offset_)
  {
    offset = offset_;
    chunkOffset = null;
  }

  public RecordOffset(ulong offset_, ulong chunkOffset_)
  {
    offset = offset_;
    chunkOffset = chunkOffset_;
  }

  public static bool operator ==(RecordOffset left, RecordOffset right)
  {
    return left.offset == right.offset && left.chunkOffset == right.chunkOffset;
  }

  public static bool operator !=(RecordOffset left, RecordOffset right)
  {
    return !(left == right);
  }

  public static bool operator >(RecordOffset left, RecordOffset right)
  {
    if (left.offset != right.offset)
    {
      return left.offset > right.offset;
    }
    return left.chunkOffset > right.chunkOffset;
  }

  public static bool operator <(RecordOffset left, RecordOffset right)
  {
    return !(left > right) && !(left == right);
  }

  public static bool operator >=(RecordOffset left, RecordOffset right)
  {
    return (left == right) || (left > right);
  }

  public static bool operator <=(RecordOffset left, RecordOffset right)
  {
    return !(left > right);
  }

  public override bool Equals(object? obj)
  {
    return obj is RecordOffset other && this == other;
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(offset, chunkOffset);
  }
}
