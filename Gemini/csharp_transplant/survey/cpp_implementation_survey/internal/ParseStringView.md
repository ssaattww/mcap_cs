# ParseStringView

`ParseStringView` は、バイト配列から長さプレフィックスを持つ文字列を `std::string_view` として解析するインライン関数です。この関数は文字列のコピーを作成しません。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline Status ParseStringView(const std::byte* data, uint64_t maxSize, std::string_view* output) {
  uint32_t size = 0;
  if (auto status = ParseUint32(data, maxSize, &size); !status.ok()) {
    const auto msg = StrCat("cannot read string size: ", status.message);
    return Status{StatusCode::InvalidRecord, msg};
  }
  if (uint64_t(size) > (maxSize - 4)) {
    const auto msg = StrCat("string size ", size, " exceeds remaining bytes ", (maxSize - 4));
    return Status(StatusCode::InvalidRecord, msg);
  }
  *output = std::string_view(reinterpret_cast<const char*>(data + 4), size);
  return StatusCode::Success;
}
```

**概要:**

この関数は、まず4バイトの長さプレフィックス (`uint32_t`) を読み取ります。次に、その長さが残りのバッファサイズを超えていないかを確認します。問題がなければ、データポインタから指定された長さの `std::string_view` を作成し、`output` に格納します。成功した場合は `StatusCode::Success` を返します。
