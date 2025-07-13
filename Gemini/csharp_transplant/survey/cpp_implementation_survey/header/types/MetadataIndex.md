# MetadataIndex

`MetadataIndex` 構造体は、サマリーセクションに存在し、単一のメタデータレコードのサマリー情報を提供します。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC MetadataIndex {
  uint64_t offset;
  uint64_t length;
  std::string name;

  MetadataIndex() = default;
  MetadataIndex(const Metadata& metadata, ByteOffset fileOffset);
};
```

**メンバー:**

*   `offset`: ファイル先頭からのメタデータレコードの開始オフセット。
*   `length`: メタデータレコードの長さ（バイト単位）。
*   `name`: メタデータグループの名前。
