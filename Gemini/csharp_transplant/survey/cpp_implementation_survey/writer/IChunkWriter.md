# IChunkWriter

`IChunkWriter` は、チャンクデータを書き込むための抽象インターフェースです。チャンクデータはメモリ上でバッファリングされ、最適な圧縮と最終的なチャンクデータサイズの計算をサポートするために単一のレコードとしてディスクに書き込まれます。

**定義:** `cpp/mcap/include/mcap/writer.hpp`

```cpp
class MCAP_PUBLIC IChunkWriter : public IWritable {
public:
  virtual ~IChunkWriter() override = default;

  virtual void end() override = 0;
  virtual uint64_t size() const override = 0;

  virtual uint64_t compressedSize() const = 0;
  virtual bool empty() const = 0;
  void clear();
  virtual const std::byte* data() const = 0;
  virtual const std::byte* compressedData() const = 0;

protected:
  virtual void handleClear() = 0;
};
```

**メンバー:**

*   `end()`: 現在の出力チャンクを閉じるための純粋仮想関数。この呼び出し後、`data()` と `size()` は圧縮データのデータとサイズを返す必要があります。
*   `size() const`: 非圧縮データのサイズ（バイト単位）を返す純粋仮想関数。
*   `compressedSize() const`: 圧縮データのサイズ（バイト単位）を返す純粋仮想関数。`end()` の後にのみ呼び出されます。
*   `empty() const`: 初期化後または最後の `clear()` 呼び出し以降に `write()` が一度も呼び出されていない場合に `true` を返す純粋仮想関数。
*   `clear()`: ライターの内部状態をクリアし、入力または出力バッファを破棄します。
*   `data() const`: 非圧縮データへのポインタを返す純粋仮想関数。
*   `compressedData() const`: 圧縮データへのポインタを返す純粋仮想関数。`end()` の後にのみ呼び出されます。
*   `handleClear()`: 内部状態をクリアするための純粋仮想関数。派生クラスで実装される必要があります。
