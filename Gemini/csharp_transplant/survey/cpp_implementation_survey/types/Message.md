# Message

`Message` 構造体は、チャンネルにパブリッシュされた単一のメッセージを表します。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC Message {
  ChannelId channelId;
  uint32_t sequence;
  Timestamp logTime;
  Timestamp publishTime;
  uint64_t dataSize;
  const std::byte* data = nullptr;
};
```

**メンバー:**

*   `channelId`: メッセージが属するチャンネルのID。
*   `sequence`: オプションのシーケンス番号。非ゼロの場合、チャンネルごとに一意で時間とともに増加するべきです。
*   `logTime`: メッセージが記録または記録のために受信されたナノ秒単位のタイムスタンプ。
*   `publishTime`: メッセージが最初にパブリッシュされたナノ秒単位のタイムスタンプ。利用できない場合は `logTime` と同じに設定されるべきです。
*   `dataSize`: メッセージペイロードのサイズ（バイト単位）。
*   `data`: メッセージペイロードへのポインタ。リーダーの場合、このポインタは `onMessage` コールバックのライフタイム中、またはメッセージイテレータが進むまでのみ有効です。
