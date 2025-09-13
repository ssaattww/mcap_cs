using McapCs.Crc;
using McapCs.Record;
using McapCs.Types;
using System;

namespace McapCs.Writer;

/// <summary>
/// MCAP file writer.
/// MCAP ファイルを書き込むためのライタークラス。
/// </summary>
public class McapWriter : IDisposable {

  /// <summary>
  /// Opens a new MCAP file and writes the header.
  /// 既に開いている場合は状態をリセットし、ヘッダーを書き込みます。
  /// </summary>
  /// <param name="filename">Path to the MCAP file to write. 書き込み先ファイルパス。</param>
  /// <param name="options">Writer options; <c>profile</c> is required. 書き込みオプション（<c>profile</c> 必須）。</param>
  /// <returns>Non-success if the file could not be opened. 開けない場合は非成功ステータス。</returns>
  public Status Open(string filename, McapWriterOptions options)
  {
    // If the writer was opened, close it first
    Close();
    fileOutput_ = new FileWriter();
    var status = fileOutput_.Open(filename);
    if (!status.Ok)
    {
      fileOutput_ = null;
      return status;
    }
    Open(fileOutput_, options);
    return new Status(StatusCode.Success);
  }

  /// <summary>
  /// Initializes the writer over a <see cref="Writable"/> and writes the header.
  /// 既存のオープン状態があればリセットして、ヘッダーを書き込みます。
  /// </summary>
  /// <param name="writer">Writable implementation to receive bytes. 出力先 Writable。</param>
  /// <param name="options">Writer options; <c>profile</c> is required. 書き込みオプション（<c>profile</c> 必須）。</param>
  // 圧縮モード（LZ4/Zstd）でも、蓄積は BufferWriter で行い、Close 時に圧縮決定します。
  public void Open(Writable writer, McapWriterOptions options)
  {
    // If the writer was opened, close it first
    Close();
    options_ = options;
    opened_ = true;
    chunkSize_ = options.noChunking ? 0 : options.chunkSize;
    compression_ = chunkSize_ > 0 ? options.compression : Compression.None;

    // ここでは蓄積用の BufferWriter を用意します（圧縮は後段の WriteChunk で実施）。
    switch (compression_)
    {
      case Compression.None:
      default:
        uncompressedChunk_ = new BufferWriter();
        break;
      case Compression.Lz4:
        // 圧縮モードでも蓄積先は BufferWriter。GetChunkWriter から同じインスタンスを返します。
        uncompressedChunk_ = new BufferWriter();
        break;
      case Compression.Zstd:
        // 圧縮モードでも蓄積先は BufferWriter。GetChunkWriter から同じインスタンスを返します。
        uncompressedChunk_ = new BufferWriter();
        break;
    }

    var chunkWriter = GetChunkWriter();
    if (chunkWriter != null)
    {
      chunkWriter.CrcEnabled = !options.noChunkCRC;
      if (chunkWriter.CrcEnabled)
      {
        chunkWriter.ResetCrc();
      }
    }
    // データセクション全体の CRC を有効化（オプション）。
    writer.CrcEnabled = options.enableDataCRC;
    output_ = writer;
    WriteMagic(writer);
    Write(writer, new Header { profile = options.Profile, library = Constants.MCAP_LIBRARY_VERSION });
  }

  /// <summary>
  /// Opens the writer on a <see cref="Stream"/> and writes the header.
  /// ストリーム上でライターを開き、ヘッダーを書き込みます。
  /// </summary>
  /// <param name="stream">Destination stream. 出力ストリーム。</param>
  /// <param name="options">Writer options. 書き込みオプション。</param>
  public void Open(Stream stream, McapWriterOptions options)
  {
    // If the writer was opened, close it first
    Close();
    streamOutput_ = new StreamWriter(stream);
    Open(streamOutput_, options);
  }

  /// <summary>
  /// Finishes the current in-progress chunk and writes it, if any.
  /// 進行中のチャンクがあれば閉じて書き込みます。
  /// </summary>
  public void CloseLastChunk()
  {
    if (!opened_ || output_ == null)
    {
      return;
    }
    var fileOutput = output_;
    var chunkWriter = GetChunkWriter();
    if (chunkWriter != null && !chunkWriter.Empty)
    {
      WriteChunk(fileOutput, chunkWriter);
    }
  }

