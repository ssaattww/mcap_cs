## Statistics (Opcode: 0x0B)

Statisticsレコードは、記録されたデータに関する要約情報を含み、ファイル内に最大1つ存在します。

### フィールド

| フィールド名 | データ型 | 説明 |
|---|---|---|
| `message_count` | `uint64` | ファイル内の`Message`レコードの総数。 |
| `schema_count` | `uint16` | ファイル内のユニークなスキーマIDの数（ゼロを除く）。 |
| `channel_count` | `uint32` | ファイル内のユニークなチャンネルIDの数。 |
| `attachment_count` | `uint32` | ファイル内の`Attachment`レコードの数。 |
| `metadata_count` | `uint32` | ファイル内の`Metadata`レコードの数。 |
| `chunk_count` | `uint32` | ファイル内の`Chunk`レコードの数。 |
| `message_start_time` | `uint64` | ファイル内の最も古いメッセージの`log_time`。メッセージがない場合はゼロ。 |
| `message_end_time` | `uint64` | ファイル内の最新のメッセージの`log_time`。メッセージがない場合はゼロ。 |
| `channel_message_counts` | `map<uint16, uint64>` | チャンネルIDからそのチャンネルの総メッセージ数へのマッピング。この統計が利用できない場合は空のマップ。 |

`channel_message_counts`が空でない`Statistics`レコードを使用する場合、サマリーデータセクションにはすべての`Channel`レコードのコピーが含まれている必要があり、それらの`Channel`レコードは`Statistics`レコードより前に出現する必要があります。
