# MessageIndex

`MessageIndex` 構造体は、単一のチャンネルに対するタイムスタンプとバイトオフセットのリストです。各チャンクの後に、そのチャンクに出現したチャンネルごとに1つずつこのレコードが出現します。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC MessageIndex {
  ChannelId channelId;
  std::vector<std::pair<Timestamp, ByteOffset>> records;
};
```

**メンバー:**

*   `channelId`: メッセージインデックスが関連付けられているチャンネルのID。
*   `records`: タイムスタンプとバイトオフセットのペアのリスト。各ペアは、チャンク内のメッセージのログタイムスタンプと、チャンクのレコードデータ先頭からのメッセージのオフセットを示します。
