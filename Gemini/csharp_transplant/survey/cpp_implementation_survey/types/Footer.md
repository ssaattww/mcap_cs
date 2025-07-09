# Footer

`Footer` 構造体は、MCAPファイルの末尾に置かれ、サマリーセクションへのオフセットなどを含むレコードです。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC Footer {
  ByteOffset summaryStart;
  ByteOffset summaryOffsetStart;
  uint32_t summaryCrc;

  Footer() = default;
  Footer(ByteOffset summaryStart, ByteOffset summaryOffsetStart)
      : summaryStart(summaryStart)
      , summaryOffsetStart(summaryOffsetStart)
      , summaryCrc(0) {}
};
```

**メンバー:**

*   `summaryStart`: サマリーセクションの開始オフセット（ファイル先頭からのバイト数）。
*   `summaryOffsetStart`: サマリーオフセットセクションの開始オフセット（ファイル先頭からのバイト数）。
*   `summaryCrc`: サマリーセクションとサマリーオフセットセクションを結合したもののCRC32値。オプション。
