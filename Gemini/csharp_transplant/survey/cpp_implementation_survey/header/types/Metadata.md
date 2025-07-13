# Metadata

`Metadata` 構造体は、任意のユーザーデータを含む名前付きのキー/値文字列マップを保持します。メタデータレコードはチャンクの外のデータセクションに存在します。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC Metadata {
  std::string name;
  KeyValueMap metadata;
};
```

**メンバー:**

*   `name`: メタデータグループの名前。
*   `metadata`: キー/値の文字列ペアのマップ。