  /// <summary>
  /// Writes the footer, flushes the stream, and resets internal state.
  /// フッターを書き込み、フラッシュ後に内部状態をリセットします。
  /// </summary>
  public void Close()
  {
    if (!opened_ || output_ == null)
    {
      return;
    }
    CloseLastChunk();

    var fileOutput = output_;

    // Write the Data End record
    Write(fileOutput, new DataEnd { dataSectionCrc = fileOutput.Crc });
#if DEBUG
    Console.WriteLine($"After DataEnd: {fileOutput.Size}");
#endif
    if (!options_.noSummaryCRC)
    {
      output_.CrcEnabled = true;
      output_.ResetCrc();
    }

    ulong summaryStart = 0;
    ulong summaryOffsetStart = 0;

    if (!options_.noSummary)
    {
      // Get the offset of the End Of File section
      summaryStart = fileOutput.Size;
#if DEBUG
      Console.WriteLine($"Summary start: {summaryStart}");
#endif
      ulong schemaStart = fileOutput.Size;
      ulong schemasLength = 0;
      if (!options_.noRepeatedSchemas)
      {
        // Write all schema records
        foreach (var schema in schemas_)
        {
          schemasLength += Write(fileOutput, schema);
        }
      }
#if DEBUG
      Console.WriteLine($"After schemas: {fileOutput.Size}");
#endif
      ulong channelStart = fileOutput.Size;
      ulong channelLength = 0;
      if (!options_.noRepeatedChannels)
      {
        // Write all channel records, but only if they appeared in this file
        var channelMessageCounts = statistics_.channelMessageCounts;
        foreach (var channel in channels_)
        {
          if (channelMessageCounts.ContainsKey(channel.id))
          {
            channelLength += Write(fileOutput, channel);
          }
        }
      }
#if DEBUG
      Console.WriteLine($"After channels: {fileOutput.Size}");
#endif
      ulong statisticsStart = fileOutput.Size;
      ulong statisticsLength = 0;
      if (!options_.noStatistics)
      {
        // Write the statistics record
        statisticsLength = Write(fileOutput, statistics_);
      }
#if DEBUG
      Console.WriteLine($"After statistics: {fileOutput.Size}");
#endif
      ulong chunkIndexStart = fileOutput.Size;
      ulong chunkIndexLength = 0;
      if (!options_.noChunkIndex)
      {
        // Write chunk index records
        foreach (var chunkIndexRecord in chunkIndex_)
        {
          chunkIndexLength += Write(fileOutput, chunkIndexRecord);
        }
      }
#if DEBUG
      Console.WriteLine($"After chunk index: {fileOutput.Size}");
#endif
      ulong attachmentIndexStart = fileOutput.Size;
      ulong attachmentIndexLength = 0;
      if (!options_.noAttachmentIndex)
      {
        // Write attachment index records
        foreach (var attachmentIndexRecord in attachmentIndex_)
        {
          attachmentIndexLength += Write(fileOutput, attachmentIndexRecord);
        }
      }
#if DEBUG
      Console.WriteLine($"After attachment index: {fileOutput.Size}");
#endif
      ulong metadataIndexStart = fileOutput.Size;
      ulong metadataIndexLength = 0;
      if (!options_.noMetadataIndex)
      {
        // Write metadata index records
        foreach (var metadataIndexRecord in metadataIndex_)
        {
          metadataIndexLength += Write(fileOutput, metadataIndexRecord);
        }
      }
#if DEBUG
      Console.WriteLine($"After metadata index: {fileOutput.Size}");
#endif
      if (!options_.noSummaryOffsets)
      {
        // Write summary offset records
        summaryOffsetStart = fileOutput.Size;
#if DEBUG
        Console.WriteLine($"Summary offset start: {summaryOffsetStart}");
#endif
        if (!options_.noRepeatedSchemas && writtenSchemas_.Count > 0)
        {
          Write(fileOutput, new SummaryOffset { groupOpCode = EOpCode.Schema, groupStart = schemaStart, groupLength = schemasLength });
        }
        if (!options_.noRepeatedChannels && channels_.Count > 0)
        {
          Write(fileOutput, new SummaryOffset { groupOpCode = EOpCode.Channel, groupStart = channelStart, groupLength = channelLength });
        }
        if (!options_.noStatistics)
        {
          Write(fileOutput, new SummaryOffset { groupOpCode = EOpCode.Statistics, groupStart = statisticsStart, groupLength = statisticsLength });
        }
        if (!options_.noChunkIndex && chunkIndex_.Count > 0)
        {
          Write(fileOutput, new SummaryOffset { groupOpCode = EOpCode.ChunkIndex, groupStart = chunkIndexStart, groupLength = chunkIndexLength });
        }
        if (!options_.noAttachmentIndex && attachmentIndex_.Count > 0)
        {
          Write(fileOutput, new SummaryOffset { groupOpCode = EOpCode.AttachmentIndex, groupStart = attachmentIndexStart, groupLength = attachmentIndexLength });
        }
        if (!options_.noMetadataIndex && metadataIndex_.Count > 0)
        {
          Write(fileOutput, new SummaryOffset { groupOpCode = EOpCode.MetadataIndex, groupStart = metadataIndexStart, groupLength = metadataIndexLength });
        }
      } else if (summaryStart == fileOutput.Size)
      {
        // No summary records were written
        summaryStart = 0;
      }
    }

    // Write the footer and trailing magic
    Write(fileOutput, new Footer { summaryStart = summaryStart, summaryOffsetStart = summaryOffsetStart }, !options_.noSummaryCRC);
#if DEBUG
    Console.WriteLine($"After Footer: {fileOutput.Size}");
#endif
    WriteMagic(fileOutput);
#if DEBUG
    Console.WriteLine($"After Magic: {fileOutput.Size}");
#endif
    // Flush output
    output_.Flush();
    fileOutput.End();

    Terminate();
  }

  /// <summary>
  /// Resets internal state without writing a footer or flushing.
  /// エラー時などにフッターを書かずに状態のみリセットします。
  /// </summary>
  public void Terminate()
  {
    output_ = null;
    fileOutput_ = null;
    streamOutput_ = null;
    uncompressedChunk_ = null;

    attachmentIndex_.Clear();
    metadataIndex_.Clear();
    chunkIndex_.Clear();
    statistics_ = new Statistics();
    writtenSchemas_.Clear();
    currentMessageIndex_.Clear();
    currentChunkStart_ = Constants.MaxTime;
    currentChunkEnd_ = 0;
    compression_ = Compression.None;
    uncompressedSize_ = 0;

    // Don't clear schemas or channels, those can be re-used between files
    // Only the channels and schemas actually referenced in the file will be written to it.

    opened_ = false;
  }

  /// <summary>
  /// Disposes the writer, closing the file if open.
  /// ライターを破棄し、必要に応じてクローズします。
  /// </summary>
  public void Dispose()
  {
    Close();
  }

  /// <summary>
  /// Adds a new schema and assigns a generated <c>schema.id</c>.
  /// スキーマを登録し、生成した <c>schema.id</c> を設定します。
  /// </summary>
  /// <param name="schema">Schema to register. 登録するスキーマ。</param>
  public void AddSchema(Schema schema)
  {
    schema.id = (ushort)(schemas_.Count + 1);
    schemas_.Add(schema);
  }

  /// <summary>
  /// Adds a new channel and assigns a generated <c>channel.id</c>.
  /// チャネルを登録し、生成した <c>channel.id</c> を設定します。
  /// </summary>
  /// <param name="channel">Channel to register. 登録するチャネル。</param>
  public void AddChannel(Channel channel)
  {
    channel.id = (ushort)(channels_.Count + 1);
    channels_.Add(channel);
  }

