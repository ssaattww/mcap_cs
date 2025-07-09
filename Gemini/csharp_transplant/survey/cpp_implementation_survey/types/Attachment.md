# Attachment

`Attachment` 構造体は、MCAPファイルに埋め込まれた任意のファイルを表します。名前、メディアタイプ、タイムスタンプ、オプションのCRCを含みます。アタッチメントレコードはチャンクの外のデータセクションに書き込まれます。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC Attachment {
  Timestamp logTime;
  Timestamp createTime;
  std::string name;
  std::string mediaType;
  uint64_t dataSize;
  const std::byte* data = nullptr;
  uint32_t crc;
};
```

**メンバー:**

*   `logTime`: アタッチメントが記録されたナノ秒単位のタイムスタンプ。
*   `createTime`: アタッチメントが作成されたナノ秒単位のタイムスタンプ。
*   `name`: アタッチメントの名前。
*   `mediaType`: アタッチメントのメディアタイプ（MIMEタイプ）。
*   `dataSize`: アタッチメントのペイロードサイズ（バイト単位）。
*   `data`: アタッチメントのペイロードデータへのポインタ。
*   `crc`: アタッチメントデータのCRC32値。オプション。
