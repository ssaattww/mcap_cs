# Channel

`Channel` 構造体は、メッセージが書き込まれるチャンネルを記述します。各トピックはパブリッシャーごとに1つのチャンネルを持ちます。チャンネルはオプションでスキーマを参照します。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC Channel {
  ChannelId id;
  std::string topic;
  std::string messageEncoding;
  SchemaId schemaId;
  KeyValueMap metadata;

  Channel() = default;Gemini/csharp_transplant/survey/cpp_implementation_survey

  Channel(const std::string_view topic, const std::string_view messageEncoding, SchemaId schemaId,
          const KeyValueMap& metadata = {})
      : topic(topic)
      , messageEncoding(messageEncoding)
      , schemaId(schemaId)
      , metadata(metadata) {}
};
```

**メンバー:**

*   `id`: チャンネルの一意な識別子 (`ChannelId` は `uint16_t` のエイリアス)。
*   `topic`: チャンネルが属するトピックの名前。
*   `messageEncoding`: メッセージのエンコーディング（例: `"ros1"`, `"protobuf"`, `""`）。
*   `schemaId`: チャンネルが参照するスキーマのID。スキーマがない場合は `0`。
*   `metadata`: チャンネルに関連付けられた任意のキー/値のマップ。