  /// <summary>
  /// Writes a message to the output stream.
  /// メッセージを出力ストリームに書き込みます。
  /// </summary>
  /// <param name="message">Message to add. 追加するメッセージ。</param>
  /// <returns>Non-zero error code on failure. 失敗時は非 0。</returns>
  public Status Write(Message message)
  {
    if (output_ == null)
    {
      return new Status(StatusCode.NotOpen);
    }
    var output = GetOutput();
    var channelMessageCounts = statistics_.channelMessageCounts;

    // Write out Channel if we have not yet done so
    if (!channelMessageCounts.ContainsKey(message.channelId))
    {
      var channelIndex = message.channelId - 1;
      if (channelIndex >= channels_.Count)
      {
        return new Status(StatusCode.InvalidChannelId, $"invalid channel id {message.channelId}");
      }

      var channel = channels_[(int)channelIndex];

      // Check if the Schema record needs to be written
      if ((channel.schemaId != 0) &&
          (!writtenSchemas_.Contains(channel.schemaId)))
      {
        var schemaIndex = channel.schemaId - 1;
        if (schemaIndex >= schemas_.Count)
        {
          return new Status(StatusCode.InvalidSchemaId, $"invalid schema id {channel.schemaId}");
        }

        // Write the Schema record
        uncompressedSize_ += Write(output, schemas_[(int)schemaIndex]);
        writtenSchemas_.Add(channel.schemaId);

        // Update schema statistics
        statistics_.schemaCount++;
      }

      // Write the Channel record
      uncompressedSize_ += Write(output, channel);

      // Update channel statistics
      channelMessageCounts.Add(message.channelId, 0);
      statistics_.channelCount++;
    }

    // Before writing a message that would overflow the current chunk, close it.
    var chunkWriter = GetChunkWriter();
    if (chunkWriter != null && /* Chunked? */
        uncompressedSize_ != 0 && /* Current chunk is not empty/new? */
        9 + GetRecordSize(message) + uncompressedSize_ >= chunkSize_ /* Overflowing? */)
    {
      var fileOutput = output_;
      WriteChunk(fileOutput, chunkWriter);
    }

    // For the chunk-local message index.
    var messageOffset = uncompressedSize_;

    // Write the message
    uncompressedSize_ += Write(output, message);

    // Update message statistics
    if (!options_.noSummary)
    {
      if (statistics_.messageCount == 0)
      {
        statistics_.messageStartTime = message.logTime;
        statistics_.messageEndTime = message.logTime;
      } else
      {
        statistics_.messageStartTime = Math.Min(statistics_.messageStartTime, message.logTime);
        statistics_.messageEndTime = Math.Max(statistics_.messageEndTime, message.logTime);
      }
      statistics_.messageCount++;
      channelMessageCounts[message.channelId]++;
    }

    if (chunkWriter != null)
    {
      if (!options_.noMessageIndex)
      {
        // Update the message index
        if (!currentMessageIndex_.ContainsKey(message.channelId))
        {
            currentMessageIndex_[message.channelId] = new MessageIndex();
        }
        var messageIndex = currentMessageIndex_[message.channelId];
        messageIndex.channelId = message.channelId;
        messageIndex.records.Add(new Tuple<ulong, ulong>(message.logTime, messageOffset));
      }

      // Update the chunk index start/end times
      currentChunkStart_ = Math.Min(currentChunkStart_, message.logTime);
      currentChunkEnd_ = Math.Max(currentChunkEnd_, message.logTime);

      // Check if the current chunk is ready to close
      if (uncompressedSize_ >= chunkSize_)
      {
        var fileOutput = output_;
        WriteChunk(fileOutput, chunkWriter);
      }
    }

    return new Status(StatusCode.Success);
  }

  /// <summary>
  /// Writes an attachment to the output stream.
  /// アタッチメントを出力ストリームに書き込みます。
  /// </summary>
  /// <param name="attachment">Attachment to add. 追加するアタッチメント。</param>
  /// <returns>Non-zero error code on failure. 失敗時は非 0。</returns>
  public Status Write(Attachment attachment)
  {
    if (output_ == null)
    {
      return new Status(StatusCode.NotOpen);
    }
    var fileOutput = output_;

    // Check if we have an open chunk that needs to be closed
    var chunkWriter = GetChunkWriter();
    if (chunkWriter != null && !chunkWriter.Empty)
    {
      WriteChunk(fileOutput, chunkWriter);
    }

    if (!options_.noAttachmentCRC)
    {
      // Calculate the CRC32 of the attachment
      uint crc = Crc32.InitialValue;
      crc = Crc32.Update(crc, BitConverter.GetBytes(attachment.logTime), 0);
      crc = Crc32.Update(crc, BitConverter.GetBytes(attachment.createTime), 0);
      crc = Crc32.Update(crc, BitConverter.GetBytes((uint)System.Text.Encoding.UTF8.GetBytes(attachment.name).Length), 0);
      crc = Crc32.Update(crc, System.Text.Encoding.UTF8.GetBytes(attachment.name), 0);
      crc = Crc32.Update(crc, BitConverter.GetBytes((uint)System.Text.Encoding.UTF8.GetBytes(attachment.mediaType).Length), 0);
      crc = Crc32.Update(crc, System.Text.Encoding.UTF8.GetBytes(attachment.mediaType), 0);
      crc = Crc32.Update(crc, BitConverter.GetBytes(attachment.dataSize), 0);
      crc = Crc32.Update(crc, attachment.data.ToArray(), 0);
      attachment.crc = Crc32.Final(crc);
    }

    var fileOffset = fileOutput.Size;

    // Write the attachment
    Write(fileOutput, attachment);

    // Update statistics and attachment index
    if (!options_.noSummary)
    {
      statistics_.attachmentCount++;
      if (!options_.noAttachmentIndex)
      {
        attachmentIndex_.Add(new AttachmentIndex(attachment, fileOffset));
      }
    }

    return new Status(StatusCode.Success);
  }

  /// <summary>
  /// Writes a metadata record to the output stream.
  /// メタデータレコードを書き込みます。
  /// </summary>
  /// <param name="metadata">Metadata to add. 追加するメタデータ。</param>
  /// <returns>Non-zero error code on failure. 失敗時は非 0。</returns>
  public Status Write(Metadata metadata)
  {
    if (output_ == null)
    {
      return new Status(StatusCode.NotOpen);
    }
    var fileOutput = output_;

    // Check if we have an open chunk that needs to be closed
    var chunkWriter = GetChunkWriter();
    if (chunkWriter != null && !chunkWriter.Empty)
    {
      WriteChunk(fileOutput, chunkWriter);
    }

    var fileOffset = fileOutput.Size;

    // Write the metadata
    Write(fileOutput, metadata);

    // Update statistics and metadata index
    if (!options_.noSummary)
    {
      statistics_.metadataCount++;
      if (!options_.noMetadataIndex)
      {
        metadataIndex_.Add(new MetadataIndex(metadata, fileOffset));
      }
    }

    return new Status(StatusCode.Success);
  }

  /// <summary>
  /// Current MCAP file-level statistics (written to Summary as a Statistics record).
  /// 現在のファイル統計（サマリーに Statistics レコードとして出力）。
  /// </summary>
  public Statistics Statistics { get { return statistics_; } }

  /// <summary>
  /// Returns the underlying <see cref="Writable"/> sink, or <c>null</c> if not open.
  /// バックエンドの <see cref="Writable"/>。未オープン時は <c>null</c>。
  /// </summary>
  public Writable? DataSink{get { return output_; } }

  // The following static methods are used for serialization of records and
  // primitives to an output stream. They are not intended to be used directly
  // unless you are implementing a lower level Writer or tests
  // 以下の静的メソッドは、レコードとプリミティブを出力ストリームにシリアル化するために使用されます。これらは、低レベルのライターやテストを実装している場合を除き、直接使用することを意図していません

