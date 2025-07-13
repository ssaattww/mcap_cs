# McapWriter

`McapWriter` は、MCAPファイルへの書き込みインターフェースを提供する主要なクラスです。

**定義:** `cpp/mcap/include/mcap/writer.hpp`

```cpp
class MCAP_PUBLIC McapWriter final {
public:
  ~McapWriter();

  Status open(std::string_view filename, const McapWriterOptions& options);
  void open(IWritable& writer, const McapWriterOptions& options);
  void open(std::ostream& stream, const McapWriterOptions& options);

  void close();
  void terminate();

  void addSchema(Schema& schema);
  void addChannel(Channel& channel);

  Status write(const Message& message);
  Status write(Attachment& attachment);
  Status write(const Metadata& metadata);

  const Statistics& statistics() const;
  IWritable* dataSink();
  void closeLastChunk();

  // Static serialization methods...
};
```

**メンバー:**

*   `open(...)`: 新しいMCAPファイルの書き込みを開始し、ヘッダーを書き込みます。ファイル名、`IWritable`、`std::ostream` のオーバーロードがあります。
*   `close()`: MCAPフッターを書き込み、保留中の書き込みをフラッシュし、内部状態をリセットします。
*   `terminate()`: フッターを書き込まずに内部状態をリセットします。エラー時に使用されます。
*   `addSchema(Schema& schema)`: 新しいスキーマをMCAPファイルに追加し、`schema.id` を生成されたIDに設定します。
*   `addChannel(Channel& channel)`: 新しいチャンネルをMCAPファイルに追加し、`channel.id` を生成されたIDに設定します。
*   `write(const Message& message)`: メッセージを出力ストリームに書き込みます。
*   `write(Attachment& attachment)`: アタッチメントを出力ストリームに書き込みます。
*   `write(const Metadata& metadata)`: メタデータレコードを出力ストリームに書き込みます。
*   `statistics() const`: 現在のMCAPファイルの統計情報を返します。
*   `dataSink()`: このライターが使用している `IWritable` データデスティネーションへのポインタを返します。
*   `closeLastChunk()`: 現在進行中のチャンクを終了し、ファイルに書き込みます。

**静的メソッド:**

`McapWriter` は、各種MCAPレコード (`Header`, `Footer`, `Schema`, `Channel`, `Message` など) やプリミティブ型 (`uint16_t`, `uint32_t`, `uint64_t`, `std::string_view` など) を `IWritable` にシリアライズするための多数の静的 `write` メソッドを提供します。これらは通常、ライブラリの内部で使用されます。
