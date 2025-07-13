# SummaryOffset

`SummaryOffset` 構造体は、サマリーオフセットセクションに存在します。サマリーセクション内のレコードはグループ化されており、サマリーセクションで見つかった各レコードタイプについて、`SummaryOffset` はそのタイプのサマリーレコードが見つかるファイルオフセットと長さを参照します。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC SummaryOffset {
  OpCode groupOpCode;
  ByteOffset groupStart;
  ByteOffset groupLength;
};
```

**メンバー:**

*   `groupOpCode`: グループ化されたサマリーレコードのオペコード。
*   `groupStart`: グループの開始オフセット（ファイル先頭からのバイト数）。
*   `groupLength`: グループの長さ（バイト単位）。
