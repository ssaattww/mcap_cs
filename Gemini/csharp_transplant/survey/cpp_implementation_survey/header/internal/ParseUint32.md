# ParseUint32

`ParseUint32` は、バイト配列からリトルエンディアンで32ビット符号なし整数を解析するインライン関数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline uint32_t ParseUint32(const std::byte* data) {
  return uint32_t(data[0]) | (uint32_t(data[1]) << 8) | (uint32_t(data[2]) << 16) |
         (uint32_t(data[3]) << 24);
}

inline Status ParseUint32(const std::byte* data, uint64_t maxSize, uint32_t* output) {
  if (maxSize < 4) {
    const auto msg = StrCat("cannot read uint32 from ", maxSize, " bytes");
    return Status{StatusCode::InvalidRecord, msg};
  }
  *output = ParseUint32(data);
  return StatusCode::Success;
}
```

**概要:**

この関数には2つのオーバーロードがあります。

1.  `const std::byte* data` を引数にとるバージョンは、与えられたポインタから4バイトを読み取り、リトルエンディアンの `uint32_t` として解釈して返します。
2.  `const std::byte* data`, `uint64_t maxSize`, `uint32_t* output` を引数にとるバージョンは、バッファの境界チェックを行いながら解析します。読み取り可能なバイト数が4バイト未満の場合はエラー `Status` を返します。成功した場合は、解析結果を `output` に格納し、`StatusCode::Success` を返します。
