# ToHex

`ToHex` は、バイトを16進数文字列に変換するインライン関数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline std::string ToHex(uint8_t byte) {
  std::string result{2, '\0'};
  result[0] = "0123456789ABCDEF"[(uint8_t(byte) >> 4) & 0x0F];
  result[1] = "0123456789ABCDEF"[uint8_t(byte) & 0x0F];
  return result;
}
inline std::string ToHex(std::byte byte) {
  return ToHex(uint8_t(byte));
}
```

**概要:**

この関数は、1バイトのデータを2文字の16進数文字列に変換します。`uint8_t` と `std::byte` の両方の型に対するオーバーロードが提供されています。
