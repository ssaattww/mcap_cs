namespace McapCs.Writer;
using McapCs.Types;

/// <summary>
/// Options controlling how MCAP files are written.
/// MCAP 書き込みのためのオプション群。
/// </summary>
public class McapWriterOptions
{
  /**
   * @brief Disable CRC calculations for Chunks.
   */
  public bool noChunkCRC = false;
  /**
   * @brief Disable CRC calculations for Attachments.
   */
  public bool noAttachmentCRC = false;
  /**
   * @brief Enable CRC calculations for all records in the data section.
   */
  public bool enableDataCRC = false;
  /**
   * @brief Disable CRC calculations for the summary section.
   */
  public bool noSummaryCRC = false;
  /**
   * @brief Do not write Chunks to the file, instead writing Schema, Channel,
   * and Message records directly into the Data section.
   */
  public bool noChunking = false;
  /**
   * @brief Do not write Message Index records to the file. If
   * `noMessageIndex=true` and `noChunkIndex=false`, Chunk Index records will
   * still be written to the Summary section, providing a coarse message index.
   */
  public bool noMessageIndex = false;
  /**
   * @brief Do not write Summary or Summary Offset sections to the file, placing
   * the Footer record immediately after DataEnd. This can provide some speed
   * boost to file writing and produce smaller files, at the expense of
   * requiring a conversion process later if fast summarization or indexed
   * access is desired.
   */
  public bool noSummary = false;
  /**
   * @brief Target uncompressed Chunk payload size in bytes. Once a Chunk's
   * uncompressed data is about to exceed this size, the Chunk will be
   * compressed (if enabled) and written to disk. Note that this is a 'soft'
   * ceiling as some Chunks could exceed this size due to either indexing
   * data or when a single message is larger than `chunkSize`, in which case,
   * the Chunk will contain only this one large message.
   * This option is ignored if `noChunking=true`.
   */
  public ulong chunkSize = Constants.DefaultChunkSize;
  /**
   * @brief Compression algorithm to use when writing Chunks. This option is
   * ignored if `noChunking=true`.
   */
  public Compression compression = Compression.Zstd;
  /**
   * @brief Compression level to use when writing Chunks. Slower generally
   * produces smaller files, at the expense of more CPU time. These levels map
   * to different internal settings for each compression algorithm.
   */
  public CompressionLevel compressionLevel = CompressionLevel.Default;
  /**
   * @brief By default, Chunks that do not benefit from compression will be
   * written uncompressed. This option can be used to force compression on all
   * Chunks. This option is ignored if `noChunking=true`.
   */
  public bool forceCompression = false;
  /**
   * @brief The recording profile. See
   * https://mcap.dev/spec/registry#well-known-profiles
   * for more information on well-known profiles.
   */
  /// <summary>
  /// Recording profile (see mcap.dev well-known profiles).
  /// 記録プロファイル。
  /// </summary>
  public string Profile { get; private set; }
  /**
   * @brief A freeform string written by recording libraries. For this library,
   * the default is "libmcap {Major}.{Minor}.{Patch}".
   */
  /// <summary>
  /// Freeform library string written into the header.
  /// ヘッダに記録されるライブラリ表記文字列。
  /// </summary>
  public string Library { get; set; } = $"libmcap {Constants.MCAP_LIBRARY_VERSION}";

  // The following options are less commonly used, providing more fine-grained
  // control of index records and the Summary section

  public bool noRepeatedSchemas = false;
  public bool noRepeatedChannels = false;
  public bool noAttachmentIndex = false;
  public bool noMetadataIndex = false;
  public bool noChunkIndex = false;
  public bool noStatistics = false;
  public bool noSummaryOffsets = false;

  /// <summary>
  /// Creates a new instance.
  /// 新しいインスタンスを作成します。
  /// </summary>
  /// <param name="profile">Recording profile. 記録プロファイル。</param>
  /// <param name="library">Library string. ライブラリ表記。</param>
  public McapWriterOptions(string profile, string library)
  {
    Profile = profile;
    Library = library;
  }
}
