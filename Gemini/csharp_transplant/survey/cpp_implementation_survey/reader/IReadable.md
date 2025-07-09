# IReadable

`IReadable` は、MCAPデータを読み込むための抽象インターフェースです。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
struct MCAP_PUBLIC IReadable {
  virtual ~IReadable() = default;

  virtual uint64_t size() const = 0;
  virtual uint64_t read(std::byte** output, uint64_t offset, uint64_t size) = 0;
};
```

**メンバー:**

*   `size() const`: ファイルのサイズ（バイト単位）を返す純粋仮想関数。
*   `read(std::byte** output, uint64_t offset, uint64_t size)`: ファイルの一部を読み込むための純粋仮想関数。このメソッドは、内部バッファを維持し、データを読み込んで `output` ポインタを更新するか、可能であれば `output` ポインタを直接ソースデータを指すように更新することが期待されます。ポインタとデータは、次の `read()` 呼び出しまで有効で変更されないままである必要があります。
