# BufferWriter

`BufferWriter` は、`IChunkWriter` インターフェースを実装し、拡張可能なバッファをバックエンドとするインメモリライターです。

**定義:** `cpp/mcap/include/mcap/writer.hpp`

```cpp
class MCAP_PUBLIC BufferWriter final : public IChunkWriter {
public:
  void handleWrite(const std::byte* data, uint64_t size) override;
  void end() override;
  uint64_t size() const override;
  uint64_t compressedSize() const override;
  bool empty() const override;
  void handleClear() override;
  const std::byte* data() const override;
  const std::byte* compressedData() const override;

private:
  std::vector<std::byte> buffer_;
};
```

**メンバー:**

*   `handleWrite(const std::byte* data, uint64_t size)`: `std::vector` バッファにデータを書き込みます。
*   `end()`: このクラスでは何もしません（圧縮を行わないため）。
*   `size() const`: バッファに書き込まれた合計バイト数を返します。
*   `compressedSize() const`: 圧縮を行わないため、`size()` と同じ値を返します。
*   `empty() const`: バッファが空かどうかを返します。
*   `handleClear()`: バッファをクリアします。
*   `data() const`: バッファのデータへのポインタを返します。
*   `compressedData() const`: 圧縮を行わないため、`data()` と同じ値を返します。
