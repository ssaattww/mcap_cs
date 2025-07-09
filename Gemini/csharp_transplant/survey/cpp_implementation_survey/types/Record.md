# Record

`Record` 構造体は、MCAPファイル内の全てのレコードの基本となる汎用的なTLV (Type-Length-Value) 構造体です。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC Record {
  OpCode opcode;
  uint64_t dataSize;
  std::byte* data;

  uint64_t recordSize() const {
    return sizeof(opcode) + sizeof(dataSize) + dataSize;
  }
};
```

**メンバー:**

*   `opcode`: レコードの種類を識別する `OpCode` 列挙型。
*   `dataSize`: `data` が指すペイロードのサイズ（バイト単位）。
*   `data`: レコードのペイロードデータへのポインタ。
*   `recordSize()`: レコード全体のサイズ（opcode + dataSize + dataSizeで示されるデータサイズ）を返します。
