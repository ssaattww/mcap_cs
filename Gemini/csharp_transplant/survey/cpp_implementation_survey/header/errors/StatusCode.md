# StatusCode

`StatusCode` 列挙型は、MCAPリーダーおよびライターの操作の結果を示すステータスコードを定義します。

**定義:** `cpp/mcap/include/mcap/errors.hpp`

```cpp
enum class StatusCode {
  Success = 0,
  NotOpen,
  InvalidSchemaId,
  InvalidChannelId,
  FileTooSmall,
  ReadFailed,
  MagicMismatch,
  InvalidFile,
  InvalidRecord,
  InvalidOpCode,
  InvalidChunkOffset,
  InvalidFooter,
  DecompressionFailed,
  DecompressionSizeMismatch,
  UnrecognizedCompression,
  OpenFailed,
  MissingStatistics,
  InvalidMessageReadOptions,
  NoMessageIndexesAvailable,
  UnsupportedCompression,
};
```

**メンバー:**

*   `Success`: 成功。
*   `NotOpen`: ファイルまたはストリームが開かれていません。
*   `InvalidSchemaId`: 無効なスキーマID。
*   `InvalidChannelId`: 無効なチャンネルID。
*   `FileTooSmall`: ファイルサイズが小さすぎます。
*   `ReadFailed`: 読み取りに失敗しました。
*   `MagicMismatch`: ファイルのマジックバイトが一致しません。
*   `InvalidFile`: 無効なファイル形式です。
*   `InvalidRecord`: 無効なレコードです。
*   `InvalidOpCode`: 無効なオペコードです。
*   `InvalidChunkOffset`: 無効なチャンクオフセットです。
*   `InvalidFooter`: 無効なフッターです。
*   `DecompressionFailed`: 伸長に失敗しました。
*   `DecompressionSizeMismatch`: 伸長後のサイズが一致しません。
*   `UnrecognizedCompression`: 認識できない圧縮形式です。
*   `OpenFailed`: ファイルを開くのに失敗しました。
*   `MissingStatistics`: 統計情報が見つかりません。
*   `InvalidMessageReadOptions`: メッセージ読み取りオプションが競合しています。
*   `NoMessageIndexesAvailable`: ファイルにメッセージインデックスがありません。
*   `UnsupportedCompression`: サポートされていない圧縮形式です。
