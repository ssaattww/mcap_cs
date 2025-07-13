# CompressionLevel

`CompressionLevel` 列挙型は、圧縮が有効な場合に使用する圧縮レベルを定義します。遅いほどファイルサイズは小さくなりますが、CPU時間をより多く消費します。これらのレベルは、各圧縮アルゴリズムの内部設定にマッピングされます。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
enum struct CompressionLevel {
  Fastest,
  Fast,
  Default,
  Slow,
  Slowest,
};
```

**メンバー:**

*   `Fastest`: 最速の圧縮レベル。
*   `Fast`: 高速な圧縮レベル。
*   `Default`: デフォルトの圧縮レベル。
*   `Slow`: 低速な圧縮レベル。
*   `Slowest`: 最も低速な圧縮レベル。
