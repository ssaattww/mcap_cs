# DataEnd

`DataEnd` 構造体は、データセクションの終わりを示す最終レコードであり、サマリーセクションの開始を知らせます。オプションでデータセクション全体のCRCを含みます。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC DataEnd {
  uint32_t dataSectionCrc;
};
```

**メンバー:**

*   `dataSectionCrc`: データセクション全体のCRC32値。
