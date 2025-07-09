# CompressionString

`CompressionString` は、`Compression` 列挙型の値に対応する文字列を返すインライン関数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline const std::string CompressionString(Compression compression) {
  switch (compression) {
    case Compression::None:
    default:
      return std::string{};
    case Compression::Lz4:
      return "lz4";
    case Compression::Zstd:
      return "zstd";
  }
}
```

**概要:**

この関数は、`Compression` 列挙型の値を受け取り、MCAP仕様で定義されている対応する圧縮アルゴリズムの識別子文字列を返します。

*   `Compression::None` -> `""` (空文字列)
*   `Compression::Lz4` -> `"lz4"`
*   `Compression::Zstd` -> `"zstd"`
