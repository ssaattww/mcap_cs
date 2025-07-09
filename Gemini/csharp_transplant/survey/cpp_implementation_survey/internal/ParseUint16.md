# ParseUint16

`ParseUint16` は、バイト配列からリトルエンディアンで16ビット符号なし整数を解析するインライン関数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline uint16_t ParseUint16(const std::byte* data) {
  return uint16_t(data[0]) | (uint16_t(data[1]) << 8);
}
```

**概要:**

この関数は、`std::byte` のポインタを受け取り、最初の2バイトをリトルエンディアンの `uint16_t` として解釈し、その値を返します。
