# ParseUint64

`ParseUint64` は、バイト配列からリトルエンディアンで64ビット符号なし整数を解析するインライン関数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline uint64_t ParseUint64(const std::byte* data) {
  return uint64_t(data[0]) | (uint64_t(data[1]) << 8) | (uint64_t(data[2]) << 16) |
         (uint64_t(data[3]) << 24) | (uint64_t(data[4]) << 32) | (uint64_t(data[5]) << 40) |
         (uint64_t(data[6]) << 48) | (uint64_t(data[7]) << 56);
}

inline Status ParseUint64(const std::byte* data, uint64_t maxSize, uint64_t* output) {
  if (maxSize < 8) {
    const auto msg = StrCat("cannot read uint64 from ", maxSize, " bytes");
    return Status{StatusCode::InvalidRecord, msg};
  }
  *output = ParseUint64(data);
  return StatusCode::Success;
}
```

**概要:**

この関数には2つのオーバーロードがあります。

1.  `const std::byte* data` を引数にとるバージョンは、与えられたポインタから8バイトを読み取り、リトルエンディアンの `uint64_t` として解釈して返します。
2.  `const std::byte* data`, `uint64_t maxSize`, `uint64_t* output` を引数にとるバージョンは、バッファの境界チェックを行いながら解析します。読み取り可能なバイト数が8バイト未満の場合はエラー `Status` を返します。成功した場合は、解析結果を `output` に格納し、`StatusCode::Success` を返します。
