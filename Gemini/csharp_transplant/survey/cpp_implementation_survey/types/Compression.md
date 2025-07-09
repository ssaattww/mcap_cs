# Compression

`Compression` 列挙型は、サポートされているMCAP圧縮アルゴリズムを定義します。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
enum struct Compression {
  None,
  Lz4,
  Zstd,
};
```

**メンバー:**

*   `None`: 圧縮なし。
*   `Lz4`: LZ4圧縮。
*   `Zstd`: Zstandard圧縮。
