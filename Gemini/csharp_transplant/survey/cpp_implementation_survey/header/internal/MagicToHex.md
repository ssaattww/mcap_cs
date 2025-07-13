# MagicToHex

`MagicToHex` は、MCAPのマジックバイト配列を16進数文字列に変換するインライン関数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline std::string MagicToHex(const std::byte* data) {
  return internal::ToHex(data[0]) + internal::ToHex(data[1]) + internal::ToHex(data[2]) +
         internal::ToHex(data[3]) + internal::ToHex(data[4]) + internal::ToHex(data[5]) +
         internal::ToHex(data[6]) + internal::ToHex(data[7]);
}
```

**概要:**

この関数は、8バイトのマジックバイト配列を受け取り、各バイトを `internal::ToHex` 関数を使用して2文字の16進数文字列に変換し、それらを連結して16文字の文字列を返します。
