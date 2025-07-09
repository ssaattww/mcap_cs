# Statistics

`Statistics` 構造体は、サマリーセクションに存在し、ファイル全体のカウントとタイムスタンプ範囲を提供します。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC Statistics {
  uint64_t messageCount;
  uint16_t schemaCount;
  uint32_t channelCount;
  uint32_t attachmentCount;
  uint32_t metadataCount;
  uint32_t chunkCount;
  Timestamp messageStartTime;
  Timestamp messageEndTime;
  std::unordered_map<ChannelId, uint64_t> channelMessageCounts;
};
```

**メンバー:**

*   `messageCount`: ファイル内の総メッセージ数。
*   `schemaCount`: ファイル内のスキーマの総数。
*   `channelCount`: ファイル内のチャンネルの総数。
*   `attachmentCount`: ファイル内のアタッチメントの総数。
*   `metadataCount`: ファイル内のメタデータの総数。
*   `chunkCount`: ファイル内のチャンクの総数。
*   `messageStartTime`: ファイル内のメッセージの最小ログタイムスタンプ。
*   `messageEndTime`: ファイル内のメッセージの最大ログタイムスタンプ。
*   `channelMessageCounts`: チャンネルIDごとのメッセージ数のマップ。
