# TypedChunkReader

`TypedChunkReader` は、チャンク内のレコードを型付けされたコールバックを介して処理するためのインターフェースです。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
struct MCAP_PUBLIC TypedChunkReader {
  std::function<void(const SchemaPtr, ByteOffset)> onSchema;
  std::function<void(const ChannelPtr, ByteOffset)> onChannel;
  std::function<void(const Message&, ByteOffset)> onMessage;
  std::function<void(const Record&, ByteOffset)> onUnknownRecord;

  TypedChunkReader();
  TypedChunkReader(const TypedChunkReader&) = delete;
  TypedChunkReader& operator=(const TypedChunkReader&) = delete;
  TypedChunkReader(TypedChunkReader&&) = delete;
  TypedChunkReader& operator=(TypedChunkReader&&) = delete;

  void reset(const Chunk& chunk, Compression compression);

  bool next();

  ByteOffset offset() const;

  const Status& status() const;

private:
  RecordReader reader_;
  Status status_;
  BufferReader uncompressedReader_;
#ifndef MCAP_COMPRESSION_NO_LZ4
  LZ4Reader lz4Reader_;
#endif
#ifndef MCAP_COMPRESSION_NO_ZSTD
  ZStdReader zstdReader_;
#endif
};
```

**メンバー:**

*   `onSchema`: `Schema` レコードが読み込まれたときに呼び出されるコールバック。
*   `onChannel`: `Channel` レコードが読み込まれたときに呼び出されるコールバック。
*   `onMessage`: `Message` レコードが読み込まれたときに呼び出されるコールバック。
*   `onUnknownRecord`: 不明なレコードが読み込まれたときに呼び出されるコールバック。
*   `reset(const Chunk& chunk, Compression compression)`: 新しいチャンクと圧縮タイプでリーダーをリセットします。内部で圧縮を解除し、`RecordReader` を初期化します。
*   `next()`: 次のレコードを処理し、対応するコールバックを呼び出します。
*   `offset() const`: チャンク内の現在のオフセットを返します。
*   `status() const`: リーダーの現在の状態を返します。
