# LZ4Writer

`LZ4Writer` は、`IChunkWriter` インターフェースを実装し、一時バッファにデータを保持してからLZ4圧縮バッファにフラッシュするインメモリライターです。

**定義:** `cpp/mcap/include/mcap/writer.hpp`

```cpp
#ifndef MCAP_COMPRESSION_NO_LZ4
class MCAP_PUBLIC LZ4Writer final : public IChunkWriter {
public:
  LZ4Writer(CompressionLevel compressionLevel, uint64_t chunkSize);

  void handleWrite(const std::byte* data, uint64_t size) override;
  void end() override;
  uint64_t size() const override;
  uint64_t compressedSize() const override;
  bool empty() const override;
  void handleClear() override;
  const std::byte* data() const override;
  const std::byte* compressedData() const override;

private:
  std::vector<std::byte> uncompressedBuffer_;
  std::vector<std::byte> compressedBuffer_;
  CompressionLevel compressionLevel_;
};
#endif
```

**メンバー:**

*   `LZ4Writer(CompressionLevel compressionLevel, uint64_t chunkSize)`: 圧縮レベルとチャンクサイズを受け取るコンストラクタ。
*   `handleWrite(const std::byte* data, uint64_t size)`: 非圧縮バッファにデータを書き込みます。
*   `end()`: 非圧縮バッファのデータをLZ4アルゴリズムで圧縮し、圧縮バッファに格納します。
*   `size() const`: 非圧縮データのサイズを返します。
*   `compressedSize() const`: 圧縮後のデータのサイズを返します。
*   `empty() const`: 非圧縮バッファが空かどうかを返します。
*   `handleClear()`: 非圧縮および圧縮バッファをクリアします。
*   `data() const`: 非圧縮データへのポインタを返します。
*   `compressedData() const`: 圧縮データへのポインタを返します。
