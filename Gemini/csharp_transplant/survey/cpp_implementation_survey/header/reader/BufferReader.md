# BufferReader

`BufferReader` は、非圧縮データを直接パススルーする「ヌル」圧縮リーダーです。内部バッファは割り当てられません。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
class MCAP_PUBLIC BufferReader final : public ICompressedReader {
public:
  void reset(const std::byte* data, uint64_t size, uint64_t uncompressedSize) override;
  uint64_t read(std::byte** output, uint64_t offset, uint64_t size) override;
  uint64_t size() const override;
  Status status() const override;

  BufferReader() = default;
  BufferReader(const BufferReader&) = delete;
  BufferReader& operator=(const BufferReader&) = delete;
  BufferReader(BufferReader&&) = delete;
  BufferReader& operator=(BufferReader&&) = delete;

private:
  const std::byte* data_;
  uint64_t size_;
};
```

**メンバー:**

*   `reset(...)`: 内部のデータポインタとサイズを、与えられた非圧縮データに設定します。
*   `read(...)`: `output` ポインタを、要求されたオフセットの内部データポインタを指すように設定します。
*   `size() const`: データの合計サイズを返します。
*   `status() const`: 常に `StatusCode::Success` を返します。
