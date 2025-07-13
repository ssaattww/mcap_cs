using McapCs.Record;
using McapCs.Types;

namespace McapCs.Writer;

public class McapWriter {

  /**
   * @brief Open a new MCAP file for writing and write the header.
   *
   * If the writer was already opened, this calls `close`() first to reset the state.
   * A writer may be re-used after being reset via `close`() or `terminate`().
   *
   * @param filename Filename of the MCAP file to write.
   * @param options Options for MCAP writing. `profile` is required.
   * @return A non-success status if the file could not be opened for writing.
   */
  public Status open(string filename, McapWriterOptions options);

  /**
   * @brief Open a new MCAP file for writing and write the header.
   *
   * If the writer was already opened, this calls `close`() first to reset the state.
   * A writer may be re-used after being reset via `close`() or `terminate`().
   *
   * @param writer An implementation of the Writable interface. Output bytes
   *   will be written to this object.
   * @param options Options for MCAP writing. `profile` is required.
   */
  public void open(Writable writer, McapWriterOptions options);

  /**
   * @brief Open a new MCAP file for writing and write the header.
   *
   * @param stream Output stream to write to.
   * @param options Options for MCAP writing. `profile` is required.
   */
  public void open(Stream stream, McapWriterOptions options);

  /**
   * @brief Write the MCAP footer, flush pending writes to the output stream,
   * and reset internal state. The writer may be re-used with another call to open afterwards.
   */
  public void close();

  /**
   * @brief Reset internal state without writing the MCAP footer or flushing
   * pending writes. This should only be used in error cases as the output MCAP
   * file will be truncated. The writer may be re-used with another call to open afterwards.
   */
  public void terminate();

  /**
   * @brief Add a new schema to the MCAP file and set `schema.id` to a generated
   * schema id. The schema id is used when adding channels to the file.
   *
   * Schemas are not cleared when the state is reset via `close`() or `terminate`().
   * If you're re-using a writer for multiple files in a row, the schemas only need
   * to be added once, before first use.
   *
   * This method does not de-duplicate schemas.
   *
   * @param schema Description of the schema to register. The `id` field is
   *   ignored and will be set to a generated schema id.
   */
  public void addSchema(Schema schema);

  /**
   * @brief Add a new channel to the MCAP file and set `channel.id` to a
   * generated channel id. The channel id is used when adding messages to the
   * file.
   *
   * Channels are not cleared when the state is reset via `close`() or `terminate`().
   * If you're re-using a writer for multiple files in a row, the channels only need
   * to be added once, before first use.
   *
   * This method does not de-duplicate channels.
   *
   * @param channel Description of the channel to register. The `id` value is
   *   ignored and will be set to a generated channel id.
   */
  public void addChannel(Channel channel);

  /**
   * @brief Write a message to the output stream.
   *
   * @param msg Message to add.
   * @return A non-zero error code on failure.
   */
  public Status write(Message message);

  /**
   * @brief Write an attachment to the output stream.
   *
   * @param attachment Attachment to add. The `attachment.crc` will be
   * calculated and set if configuration options allow CRC calculation.
   * @return A non-zero error code on failure.
   */
  public Status write(Attachment attachment);

  /**
   * @brief Write a metadata record to the output stream.
   *
   * @param metadata Named group of key/value string pairs to add.
   * @return A non-zero error code on failure.
   */
  public Status write(Metadata metadata);

  /**
   * @brief Current MCAP file-level statistics. This is written as a Statistics
   * record in the Summary section of the MCAP file.
   */
  public Statistics statistics();

  /**
   * @brief Returns a pointer to the Writable data destination backing this
   * writer. Will return nullptr if the writer is not open.
   */
  public Writable dataSink();

  /**
   * @brief finishes the current chunk in progress and writes it to the file, if a chunk
   * is in progress.
   */
  public void closeLastChunk();

  // The following static methods are used for serialization of records and
  // primitives to an output stream. They are not intended to be used directly
  // unless you are implementing a lower level writer or tests

  public static void writeMagic(Writable output);

  public static ulong write(Writable output, Header header);
  public static ulong write(Writable output, Footer footer, bool crcEnabled);
  public static ulong write(Writable output, Schema schema);
  public static ulong write(Writable output, Channel channel);
  public static ulong getRecordSize(Message message);
  public static ulong write(Writable output, Message message);
  public static ulong write(Writable output, Attachment attachment);
  public static ulong write(Writable output, Metadata metadata);
  public static ulong write(Writable output, Chunk chunk);
  public static ulong write(Writable output, MessageIndex index);
  public static ulong write(Writable output, ChunkIndex index);
  public static ulong write(Writable output, AttachmentIndex index);
  public static ulong write(Writable output, MetadataIndex index);
  public static ulong write(Writable output, Statistics stats);
  public static ulong write(Writable output, SummaryOffset summaryOffset);
  public static ulong write(Writable output, DataEnd dataEnd);
  public static ulong write(Writable output, Record.Record record);

  public static void write(Writable output, string str);
  public static void write(Writable output, List<byte> bytes);
  public static void write(Writable output, EOpCode value);
  public static void write(Writable output, ushort value);
  public static void write(Writable output, uint value);
  public static void write(Writable output, ulong value);
  public static void write(Writable output, byte[] data, ulong size);
  public static void write(Writable output, Dictionary<string, string> map, ulong size);


  McapWriterOptions options_;
  ulong chunkSize_ = Constants.DefaultChunkSize;
  Writable? output_ = null;
  FileWriter fileOutput_;
  StreamWriter streamOutput_;
  BufferWriter uncompressedChunk_;
#ifndef MCAP_COMPRESSION_NO_LZ4
  LZ4Writer? lz4Chunk_;
#endif
#ifndef MCAP_COMPRESSION_NO_ZSTD
  ZStdWriter? zstdChunk_;
#endif
  List<Schema> schemas_;
  List<Channel> channels_;
  List<AttachmentIndex> attachmentIndex_;
  List<MetadataIndex> metadataIndex_;
  List<ChunkIndex> chunkIndex_;
  Statistics statistics_;
  HashSet<ushort> writtenSchemas_;
  Dictionary<ushort, MessageIndex> currentMessageIndex_;
  ulong currentChunkStart_ = Constants.MaxTime;
  ulong currentChunkEnd_ = 0;
  Compression compression_ = Compression.None;
  ulong uncompressedSize_ = 0;
  bool opened_ = false;

  Writable getOutput();
  ChunkWriter getChunkWriter();
  void writeChunk(Writable output, ChunkWriter chunkData);
};
