namespace McapCs.Record;

using McapCs.Util;

/// <summary>
/// Summary index entry for a single Metadata record.
/// メタデータレコード用のサマリ索引。
/// </summary>
public struct MetadataIndex
{
  public ulong offset;
  public ulong length;
  public string name = string.Empty;

  public MetadataIndex()
  {

  }

  public MetadataIndex(Metadata metadata, ulong fileOffset)
  {
    offset = fileOffset;
    
    // Calculate the exact length of the Metadata record based on MCAP specification
    ulong calculatedLength = 0;
    calculatedLength += 4; // Size of 'name' length (uint32)
    calculatedLength += (ulong)metadata.name.Length; // Size of 'name' string bytes

    calculatedLength += StaticMethoads.KeyValueMapSize(metadata.metadata);
    
    length = calculatedLength;
    name = metadata.name;
  }
}
