## Attachment Index (Opcode: 0x0A)

Attachment Indexレコードは、ファイル内のアタッチメントの位置を示すために使用されます。各`Attachment`レコードに対応して1つの`Attachment Index`レコードが存在します。

### フィールド

| フィールド名 | データ型 | 説明 |
|---|---|---|
| `offset` | `uint64` | ファイルの先頭からアタッチメントレコードまでのバイトオフセット。 |
| `length` | `uint64` | オペコードと長さプレフィックスを含むアタッチメントレコードのバイト長。 |
| `log_time` | `uint64` | アタッチメントが記録された時刻。 |
| `create_time` | `uint64` | アタッチメントが作成された時刻（利用不可の場合はゼロ）。 |
| `data_size` | `uint64` | アタッチメントデータのサイズ。 |
| `name` | `string` | アタッチメントの名前（例: "scene1.jpg"）。 |
| `media_type` | `string` | アタッチメントのメディアタイプ（例: "text/plain"）。 |
