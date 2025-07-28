## Chunk Index (Opcode: 0x08)

Chunk Indexレコードは、ファイル内の各`Chunk`レコードとその関連する`Message Index`レコードの位置を示すために存在します。

### フィールド

| フィールド名 | データ型 | 説明 |
|---|---|---|
| `message_start_time` | `uint64` | チャンク内の最も古いメッセージのログタイム。 |
| `message_end_time` | `uint64` | チャンク内の最も新しいメッセージのログタイム。 |
| `chunk_start_offset` | `uint64` | ファイルの先頭からチャンクレコードまでのオフセット。 |
| `chunk_length` | `uint64` | オペコードと長さプレフィックスを含むチャンクレコードのバイト長。 |
| `message_index_offsets` | `map<uint16, uint64>` | チャンク後のメッセージインデックスレコードのチャネルIDからファイル先頭からのオフセットへのマッピング。 |
| `message_index_length` | `uint64` | チャンク後のメッセージインデックスレコードの合計バイト長。 |
| `compression` | `string` | チャンク内で使用される圧縮アルゴリズム（例: "zstd", "lz4", ""）。 |
| `compressed_size` | `uint64` | チャンクレコードフィールドの圧縮後のサイズ。 |
| `uncompressed_size` | `uint64` | チャンクレコードフィールドの非圧縮サイズ。 |