  public static void WriteMagic(Writable output)
  {
    output.Write(Constants.Magic);
  }

  public static ulong Write(Writable output, Header header)
  {
    // recordSize: profile length (4 bytes) + profile string size + library length (4 bytes) + library string size
    // recordSize: プロファイル長 (4バイト) + プロファイル文字列サイズ + ライブラリ長 (4バイト) + ライブラリ文字列サイズ
    ulong recordSize = 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(header.profile).Length + 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(header.library).Length;

    Write(output, EOpCode.Header);
    Write(output, recordSize);
    Write(output, header.profile);
    Write(output, header.library);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, Footer footer, bool crcEnabled)
  {
    // recordSize: summary_start (8 bytes) + summary_offset_start (8 bytes) + summary_crc (4 bytes)
    // recordSize: summary_start (8バイト) + summary_offset_start (8バイト) + summary_crc (4バイト)
    ulong recordSize = /* summary_start */ 8 +
                              /* summary_offset_start */ 8 +
                              /* summary_crc */ 4;

    Write(output, EOpCode.Footer);
    Write(output, recordSize);
    Write(output, footer.summaryStart);
    Write(output, footer.summaryOffsetStart);
    uint summaryCrc = 0;
    if (crcEnabled)
    {
      summaryCrc = output.Crc;
    }
    Write(output, summaryCrc);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, Schema schema)
  {
    // recordSize: id (2 bytes) + name length (4 bytes) + name string size + encoding length (4 bytes) + encoding string size + data length (4 bytes) + data bytes size
    // recordSize: ID (2バイト) + 名前長 (4バイト) + 名前文字列サイズ + エンコーディング長 (4バイト) + エンコーディング文字列サイズ + データ長 (4バイト) + データバイトサイズ
    ulong calculatedRecordSize = /* id */ 2 +
                              /* name */ 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(schema.name).Length +
                              /* encoding */ 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(schema.encoding).Length +
                              /* data */ 4 + (ulong)schema.data.Count;

    Write(output, EOpCode.Schema);
    Write(output, calculatedRecordSize);
    Write(output, schema.id);
    Write(output, schema.name);
    Write(output, schema.encoding);
    Write(output, schema.data);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + calculatedRecordSize;
  }
  public static ulong Write(Writable output, Channel channel)
  {
    // metadataSize: size of the KeyValueMap (calculated by StaticMethoads.KeyValueMapSize)
    // metadataSize: KeyValueMapのサイズ (StaticMethoads.KeyValueMapSizeで計算)
    ulong metadataSize = McapCs.Util.StaticMethoads.KeyValueMapSize(channel.metadata);
    // recordSize: id (2 bytes) + topic length (4 bytes) + topic string size + message_encoding length (4 bytes) + message_encoding string size + schema_id (2 bytes) + metadata length (4 bytes) + metadata bytes size
    // recordSize: ID (2バイト) + トピック長 (4バイト) + トピック文字列サイズ + メッセージエンコーディング長 (4バイト) + メッセージエンコーディング文字列サイズ + スキーマID (2バイト) + メタデータ長 (4バイト) + メタデータバイトサイズ
    ulong recordSize = /* id */ 2 +
                              /* topic */ 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(channel.topic).Length +
                              /* message_encoding */ 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(channel.messageEncoding).Length +
                              /* schema_id */ 2 +
                              /* metadata */ 4 + metadataSize;

    Write(output, EOpCode.Channel);
    Write(output, recordSize);
    Write(output, channel.id);
    Write(output, channel.schemaId);
    Write(output, channel.topic);
    Write(output, channel.messageEncoding);
    Write(output, (uint)metadataSize);
    Write(output, channel.metadata, metadataSize);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong GetRecordSize(Message message)
  {
    // recordSize: channelId (2 bytes) + sequence (4 bytes) + logTime (8 bytes) + publishTime (8 bytes) + data bytes size
    // recordSize: チャネルID (2バイト) + シーケンス (4バイト) + ログタイム (8バイト) + パブリッシュタイム (8バイト) + データバイトサイズ
    return 2 + 4 + 8 + 8 + (ulong)message.data.Count;
  }
  public static ulong Write(Writable output, Message message)
  {
    ulong recordSize = GetRecordSize(message);

    Write(output, EOpCode.Message);
    Write(output, recordSize);
    Write(output, message.channelId);
    Write(output, message.sequence);
    Write(output, message.logTime);
    Write(output, message.publishTime);
    // 仕様準拠: データ長の4バイトは出力せず、残り全体をデータとして書き込む
    output.Write(message.data.ToArray());

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, Attachment attachment)
  {
    // recordSize: name length (4 bytes) + name string size + logTime (8 bytes) + createTime (8 bytes) + mediaType length (4 bytes) + mediaType string size + dataSize (8 bytes) + data bytes size + crc (4 bytes)
    // recordSize: 名前長 (4バイト) + 名前文字列サイズ + ログタイム (8バイト) + 作成タイム (8バイト) + メディアタイプ長 (4バイト) + メディアタイプ文字列サイズ + データサイズ (8バイト) + データバイトサイズ + CRC (4バイト)
    ulong recordSize = 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(attachment.name).Length + 8 + 8 + 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(attachment.mediaType).Length +
                              8 + attachment.dataSize + 4;

    Write(output, EOpCode.Attachment);
    Write(output, recordSize);
    Write(output, attachment.logTime);
    Write(output, attachment.createTime);
    Write(output, attachment.name);
    Write(output, attachment.mediaType);
    Write(output, attachment.dataSize);
    output.Write(attachment.data.ToArray());
    Write(output, attachment.crc);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, Metadata metadata)
  {
    // metadataSize: size of the KeyValueMap (calculated by StaticMethoads.KeyValueMapSize)
    // metadataSize: KeyValueMapのサイズ (StaticMethoads.KeyValueMapSizeで計算)
    ulong metadataSize = McapCs.Util.StaticMethoads.KeyValueMapSize(metadata.metadata);
    // recordSize: name length (4 bytes) + name string size + metadata length (4 bytes) + metadata bytes size
    // recordSize: 名前長 (4バイト) + 名前文字列サイズ + メタデータ長 (4バイト) + メタデータバイトサイズ
    ulong recordSize = 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(metadata.name).Length + 4 + metadataSize;

    Write(output, EOpCode.Metadata);
    Write(output, recordSize);
    Write(output, metadata.name);
    Write(output, (uint)metadataSize);
    Write(output, metadata.metadata, metadataSize);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, Chunk chunk)
  {
    // recordSize: messageStartTime (8 bytes) + messageEndTime (8 bytes) + uncompressedSize (8 bytes) + uncompressedCrc (4 bytes) + compression length (4 bytes) + compression string size + compressedSize (8 bytes) + compressed data size
    // recordSize: メッセージ開始時刻 (8バイト) + メッセージ終了時刻 (8バイト) + 非圧縮サイズ (8バイト) + 非圧縮CRC (4バイト) + 圧縮長 (4バイト) + 圧縮文字列サイズ + 圧縮サイズ (8バイト) + 圧縮データサイズ
    ulong recordSize = 8 + 8 + 8 + 4 + 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(chunk.compression).Length + 8 + (ulong)chunk.records.Count;

    Write(output, EOpCode.Chunk);
    Write(output, recordSize);
    Write(output, chunk.messageStartTime);
    Write(output, chunk.messageEndTime);
    Write(output, chunk.uncompressedSize);
    Write(output, chunk.uncompressedCrc);
    Write(output, chunk.compression);
    Write(output, chunk.compressedSize);
    output.Write(chunk.records.ToArray());
    output.Flush();

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, MessageIndex index)
  {
    // recordsSize: number of records * (timestamp (8 bytes) + offset (8 bytes))
    // recordsSize: レコード数 * (タイムスタンプ (8バイト) + オフセット (8バイト))
    ulong recordsSize = (ulong)index.records.Count * 16;
    // recordSize: channelId (2 bytes) + recordsSize (4 bytes) + records bytes size
    // recordSize: チャネルID (2バイト) + レコードサイズ (4バイト) + レコードバイトサイズ
    ulong recordSize = 2 + 4 + recordsSize;

    Write(output, EOpCode.MessageIndex);
    Write(output, recordSize);
    Write(output, index.channelId);

    Write(output, (uint)recordsSize);
    foreach (var record in index.records)
    {
      Write(output, record.Item1); // timestamp
      Write(output, record.Item2); // offset
    }

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, ChunkIndex index)
  {
    // messageIndexOffsetsSize: number of message index offsets * (channelId (2 bytes) + offset (8 bytes))
    // messageIndexOffsetsSize: メッセージインデックスオフセットの数 * (チャネルID (2バイト) + オフセット (8バイト))
    ulong messageIndexOffsetsSize = (ulong)index.messageIndexOffsets.Count * 10;
    // recordSize: messageStartTime (8 bytes) + messageEndTime (8 bytes) + chunkStartOffset (8 bytes) + chunkLength (8 bytes) + messageIndexOffsets length (4 bytes) + messageIndexOffsets bytes size + messageIndexLength (8 bytes) + compression length (4 bytes) + compression string size + compressedSize (8 bytes) + uncompressedSize (8 bytes)
    // recordSize: メッセージ開始時刻 (8バイト) + メッセージ終了時刻 (8バイト) + チャンク開始オフセット (8バイト) + チャンク長 (8バイト) + メッセージインデックスオフセット長 (4バイト) + メッセージインデックスオフセットバイトサイズ + メッセージインデックス長 (8バイト) + 圧縮長 (4バイト) + 圧縮文字列サイズ + 圧縮サイズ (8バイト) + 非圧縮サイズ (8バイト)
    ulong recordSize = /* start_time */ 8 +
                              /* end_time */ 8 +
                              /* chunk_start_offset */ 8 +
                              /* chunk_length */ 8 +
                              /* message_index_offsets */ 4 + messageIndexOffsetsSize +
                              /* message_index_length */ 8 +
                              /* compression */ 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(index.compression).Length +
                              /* compressed_size */ 8 +
                              /* uncompressed_size */ 8;

    Write(output, EOpCode.ChunkIndex);
    Write(output, recordSize);
    Write(output, index.messageStartTime);
    Write(output, index.messageEndTime);
    Write(output, index.chunkStartOffset);
    Write(output, index.chunkLength);

    Write(output, (uint)messageIndexOffsetsSize);
    foreach (var entry in index.messageIndexOffsets)
    {
      Write(output, entry.Key);   // channelId
      Write(output, entry.Value); // offset
    }

    Write(output, index.messageIndexLength);
    Write(output, index.compression);
    Write(output, index.compressedSize);
    Write(output, index.uncompressedSize);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, AttachmentIndex index)
  {
    // recordSize: offset (8 bytes) + length (8 bytes) + log_time (8 bytes) + create_time (8 bytes) + data_size (8 bytes) + name length (4 bytes) + name string size + media_type length (4 bytes) + media_type string size
    // recordSize: オフセット (8バイト) + 長さ (8バイト) + ログタイム (8バイト) + 作成タイム (8バイト) + データサイズ (8バイト) + 名前長 (4バイト) + 名前文字列サイズ + メディアタイプ長 (4バイト) + メディアタイプ文字列サイズ
    ulong recordSize = /* offset */ 8 +
                              /* length */ 8 +
                              /* log_time */ 8 +
                              /* create_time */ 8 +
                              /* data_size */ 8 +
                              /* name */ 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(index.name).Length +
                              /* media_type */ 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(index.mediaType).Length;

    Write(output, EOpCode.AttachmentIndex);
    Write(output, recordSize);
    Write(output, index.offset);
    Write(output, index.length);
    Write(output, index.logTime);
    Write(output, index.createTime);
    Write(output, index.dataSize);
    Write(output, index.name);
    Write(output, index.mediaType);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, MetadataIndex index)
  {
    // recordSize: offset (8 bytes) + length (8 bytes) + name length (4 bytes) + name string size
    // recordSize: オフセット (8バイト) + 長さ (8バイト) + 名前長 (4バイト) + 名前文字列サイズ
    ulong recordSize = /* offset */ 8 +
                              /* length */ 8 +
                              /* name */ 4 + (ulong)System.Text.Encoding.UTF8.GetBytes(index.name).Length;

    Write(output, EOpCode.MetadataIndex);
    Write(output, recordSize);
    Write(output, index.offset);
    Write(output, index.length);
    Write(output, index.name);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, Statistics stats)
  {
    // channelMessageCountsSize: number of channel message counts * (channelId (2 bytes) + messageCount (8 bytes))
    // channelMessageCountsSize: チャネルメッセージカウントの数 * (チャネルID (2バイト) + メッセージカウント (8バイト))
    ulong channelMessageCountsSize = (ulong)stats.channelMessageCounts.Count * 10;
    // recordSize: message_count (8 bytes) + schema_count (2 bytes) + channel_count (4 bytes) + attachment_count (4 bytes) + metadata_count (4 bytes) + chunk_count (4 bytes) + message_start_time (8 bytes) + message_end_time (8 bytes) + channel_message_counts length (4 bytes) + channel_message_counts bytes size
    // recordSize: メッセージカウント (8バイト) + スキーマカウント (2バイト) + チャネルカウント (4バイト) + アタッチメントカウント (4バイト) + メタデータカウント (4バイト) + チャンクカウント (4バイト) + メッセージ開始時刻 (8バイト) + メッセージ終了時刻 (8バイト) + チャネルメッセージカウント長 (4バイト) + チャネルメッセージカウントバイトサイズ
    ulong recordSize = /* message_count */ 8 +
                              /* schema_count */ 2 +
                              /* channel_count */ 4 +
                              /* attachment_count */ 4 +
                              /* metadata_count */ 4 +
                              /* chunk_count */ 4 +
                              /* message_start_time */ 8 +
                              /* message_end_time */ 8 +
                              /* channel_message_counts */ 4 + channelMessageCountsSize;

    Write(output, EOpCode.Statistics);
    Write(output, recordSize);
    Write(output, stats.messageCount);
    Write(output, stats.schemaCount);
    Write(output, stats.channelCount);
    Write(output, stats.attachmentCount);
    Write(output, stats.metadataCount);
    Write(output, stats.chunkCount);
    Write(output, stats.messageStartTime);
    Write(output, stats.messageEndTime);

    Write(output, (uint)channelMessageCountsSize);
    foreach (var entry in stats.channelMessageCounts)
    {
      Write(output, entry.Key);   // channelId
      Write(output, entry.Value); // messageCount
    }

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, SummaryOffset summaryOffset)
  {
    // recordSize: group_opcode (1 byte) + group_start (8 bytes) + group_length (8 bytes)
    // recordSize: グループオペコード (1バイト) + グループ開始 (8バイト) + グループ長 (8バイト)
    ulong recordSize = /* group_opcode */ 1 +
                              /* group_start */ 8 +
                              /* group_length */ 8;

    Write(output, EOpCode.SummaryOffset);
    Write(output, recordSize);
    Write(output, summaryOffset.groupOpCode);
    Write(output, summaryOffset.groupStart);
    Write(output, summaryOffset.groupLength);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, DataEnd dataEnd)
  {
    // recordSize: data_section_crc (4 bytes)
    // recordSize: データセクションCRC (4バイト)
    ulong recordSize = /* data_section_crc */ 4;

    Write(output, EOpCode.DataEnd);
    Write(output, recordSize);
    Write(output, dataEnd.dataSectionCrc);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    // 9: オペコード (1バイト) + レコード長 (8バイト)
    return 9 + recordSize;
  }
  public static ulong Write(Writable output, Record.Record record)
  {
#if DEBUG
    Console.WriteLine($"Writing record: OpCode={record.opcode}, DataSize={record.dataSize}, CurrentFileSize={output.Size}");
#endif
    Write(output, record.opcode);
    Write(output, record.dataSize);
    Write(output, record.data, record.dataSize);

    // 9: OpCode (1 byte) + Record Length (8 bytes)
    return 9 + record.dataSize;
  }

  public static void Write(Writable output, string str)
  {
    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
    Write(output, (uint)bytes.Length);
    output.Write(bytes);
  }
  public static void Write(Writable output, List<byte> bytes)
  {
    Write(output, (uint)bytes.Count);
    output.Write(bytes.ToArray());
  }
  public static void Write(Writable output, EOpCode value)
  {
    output.Write(new byte[] { (byte)value });
  }
  public static void Write(Writable output, ushort value)
  {
    output.Write(BitConverter.GetBytes(value));
  }
  public static void Write(Writable output, uint value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    if (BitConverter.IsLittleEndian)
    {
      // 何もしない
    }
    else
    {
      Array.Reverse(bytes);
    }
    Console.WriteLine($"Writing uint {value}: {BitConverter.ToString(bytes)}");
    output.Write(bytes);
  }
  public static void Write(Writable output, ulong value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    if (BitConverter.IsLittleEndian)
    {
      // 何もしない
    }
    else
    {
      Array.Reverse(bytes);
    }
    Console.WriteLine($"Writing ulong {value}: {BitConverter.ToString(bytes)}");
    output.Write(bytes);
  }
  public static void Write(Writable output, byte[] data, ulong size)
  {
    output.Write(data);
  }
  public static void Write(Writable output, Dictionary<string, string> map, ulong size)
  {
    // Create a list of key-value pairs so we can lexicographically sort by key
    // キーで辞書順にソートできるように、キーと値のペアのリストを作成します
    var pairs = new List<KeyValuePair<string, string>>(map);
    pairs.Sort((a, b) => string.CompareOrdinal(a.Key, b.Key));

    
    foreach (var entry in pairs)
    {
      Write(output, entry.Key);
      Write(output, entry.Value);
    }
  }


  McapWriterOptions options_;
  ulong chunkSize_ = Constants.DefaultChunkSize;
  Writable? output_ = null;
  FileWriter? fileOutput_ = null;
  StreamWriter? streamOutput_ = null;
  BufferWriter? uncompressedChunk_ = null;

  List<Schema> schemas_ = new List<Schema>();
  List<Channel> channels_ = new List<Channel>();
  List<AttachmentIndex> attachmentIndex_ = new List<AttachmentIndex>();
  List<MetadataIndex> metadataIndex_ = new List<MetadataIndex>();
  List<ChunkIndex> chunkIndex_ = new List<ChunkIndex>();
  Statistics statistics_ = new Statistics();
  HashSet<ushort> writtenSchemas_ = new HashSet<ushort>();
  Dictionary<ushort, MessageIndex> currentMessageIndex_ = new Dictionary<ushort, MessageIndex>();
  ulong currentChunkStart_ = Constants.MaxTime;
  ulong currentChunkEnd_ = 0;
  Compression compression_ = Compression.None;
  ulong uncompressedSize_ = 0;
  bool opened_ = false;

  public McapWriter()
  {
    options_ = new McapWriterOptions("default", $"libmcap {Constants.MCAP_LIBRARY_VERSION}"); // Default profile and library
    fileOutput_ = null;
    streamOutput_ = null;
    uncompressedChunk_ = null;

  }

  /// <summary>
  /// Returns the current write target depending on chunking/compression settings.
  /// チャンク/圧縮設定に応じた現在の書き込み先を返します。
  /// </summary>
  /// <returns>Writable sink for record emission. レコード出力先。</returns>
  /// <exception cref="InvalidOperationException">Writer is not open, or chunk buffer missing. ライター未オープン、またはチャンクバッファ未初期化。</exception>
  private Writable GetOutput()
  {
    if (output_ == null)
    {
      throw new InvalidOperationException("Writer is not open.");
    }
    if (chunkSize_ == 0)
    {
      return output_;
    }
    switch (compression_)
    {
      case Compression.None:
      default:
        if (uncompressedChunk_ == null)
        {
          throw new InvalidOperationException("Chunking is enabled but uncompressedChunk_ is null.");
        }
        else
        {
          return uncompressedChunk_;
        }

      case Compression.Lz4:
        // 蓄積は BufferWriter。圧縮は WriteChunk() 側で実施します。
        if (uncompressedChunk_ == null)
        {
          uncompressedChunk_ = new BufferWriter();
          uncompressedChunk_.CrcEnabled = !options_.noChunkCRC;
        }
        return uncompressedChunk_;
      case Compression.Zstd:
        // 蓄積は BufferWriter。圧縮は WriteChunk() 側で実施します。
        if (uncompressedChunk_ == null)
        {
          uncompressedChunk_ = new BufferWriter();
          uncompressedChunk_.CrcEnabled = !options_.noChunkCRC;
        }
        return uncompressedChunk_;
    }
  }
  // チャンクへの書き込み先（蓄積用）を取得します。
  // 圧縮の有無に関わらず、レコードは一旦非圧縮バッファ（BufferWriter）に蓄積し、
  // チャンクを閉じる段階（WriteChunk）で実際の圧縮（lz4/zstd）を行います。
  // こうすることで「サイズ閾値・圧縮率」の判定後に、圧縮の採否を決められます。
  /// <summary>
  /// Returns the accumulation writer for the current chunk.
  /// 現在のチャンク用の蓄積ライター（非圧縮バッファ）を返します。
  /// </summary>
  /// <returns>Chunk writer used to buffer records. レコード蓄積用チャンクライター。</returns>
  /// <exception cref="InvalidOperationException">Chunking is disabled or buffer missing. チャンク無効またはバッファ未初期化。</exception>
  private ChunkWriter GetChunkWriter()
  {
    if (chunkSize_ == 0)
    {
      throw new InvalidOperationException("Chunking is not enabled.");
    }

    // 圧縮モードでもここでは BufferWriter を返す点に注意：
    // - 書き込み時点では非圧縮で蓄積し、uncompressed CRC を計算
    // - WriteChunk 内で圧縮を実施し、圧縮の採否と compression 文字列（"lz4"/"zstd"/"none"）を確定
    switch (compression_)
    {
      case Compression.None:
      default:
        if (uncompressedChunk_ == null)
        {
          throw new InvalidOperationException("Chunking is enabled but uncompressedChunk_ is null.");
        }
        else
        {
          return uncompressedChunk_;
        }
      case Compression.Lz4:
        // 蓄積中は BufferWriter を使用。実際の圧縮は WriteChunk() で一括実行します。
        if (uncompressedChunk_ == null)
        {
          uncompressedChunk_ = new BufferWriter();
          uncompressedChunk_.CrcEnabled = !options_.noChunkCRC;
        }
        return uncompressedChunk_;
      case Compression.Zstd:
        // 蓄積中は BufferWriter を使用。実際の圧縮は WriteChunk() で一括実行します。
        if (uncompressedChunk_ == null)
        {
          uncompressedChunk_ = new BufferWriter();
          uncompressedChunk_.CrcEnabled = !options_.noChunkCRC;
        }
        return uncompressedChunk_;
    }
  }
  // チャンクを確定して出力します。
  // ここで初めて圧縮の実行と採否判定（lz4/zstd/none）を行います。
  // 入力 chunkData は、GetChunkWriter() で返した BufferWriter（非圧縮蓄積）です。
  public void WriteChunk(Writable output, ChunkWriter chunkData)
  {
    // 圧縮実施のしきい値と採用条件
    // - LZ4/ZSTDともに、非常に小さいデータは圧縮効率が悪いため約1KB以上を目安とします
    // - 圧縮後サイズが原サイズの98%未満（= 2%以上縮小）でなければ、非圧縮として書き出します
    // Both LZ4 and ZSTD recommend ~1KB as the minimum size for compressed data
    // LZ4とZSTDはどちらも、圧縮データの最小サイズとして約1KBを推奨しています
    const ulong MIN_COMPRESSION_SIZE = 1024;
    // Throw away any compression results that save less than 2% of the original size
    // 元のサイズの2%未満しか節約できない圧縮結果は破棄します
    const double MIN_COMPRESSION_RATIO = 1.02;

    Compression compression = Compression.None;
    var uncompressedSize = uncompressedSize_;
    ulong compressedSize = uncompressedSize;
    byte[] compressedData = chunkData.Data;

    // 閾値（サイズまたは強制）を満たした場合のみ、圧縮を試行
    if (options_.forceCompression || uncompressedSize >= MIN_COMPRESSION_SIZE)
    {
      // Flush any in-progress compression stream
      // 進行中の圧縮ストリームをフラッシュします
      chunkData.End();

      // 実際に圧縮した結果サイズと比較して、採否を決める
      ulong candidateSize = uncompressedSize;
      byte[] candidateData = chunkData.Data;
      Compression candidateAlgo = Compression.None;
      // 選択アルゴリズムで圧縮を一度実施
      switch (compression_)
      {
        case Compression.Lz4:
        {
          var lz4Writer = new Lz4ChunkWriter(MapLz4Level(options_.compressionLevel));
          lz4Writer.CrcEnabled = chunkData.CrcEnabled;
          lz4Writer.Write(chunkData.Data);
          lz4Writer.End();
          candidateAlgo = Compression.Lz4;
          candidateSize = lz4Writer.CompressedSize;
          candidateData = lz4Writer.CompressedData;
          break;
        }
        case Compression.Zstd:
        {
          var zstdWriter = new ZstdChunkWriter(MapZstdLevel(options_.compressionLevel));
          zstdWriter.CrcEnabled = chunkData.CrcEnabled;
          zstdWriter.Write(chunkData.Data);
          zstdWriter.End();
          candidateAlgo = Compression.Zstd;
          candidateSize = zstdWriter.CompressedSize;
          candidateData = zstdWriter.CompressedData;
          break;
        }
        case Compression.None:
        default:
          break;
      }

      var compressionRatio = candidateSize == 0 ? double.PositiveInfinity : (double)uncompressedSize / (double)candidateSize;
      if (options_.forceCompression || compressionRatio >= MIN_COMPRESSION_RATIO)
      {
        compression = candidateAlgo;
        compressedSize = candidateSize;
        compressedData = candidateData;
      }
    }

    // 採用結果に応じてヘッダの compression を設定（"lz4"/"zstd"/"none"）
    var compressionStr = compression.ToString().ToLower(); // Assuming Compression enum values match string representation
    var uncompressedCrc = chunkData.Crc;

    // Write the chunk
    // チャンクを書き込みます
    var chunkStartOffset = output.Size;
    Write(output, new Chunk { messageStartTime = currentChunkStart_, messageEndTime = currentChunkEnd_, uncompressedSize = uncompressedSize, uncompressedCrc = uncompressedCrc, compression = compressionStr, compressedSize = compressedSize, records = compressedData.ToList() });

    var chunkLength = output.Size - chunkStartOffset;

    if (!options_.noChunkIndex)
    {
      // Create a chunk index record
      // チャンクインデックスレコードを作成します
      var chunkIndexRecord = new ChunkIndex();

      var messageIndexOffset = output.Size;
      if (!options_.noMessageIndex)
      {
        // Write the message index records
        // メッセージインデックスレコードを書き込みます
        foreach (var entry in currentMessageIndex_)
        {
          var channelId = entry.Key;
          var messageIndex = entry.Value;
          // currentMessageIndex_ contains entries for every channel ever seen, not just in this
          // chunk. Only write message index records for channels with messages in this chunk.
          // currentMessageIndex_ には、このチャンクだけでなく、これまでに表示されたすべてのチャネルのエントリが含まれています。
          // このチャンク内のメッセージを持つチャネルのメッセージインデックスレコードのみを書き込みます。
          if (messageIndex.records.Count > 0)
          {
            chunkIndexRecord.messageIndexOffsets.Add(channelId, output.Size);
            Write(output, messageIndex);
            // reset this message index for the next chunk. This allows us to re-use
            // allocations vs. the alternative strategy of allocating a fresh set of MessageIndex
            // objects per chunk.
            // 次のチャンクのためにこのメッセージインデックスをリセットします。これにより、
            // チャンクごとに新しいMessageIndexオブジェクトのセットを割り当てる代替戦略と比較して、
            // 割り当てを再利用できます。
            messageIndex.records.Clear();
          }
        }
      }
      var messageIndexLength = output.Size - messageIndexOffset;

      // Fill in the newly created chunk index record. This will be written into
      // the summary section when close() is called. Note that currentChunkStart_
      // may still be initialized to MaxTime if this chunk does not contain any
      // messages.
      // 新しく作成されたチャンクインデックスレコードを埋めます。これは、close()が呼び出されたときに
      // サマリーセクションに書き込まれます。このチャンクにメッセージが含まれていない場合、
      // currentChunkStart_ はまだMaxTimeに初期化されている可能性があることに注意してください。
      chunkIndexRecord.messageStartTime = currentChunkStart_ == Constants.MaxTime ? 0 : currentChunkStart_;
      chunkIndexRecord.messageEndTime = currentChunkEnd_;
      chunkIndexRecord.chunkStartOffset = chunkStartOffset;
      chunkIndexRecord.chunkLength = chunkLength;
      chunkIndexRecord.messageIndexLength = messageIndexLength;
      chunkIndexRecord.compression = compressionStr;
      chunkIndexRecord.compressedSize = compressedSize;
      chunkIndexRecord.uncompressedSize = uncompressedSize;
      chunkIndex_.Add(chunkIndexRecord);
    } else if (!options_.noMessageIndex)
    {
      // Write the message index records
      // メッセージインデックスレコードを書き込みます
      foreach (var entry in currentMessageIndex_)
      {
        var channelId = entry.Key;
        var messageIndex = entry.Value;
        // currentMessageIndex_ contains entries for every channel ever seen, not just in this
        // chunk. Only write message index records for channels with messages in this chunk.
        // currentMessageIndex_ には、このチャンクだけでなく、これまでに表示されたすべてのチャネルのエントリが含まれています。
        // このチャンク内のメッセージを持つチャネルのメッセージインデックスレコードのみを書き込みます。
        if (messageIndex.records.Count > 0)
        {
          Write(output, messageIndex);
          // reset this message index for the next chunk. This allows us to re-use
          // allocations vs. the alternative strategy of allocating a fresh set of MessageIndex
          // objects per chunk.
          // 次のチャンクのためにこのメッセージインデックスをリセットします。これにより、
          // チャンクごとに新しいMessageIndexオブジェクトのセットを割り当てる代替戦略と比較して、
          // 割り当てを再利用できます。
          messageIndex.records.Clear();
        }
      }
    }

    // Reset uncompressedSize and start/end times for the next chunk
    // 非圧縮サイズと次のチャンクの開始/終了時間をリセットします
    uncompressedSize_ = 0;
    currentChunkStart_ = Constants.MaxTime;
    currentChunkEnd_ = 0;

    // Update statistics
    // 統計情報を更新します
    statistics_.chunkCount++;

    // Reset the chunk writer
    // チャンクライターをリセットします
    chunkData.Clear();
  }
  /// <summary>
  /// Maps <see cref="CompressionLevel"/> to LZ4 level.
  /// <see cref="CompressionLevel"/> を LZ4 のレベルに対応付けます。
  /// </summary>
  /// <param name="level">Requested compression level. 要求レベル。</param>
  /// <returns>LZ4 level enum. LZ4 レベル。</returns>
  private static K4os.Compression.LZ4.LZ4Level MapLz4Level(CompressionLevel level)
  {
    return level switch
    {
      CompressionLevel.Fastest => K4os.Compression.LZ4.LZ4Level.L00_FAST,
      CompressionLevel.Fast => K4os.Compression.LZ4.LZ4Level.L03_HC,
      CompressionLevel.Default => K4os.Compression.LZ4.LZ4Level.L06_HC,
      CompressionLevel.Slow => K4os.Compression.LZ4.LZ4Level.L09_HC,
      CompressionLevel.Slowest => K4os.Compression.LZ4.LZ4Level.L12_MAX,
      _ => K4os.Compression.LZ4.LZ4Level.L06_HC,
    };
  }

  /// <summary>
  /// Maps <see cref="CompressionLevel"/> to Zstd numeric level.
  /// <see cref="CompressionLevel"/> を Zstd の数値レベルに対応付けます。
  /// </summary>
  /// <param name="level">Requested compression level. 要求レベル。</param>
  /// <returns>Zstd level as integer. Zstd レベル（整数）。</returns>
  private static int MapZstdLevel(CompressionLevel level)
  {
    return level switch
    {
      CompressionLevel.Fastest => 1,
      CompressionLevel.Fast => 3,
      CompressionLevel.Default => 5,
      CompressionLevel.Slow => 10,
      CompressionLevel.Slowest => 19,
      _ => 5,
    };
  }
};
