# RecordReader

`RecordReader` は、データソースからMCAP形式のTLV (Type-Length-Value) レコードを解析するための低レベルインターフェースです。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
struct MCAP_PUBLIC RecordReader {
  ByteOffset offset;
  ByteOffset endOffset;

  RecordReader(IReadable& dataSource, ByteOffset startOffset, ByteOffset endOffset = EndOffset);

  void reset(IReadable& dataSource, ByteOffset startOffset, ByteOffset endOffset);

  std::optional<Record> next();

  const Status& status() const;

  ByteOffset curRecordOffset() const;

private:
  IReadable* dataSource_ = nullptr;
  Status status_;
  Record curRecord_;
};
```

**メンバー:**

*   `offset`: 現在の読み取りオフセット。
*   `endOffset`: 読み取りの終了オフセット。
*   `RecordReader(...)`: データソース、開始オフセット、終了オフセットで初期化するコンストラクタ。
*   `reset(...)`: 新しいデータソースとオフセットでリーダーをリセットします。
*   `next()`: 次のレコードを読み込み、`std::optional<Record>` として返します。終端に達したかエラーが発生した場合は `std::nullopt` を返します。
*   `status() const`: リーダーの現在の状態を返します。
*   `curRecordOffset() const`: 現在のレコードの開始オフセットを返します。
