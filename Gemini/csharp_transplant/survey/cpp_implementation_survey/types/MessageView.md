# MessageView

`MessageView` 構造体は、ファイル内のメッセージをイテレートする際に返されます。`MessageView` は1つの `Message` への参照、その `Channel` へのポインタ、およびその `Channel` の `Schema` へのオプションのポインタを含みます。`Channel` ポインタは有効であることが保証されますが、`Schema` ポインタは `Channel` が `schema_id 0` を参照している場合は `null` になることがあります。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC MessageView {
  const Message& message;
  const ChannelPtr channel;
  const SchemaPtr schema;
  const RecordOffset messageOffset;

  MessageView(const Message& message, const ChannelPtr channel, const SchemaPtr schema,
              RecordOffset offset)
      : message(message)
      , channel(channel)
      , schema(schema)
      , messageOffset(offset) {}
};
```

**メンバー:**

*   `message`: 参照されるメッセージ。
*   `channel`: メッセージが属するチャンネルへの共有ポインタ。
*   `schema`: チャンネルが参照するスキーマへの共有ポインタ。チャンネルが `schema_id 0` を参照している場合は `null` になることがあります。
*   `messageOffset`: メッセージのレコードオフセット。
