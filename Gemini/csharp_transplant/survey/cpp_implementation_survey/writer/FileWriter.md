# FileWriter

`FileWriter` は、`IWritable` インターフェースを実装し、`fopen()` によって作成された `FILE*` ポインタをラップしてファイルへの書き込みを行います。

**定義:** `cpp/mcap/include/mcap/writer.hpp`

```cpp
class MCAP_PUBLIC FileWriter final : public IWritable {
public:
  ~FileWriter() override;

  Status open(std::string_view filename);

  void handleWrite(const std::byte* data, uint64_t size) override;
  void end() override;
  void flush() override;
  uint64_t size() const override;

private:
  std::FILE* file_ = nullptr;
  uint64_t size_ = 0;
};
```

**メンバー:**

*   `open(std::string_view filename)`: 指定されたファイル名で書き込み用にファイルを開きます。
*   `handleWrite(const std::byte* data, uint64_t size)`: `fwrite` を使用してファイルにデータを書き込みます。
*   `end()`: `fclose` を使用してファイルを閉じます。
*   `flush()`: `fflush` を使用してファイルバッファをフラッシュします。
*   `size() const`: これまでに書き込まれた合計バイト数を返します。
