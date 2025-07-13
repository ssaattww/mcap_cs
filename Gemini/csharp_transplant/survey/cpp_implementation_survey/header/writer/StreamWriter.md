# StreamWriter

`StreamWriter` は、`IWritable` インターフェースを実装し、`std::ostream` ストリームをラップして書き込みを行います。

**定義:** `cpp/mcap/include/mcap/writer.hpp`

```cpp
class MCAP_PUBLIC StreamWriter final : public IWritable {
public:
  StreamWriter(std::ostream& stream);

  void handleWrite(const std::byte* data, uint64_t size) override;
  void end() override;
  void flush() override;
  uint64_t size() const override;

private:
  std::ostream& stream_;
  uint64_t size_ = 0;
};
```

**メンバー:**

*   `StreamWriter(std::ostream& stream)`: 書き込み先の `std::ostream` を受け取るコンストラクタ。
*   `handleWrite(const std::byte* data, uint64_t size)`: `stream.write` を使用してストリームにデータを書き込みます。
*   `end()`: ストリームをフラッシュします。
*   `flush()`: `stream.flush` を使用してストリームバッファをフラッシュします。
*   `size() const`: これまでに書き込まれた合計バイト数を返します。
