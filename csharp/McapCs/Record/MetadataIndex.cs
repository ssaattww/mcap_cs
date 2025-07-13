/**
 * @brief Metadata Index records are found in the Summary section, providing
 * summary information for a single Metadata record.
 */
namespace McapCs.Record;

using McapCs.Util;

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
