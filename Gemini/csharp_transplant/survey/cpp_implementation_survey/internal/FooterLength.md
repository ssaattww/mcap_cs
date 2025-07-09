# FooterLength

`FooterLength` は、MCAPフッターの長さを定義する `constexpr` 定数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
constexpr uint64_t FooterLength = /* opcode */ 1 +
                                  /* record length */ 8 +
                                  /* summary start */ 8 +
                                  /* summary offset start */ 8 +
                                  /* summary crc */ 4 +
                                  /* magic bytes */ sizeof(Magic);
```

**概要:**

この定数は、MCAPファイルのフッターレコードと末尾のマジックバイトを合わせた長さを計算します。内訳は以下の通りです。

*   オペコード (1バイト)
*   レコード長 (8バイト)
*   サマリー開始位置 (8バイト)
*   サマリーオフセット開始位置 (8バイト)
*   サマリーCRC (4バイト)
*   マジックバイト (`Magic`)
