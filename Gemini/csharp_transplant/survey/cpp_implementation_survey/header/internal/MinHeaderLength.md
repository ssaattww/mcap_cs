# MinHeaderLength

`MinHeaderLength` は、MCAPヘッダーの最小長を定義する `constexpr` 定数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
constexpr uint64_t MinHeaderLength = /* magic bytes */ sizeof(Magic) +
                                     /* opcode */ 1 +
                                     /* record length */ 8 +
                                     /* profile length */ 4 +
                                     /* library length */ 4;
```

**概要:**

この定数は、MCAPファイルのヘッダーレコードが持つべき最小のバイトサイズを計算します。内訳は以下の通りです。

*   マジックバイト (`Magic`)
*   オペコード (1バイト)
*   レコード長 (8バイト)
*   プロファイル文字列の長さ (4バイト)
*   ライブラリ文字列の長さ (4バイト)
