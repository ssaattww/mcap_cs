# KeyValueMapSize

`KeyValueMapSize` は、`KeyValueMap` のシリアライズ後のサイズを計算するインライン関数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline uint32_t KeyValueMapSize(const KeyValueMap& map) {
  size_t size = 0;
  for (const auto& [key, value] : map) {
    size += 4 + key.size() + 4 + value.size();
  }
  return (uint32_t)(size);
}
```

**概要:**

この関数は、キーと値のペアのマップを受け取り、それがMCAP形式でシリアライズされた場合の合計バイトサイズを計算します。各キーと値のペアについて、キーの長さプレフィックス (4バイト)、キーの文字列、値の長さプレフィックス (4バイト)、値の文字列のサイズを合計します。
