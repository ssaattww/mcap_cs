# TypedRecordReader

`TypedRecordReader` は、データソースからMCAPレコードを解析し、検証するためのミッドレベルインターフェースです。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
struct MCAP_PUBLIC TypedRecordReader {
  std::function<void(const Header&, ByteOffset)> onHeader;
  std::function<void(const Footer&, ByteOffset)> onFooter;
  std::function<void(const SchemaPtr, ByteOffset, std::optional<ByteOffset>)> onSchema;
  std::function<void(const ChannelPtr, ByteOffset, std::optional<ByteOffset>)> onChannel;
  std::function<void(const Message&, ByteOffset, std::optional<ByteOffset>)> onMessage;
  std::function<void(const Chunk&, ByteOffset)> onChunk;
  std::function<void(const MessageIndex&, ByteOffset)> onMessageIndex;
  std::function<void(const ChunkIndex&, ByteOffset)> onChunkIndex;
  std::function<void(const Attachment&, ByteOffset)> onAttachment;
  std::function<void(const AttachmentIndex&, ByteOffset)> onAttachmentIndex;
  std::function<void(const Statistics&, ByteOffset)> onStatistics;
  std::function<void(const Metadata&, ByteOffset)> onMetadata;
  std::function<void(const MetadataIndex&, ByteOffset)> onMetadataIndex;
  std::function<void(const SummaryOffset&, ByteOffset)> onSummaryOffset;
  std::function<void(const DataEnd&, ByteOffset)> onDataEnd;
  std::function<void(const Record&, ByteOffset, std::optional<ByteOffset>)> onUnknownRecord;
  std::function<void(ByteOffset)> onChunkEnd;

  TypedRecordReader(IReadable& dataSource, ByteOffset startOffset,
                    ByteOffset endOffset = EndOffset);

  bool next();

  ByteOffset offset() const;

  const Status& status() const;

private:
  RecordReader reader_;
  TypedChunkReader chunkReader_;
  Status status_;
  bool parsingChunk_;
};
```

**メンバー:**

*   `on...` コールバック群: `Header`, `Footer`, `Schema`, `Channel`, `Message`, `Chunk` など、それぞれのMCAPレコードタイプが読み込まれたときに呼び出されるコールバック関数です。
*   `TypedRecordReader(...)`: データソース、開始オフセット、終了オフセットで初期化するコンストラクタ。
*   `next()`: 次のレコードを読み込み、解析し、対応するコールバックを呼び出します。チャンクレコードを見つけた場合は、内部の `TypedChunkReader` を使用してチャンク内のレコードを処理します。
*   `offset() const`: データソース内の現在のオフセットを返します。
*   `status() const`: リーダーの現在の状態を返します。
