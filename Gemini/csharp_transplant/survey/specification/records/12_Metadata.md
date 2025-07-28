## Metadata (Opcode: 0x0C)

Metadataレコードは、キーと値のペアで任意のユーザーデータを格納するために使用されます。

### フィールド

| フィールド名 | データ型 | 説明 |
|---|---|---|
| `name` | `string` | メタデータレコードの名前を示します（例: `my_company_name_hardware_info`）。 |
| `metadata` | `map<string, string>` | キーと値のペアのマップです。例えば、キーとして `part_id`、`serial`、`board_revision` などが使用されます。 |

このレコードの役割は、ファイルに記録されたデータに関連する追加情報やコンテキストを提供することです。
