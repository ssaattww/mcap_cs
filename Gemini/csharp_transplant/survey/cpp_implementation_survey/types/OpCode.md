# OpCode

`OpCode` 列挙型は、MCAPレコードの種類を識別するために使用されます。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
enum struct OpCode : uint8_t {
  Header = 0x01,
  Footer = 0x02,
  Schema = 0x03,
  Channel = 0x04,
  Message = 0x05,
  Chunk = 0x06,
  MessageIndex = 0x07,
  ChunkIndex = 0x08,
  Attachment = 0x09,
  AttachmentIndex = 0x0A,
  Statistics = 0x0B,
  Metadata = 0x0C,
  MetadataIndex = 0x0D,
  SummaryOffset = 0x0E,
  DataEnd = 0x0F,
};
```

**メンバー:**

*   `Header`: ヘッダーレコード (0x01)
*   `Footer`: フッターレコード (0x02)
*   `Schema`: スキーマレコード (0x03)
*   `Channel`: チャンネルレコード (0x04)
*   `Message`: メッセージレコード (0x05)
*   `Chunk`: チャンクレコード (0x06)
*   `MessageIndex`: メッセージインデックスレコード (0x07)
*   `ChunkIndex`: チャンクインデックスレコード (0x08)
*   `Attachment`: アタッチメントレコード (0x09)
*   `AttachmentIndex`: アタッチメントインデックスレコード (0x0A)
*   `Statistics`: 統計レコード (0x0B)
*   `Metadata`: メタデータレコード (0x0C)
*   `MetadataIndex`: メタデータインデックスレコード (0x0D)
*   `SummaryOffset`: サマリーオフセットレコード (0x0E)
*   `DataEnd`: データエンドレコード (0x0F)
