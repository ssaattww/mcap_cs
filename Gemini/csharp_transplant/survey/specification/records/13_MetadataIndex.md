## Metadata Index (Opcode: 0x0D)

Metadata Indexレコードは、ファイル内のMetadataレコードの位置を示すために使用されます。

### フィールド

| フィールド名 | データ型 | 説明 |
|---|---|---|
| `offset` | `uint64` | ファイルの先頭からMetadataレコードまでのバイトオフセットを示します。 |
| `length` | `uint64` | オペコードと長さプレフィックスを含む、Metadataレコードの合計バイト長を示します。 |
| `name` | `string` | Metadataレコードの名前を示します。 |

これらのフィールドにより、MCAPファイル内の特定のMetadataレコードを効率的に検索し、アクセスすることが可能になります。
