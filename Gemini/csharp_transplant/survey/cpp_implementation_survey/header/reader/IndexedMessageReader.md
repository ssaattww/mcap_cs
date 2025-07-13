# IndexedMessageReader

`IndexedMessageReader` は、メッセージインデックスを使用して、MCAPからログ時刻順にメッセージを読み取ります。このリーダーを使用するには、MCAPファイルがチャンク化され、サマリーセクションとメッセージインデックスを持っている必要があります。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
struct MCAP_PUBLIC IndexedMessageReader {
public:
  IndexedMessageReader(McapReader& reader, const ReadMessageOptions& options,
                       const std::function<void(const Message&, RecordOffset)> onMessage);

  bool next();

  Status status() const;

private:
  struct ChunkSlot {
    ByteArray decompressedChunk;
    ByteOffset chunkStartOffset;
    int unreadMessages = 0;
  };
  size_t findFreeChunkSlot();
  void decompressChunk(const Chunk& chunk, ChunkSlot& slot);
  Status status_;
  McapReader& mcapReader_;
  RecordReader recordReader_;
#ifndef MCAP_COMPRESSION_NO_LZ4
  LZ4Reader lz4Reader_;
#endif
  ReadMessageOptions options_;
  std::unordered_set<ChannelId> selectedChannels_;
  std::function<void(const Message&, RecordOffset)> onMessage_;
  internal::ReadJobQueue queue_;
  std::vector<ChunkSlot> chunkSlots_;
};
```

**メンバー:**

*   `IndexedMessageReader(...)`: `McapReader`、読み取りオプション、メッセージコールバックで初期化するコンストラクタ。
*   `next()`: 次のメッセージを読み込みます。メッセージが見つかった場合は `true` を返します。
*   `status() const`: リーダーの現在の状態を返します。

**内部動作:**

このリーダーは、`ReadJobQueue` を使用して、チャンクの伸長とメッセージの読み取りを並行して行います。`ChunkSlot` 構造体は、伸長されたチャンクデータを保持するためのスロットとして機能します。
