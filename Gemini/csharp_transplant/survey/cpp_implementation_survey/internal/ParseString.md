# ParseString

`ParseString` は、バイト配列から長さプレフィックスを持つ文字列を `std::string` として解析するインライン関数です。この関数は文字列のコピーを作成します。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline Status ParseString(const std::byte* data, uint64_t maxSize, std::string* output) {
  uint32_t size = 0;
  if (auto status = ParseUint32(data, maxSize, &size); !status.ok()) {
    return status;
  }
  if (uint64_t(size) > (maxSize - 4)) {
    const auto msg = StrCat("string size ", size, " exceeds remaining bytes ", (maxSize - 4));
    return Status(StatusCode::InvalidRecord, msg);
  }
  *output = std::string(reinterpret_cast<const char*>(data + 4), size);
  return StatusCode::Success;
}
```

**概要:**

この関数は、`ParseStringView` と同様に、まず4バイトの長さプレフィックスを読み取り、サイズを検証します。その後、データポインタから指定された長さの `std::string` を構築し、`output` に格納します。成功した場合は `StatusCode::Success` を返します。
