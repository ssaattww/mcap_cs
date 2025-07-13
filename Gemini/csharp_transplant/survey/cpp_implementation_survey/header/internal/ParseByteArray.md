# ParseByteArray

`ParseByteArray` は、バイト配列から長さプレフィックスを持つバイト配列を `ByteArray` (`std::vector<std::byte>`) として解析するインライン関数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
inline Status ParseByteArray(const std::byte* data, uint64_t maxSize, ByteArray* output) {
  uint32_t size = 0;
  if (auto status = ParseUint32(data, maxSize, &size); !status.ok()) {
    return status;
  }
  if (uint64_t(size) > (maxSize - 4)) {
    const auto msg = StrCat("byte array size ", size, " exceeds remaining bytes ", (maxSize - 4));
    return Status(StatusCode::InvalidRecord, msg);
  }
  output->resize(size);
  if (size > 0) {
    std::memcpy(output->data(), data + 4, size);
  }
  return StatusCode::Success;
}
```

**概要:**

この関数は、まず4バイトの長さプレフィックス (`uint32_t`) を読み取ります。次に、その長さが残りのバッファサイズを超えていないかを確認します。問題がなければ、`output` ベクターのサイズを読み取ったサイズに変更し、`std::memcpy` を使用してデータをコピーします。成功した場合は `StatusCode::Success` を返します。
