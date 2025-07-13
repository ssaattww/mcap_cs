# FileReader

`FileReader` は、`IReadable` インターフェースを実装し、`fopen()` によって作成された `FILE*` ポインタと読み取りバッファをラップします。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
class MCAP_PUBLIC FileReader final : public IReadable {
public:
  FileReader(std::FILE* file);

  uint64_t size() const override;
  uint64_t read(std::byte** output, uint64_t offset, uint64_t size) override;

private:
  std::FILE* file_;
  std::vector<std::byte> buffer_;
  uint64_t size_;
  uint64_t position_;
};
```

**メンバー:**

*   `FileReader(std::FILE* file)`: `FILE*` ポインタを受け取るコンストラクタ。
*   `size() const`: ファイルの合計サイズを返します。
*   `read(std::byte** output, uint64_t offset, uint64_t size)`: `fseek` と `fread` を使用してファイルからデータを読み取り、内部バッファに格納して、`output` ポインタをそのバッファを指すように設定します。
