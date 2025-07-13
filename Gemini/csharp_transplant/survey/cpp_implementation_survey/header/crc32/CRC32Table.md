# CRC32Table

`CRC32Table` 構造体は、CRC32のルックアップテーブルを生成します。

**定義:** `cpp/mcap/include/mcap/crc32.hpp`

```cpp
template <size_t Polynomial, size_t NumTables>
struct CRC32Table {
private:
  std::array<uint32_t, 256 * NumTables> table = {};

public:
  constexpr CRC32Table();

  constexpr uint32_t operator[](size_t index) const;
};
```

**概要:**

この構造体は、コンパイル時にCRC32の計算に使用されるルックアップテーブルを生成するために使用されます。テンプレートパラメータとして多項式 (`Polynomial`) とテーブルの数 (`NumTables`) を受け取ります。

*   コンストラクタは `constexpr` であり、コンパイル時にテーブルを初期化します。
*   `operator[]` は、テーブルの特定のエントリへのアクセスを提供します。

このライブラリでは、`CRC32_TABLE` という名前の `CRC32Table<0xedb88320, 8>` のインスタンスが定義され、`crc32Update` 関数内で使用されます。
