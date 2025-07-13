# ZStdReader

`ZStdReader` は、Zstandard (https://facebook.github.io/zstd/) データを伸長する `ICompressedReader` の実装です。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
#ifndef MCAP_COMPRESSION_NO_ZSTD
class MCAP_PUBLIC ZStdReader final : public ICompressedReader {
public:
  void reset(const std::byte* data, uint64_t size, uint64_t uncompressedSize) override;
  uint64_t read(std::byte** output, uint64_t offset, uint64_t size) override;
  uint64_t size() const override;
  Status status() const override;

  static Status DecompressAll(const std::byte* data, uint64_t compressedSize,
                              uint64_t uncompressedSize, ByteArray* output);
  ZStdReader() = default;
  ZStdReader(const ZStdReader&) = delete;
  ZStdReader& operator=(const ZStdReader&) = delete;
  ZStdReader(ZStdReader&&) = delete;
  ZStdReader& operator=(ZStdReader&&) = delete;

private:
  Status status_;
  ByteArray uncompressedData_;
};
#endif
```

**メンバー:**

*   `reset(...)`: Zstdで圧縮されたデータを伸長し、内部の非圧縮データバッファに格納します。
*   `read(...)`: `output` ポインタを、要求されたオフセットの非圧縮データバッファを指すように設定します。
*   `size() const`: 非圧縮データの合計サイズを返します。
*   `status() const`: 伸長処理の状態を返します。
*   `DecompressAll(...)`: Zstdで圧縮されたチャンク全体を一度に伸長する静的メソッド。
