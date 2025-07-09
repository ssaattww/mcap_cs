# ICompressedReader

`ICompressedReader` は、圧縮されたデータを読み込むための抽象インターフェースです。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
class MCAP_PUBLIC ICompressedReader : public IReadable {
public:
  virtual ~ICompressedReader() override = default;

  virtual void reset(const std::byte* data, uint64_t size, uint64_t uncompressedSize) = 0;
  virtual Status status() const = 0;
};
```

**メンバー:**

*   `reset(const std::byte* data, uint64_t size, uint64_t uncompressedSize)`: リーダーの状態をリセットし、新しい圧縮データで初期化するための純粋仮想関数。
*   `status() const`: 伸長処理の現在の状態を報告するための純粋仮想関数。
