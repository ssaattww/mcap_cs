# AttachmentIndex

`AttachmentIndex` 構造体は、サマリーセクションに存在し、単一のアタッチメントのサマリー情報を提供します。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC AttachmentIndex {
  ByteOffset offset;
  ByteOffset length;
  Timestamp logTime;
  Timestamp createTime;
  uint64_t dataSize;
  std::string name;
  std::string mediaType;

  AttachmentIndex() = default;
  AttachmentIndex(const Attachment& attachment, ByteOffset fileOffset)
      : offset(fileOffset)
      , length(9 +
               /* name */ 4 + attachment.name.size() +
               /* log_time */ 8 +
               /* create_time */ 8 +
               /* media_type */ 4 + attachment.mediaType.size() +
               /* data */ 8 + attachment.dataSize +
               /* crc */ 4)
      , logTime(attachment.logTime)
      , createTime(attachment.createTime)
      , dataSize(attachment.dataSize)
      , name(attachment.name)
      , mediaType(attachment.mediaType) {}
};
```

**メンバー:**

*   `offset`: ファイル先頭からのアタッチメントレコードの開始オフセット。
*   `length`: アタッチメントレコードの長さ（バイト単位）。
*   `logTime`: アタッチメントが記録されたナノ秒単位のタイムスタンプ。
*   `createTime`: アタッチメントが作成されたナノ秒単位のタイムスタンプ。
*   `dataSize`: アタッチメントのペイロードサイズ（バイト単位）。
*   `name`: アタッチメントの名前。
*   `mediaType`: アタッチメントのメディアタイプ。
