# IWritable

`IWritable` は、MCAPデータを書き込むための抽象インターフェースです。

**定義:** `cpp/mcap/include/mcap/writer.hpp`

```cpp
class MCAP_PUBLIC IWritable {
public:
  bool crcEnabled = false;

  IWritable() noexcept;
  virtual ~IWritable() = default;

  void write(const std::byte* data, uint64_t size);
  virtual void end() = 0;
  virtual uint64_t size() const = 0;
  uint32_t crc();
  void resetCrc();
  virtual void flush() {}

protected:
  virtual void handleWrite(const std::byte* data, uint64_t size) = 0;

private:
  uint32_t crc_;
};
```

**メンバー:**

*   `crcEnabled`: CRC計算が有効かどうかを示すフラグ。
*   `write(const std::byte* data, uint64_t size)`: 出力にデータを書き込みます。`crcEnabled` が `true` の場合、CRC計算も更新します。
*   `end()`: 書き込み処理を終了するための純粋仮想関数。
*   `size() const`: 書き込まれた合計バイト数を返す純粋仮想関数。
*   `crc()`: 計算されたCRC32値を返します。
*   `resetCrc()`: CRC32計算をリセットします。
*   `flush()`: バッファされたデータをフラッシュするための仮想関数。デフォルトは空の実装です。
*   `handleWrite(const std::byte* data, uint64_t size)`: 実際の書き込み処理を行うための純粋仮想関数。派生クラスで実装される必要があります。
