# McapReader

`McapReader` は、MCAPファイルへの読み取りインターフェースを提供する主要なクラスです。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
class MCAP_PUBLIC McapReader final {
public:
  ~McapReader();

  Status open(IReadable& reader);
  Status open(std::string_view filename);
  Status open(std::ifstream& stream);

  void close();

  Status readSummary(ReadSummaryMethod method, const ProblemCallback& onProblem = [](const Status&) {});

  LinearMessageView readMessages(Timestamp startTime = 0, Timestamp endTime = MaxTime);
  LinearMessageView readMessages(const ProblemCallback& onProblem, Timestamp startTime = 0, Timestamp endTime = MaxTime);
  LinearMessageView readMessages(const ProblemCallback& onProblem, const ReadMessageOptions& options);

  std::pair<ByteOffset, ByteOffset> byteRange(Timestamp startTime, Timestamp endTime = MaxTime) const;

  IReadable* dataSource();

  const std::optional<Header>& header() const;
  const std::optional<Footer>& footer() const;
  const std::optional<Statistics>& statistics() const;

  const std::unordered_map<ChannelId, ChannelPtr> channels() const;
  const std::unordered_map<SchemaId, SchemaPtr> schemas() const;

  ChannelPtr channel(ChannelId channelId) const;
  SchemaPtr schema(SchemaId schemaId) const;

  const std::vector<ChunkIndex>& chunkIndexes() const;
  const std::multimap<std::string, MetadataIndex>& metadataIndexes() const;
  const std::multimap<std::string, AttachmentIndex>& attachmentIndexes() const;

  // Static parsing methods...
};
```

**メンバー:**

*   `open(...)`: MCAPファイルを読み込み用に開きます。`IReadable`、ファイル名、`std::ifstream` のオーバーロードがあります。
*   `close()`: MCAPファイルを閉じ、内部の状態をクリアします。
*   `readSummary(...)`: ファイルのサマリーセクションを読み込み、解析します。これにより、インデックスが構築され、効率的なランダムアクセスが可能になります。
*   `readMessages(...)`: メッセージをイテレートするための `LinearMessageView` を返します。時間範囲やトピックによるフィルタリングが可能です。
*   `byteRange(...)`: 指定された時間範囲のメッセージを読み込むために必要なバイトオフセットの範囲を返します。
*   `dataSource()`: このリーダーが使用している `IReadable` データソースへのポインタを返します。
*   `header()`, `footer()`, `statistics()`: ファイルのヘッダー、フッター、統計情報（利用可能な場合）を返します。
*   `channels()`, `schemas()`: ファイルに登録されている全てのチャンネルとスキーマを返します。
*   `channel(ChannelId)`, `schema(SchemaId)`: 指定されたIDのチャンネルまたはスキーマを検索します。
*   `chunkIndexes()`, `metadataIndexes()`, `attachmentIndexes()`: 対応するインデックス情報を返します。

**静的メソッド:**

`McapReader` は、`Record` から特定のレコードタイプ (`Header`, `Footer`, `Schema` など) を解析するための多数の静的 `Parse...` メソッドを提供します。
