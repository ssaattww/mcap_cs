# LZ4Reader

`LZ4Reader` は、LZ4 (https://lz4.github.io/lz4/) データを伸長する `ICompressedReader` の実装です。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
#ifndef MCAP_COMPRESSION_NO_LZ4
class MCAP_PUBLIC LZ4Reader final : public ICompressedReader {
public:
  void reset(const std::byte* data, uint64_t size, uint64_t uncompressedSize) override;
  uint64_t read(std::byte** output, uint64_t offset, uint64_t size) override;
  uint64_t size() const override;
  Status status() const override;

  Status decompressAll(const std::byte* data, uint64_t size, uint64_t uncompressedSize,
                       ByteArray* output);
  LZ4Reader();
  LZ4Reader(const LZ4Reader&) = delete;
  LZ4Reader& operator=(const LZ4Reader&) = delete;
  LZ4Reader(LZ4Reader&&) = delete;
  LZ4Reader& operator=(LZ4Reader&&) = delete;
  ~LZ4Reader() override;

private:
  void* decompressionContext_ = nullptr;  // LZ4F_dctx*
  Status status_;
  const std::byte* compressedData_;
  ByteArray uncompressedData_;
  uint64_t compressedSize_;
  uint64_t uncompressedSize_;
};
#endif
```

**メンバー:**

*   `reset(...)`: LZ4で圧縮されたデータを伸長し、内部の非圧縮データバッファに格納します。
*   `read(...)`: `output` ポインタを、要求されたオフセットの非圧縮データバッファを指すように設定します。
*   `size() const`: 非圧縮データの合計サイズを返します。
*   `status() const`: 伸長処理の状態を返します。
*   `decompressAll(...)`: LZ4で圧縮されたチャンク全体を一度に伸長するメソッド。
*   `~LZ4Reader()`: LZ4伸長コンテキストを解放するデストラクタ。
