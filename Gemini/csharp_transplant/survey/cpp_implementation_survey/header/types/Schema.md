# Schema

`Schema` 構造体は、メッセージのスキーマ定義を記述します。一つ以上の `Channel` レコードが単一の `Schema` にマップされます。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC Schema {
  SchemaId id;
  std::string name;
  std::string encoding;
  ByteArray data;

  Schema() = default;

  Schema(const std::string_view name, const std::string_view encoding, const std::string_view data)
      : name(name)
      , encoding(encoding)
      , data{reinterpret_cast<const std::byte*>(data.data()),
             reinterpret_cast<const std::byte*>(data.data() + data.size())} {}

  Schema(const std::string_view name, const std::string_view encoding, const ByteArray& data)
      : name(name)
      , encoding(encoding)
      , data{data} {}
};
```

**メンバー:**

*   `id`: スキーマの一意な識別子 (`SchemaId` は `uint16_t` のエイリアス)。
*   `name`: スキーマの名前。
*   `encoding`: スキーマのエンコーディング（例: `"ros1"`, `"protobuf"`, `"json"`）。
*   `data`: スキーマの生データ（バイト配列）。
