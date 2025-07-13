# FileStreamReader

`FileStreamReader` は、`IReadable` インターフェースを実装し、`std::ifstream` 入力ファイルストリームをラップします。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
class MCAP_PUBLIC FileStreamReader final : public IReadable {
public:
  FileStreamReader(std::ifstream& stream);

  uint64_t size() const override;
  uint64_t read(std::byte** output, uint64_t offset, uint64_t size) override;

private:
  std::ifstream& stream_;
  std::vector<std::byte> buffer_;
  uint64_t size_;
  uint64_t position_;
};
```

**メンバー:**

*   `FileStreamReader(std::ifstream& stream)`: `std::ifstream` を受け取るコンストラクタ。
*   `size() const`: ストリームの合計サイズを返します。
*   `read(std::byte** output, uint64_t offset, uint64_t size)`: `seekg` と `read` を使用してストリームからデータを読み取り、内部バッファに格納して、`output` ポインタをそのバッファを指すように設定します。
