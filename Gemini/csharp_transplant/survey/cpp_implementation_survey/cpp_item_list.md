# C++ MCAPライブラリ クラス・構造体一覧

| クラス/構造体名 | 機能カテゴリ | ヘッダーファイル名 |
|---|---|---|
| `McapWriter` | 書き込み機能 (`McapWriter`) | `writer.hpp` |
| `McapWriterOptions` | 基本型とユーティリティ | `writer.hpp` |
| `IWritable` | 低レベルインターフェースと拡張性 | `writer.hpp` |
| `FileWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` |
| `StreamWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` |
| `IChunkWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` |
| `BufferWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` |
| `LZ4Writer` | 低レベルインターフェースと拡張性 | `writer.hpp` |
| `ZStdWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` |
| `McapReader` | 読み込み機能 (`McapReader`) | `reader.hpp` |
| `ReadMessageOptions` | 基本型とユーティリティ | `reader.hpp` |
| `IReadable` | 低レベルインターフェースと拡張性 | `reader.hpp` |
| `FileReader` | 低レベルインターフェースと拡張性 | `reader.hpp` |
| `FileStreamReader` | 低レベルインターフェースと拡張性 | `reader.hpp` |
| `ICompressedReader` | 低レベルインターフェースと拡張性 | `reader.hpp` |
| `BufferReader` | 低レベルインターフェースと拡張性 | `reader.hpp` |
| `LZ4Reader` | 低レベルインターフェースと拡張性 | `reader.hpp` |
| `ZStdReader` | 低レベルインターフェースと拡張性 | `reader.hpp` |
| `RecordReader` | 低レベルインターフェースと拡張性 | `reader.hpp` |
| `TypedChunkReader` | 低レベルインターフェースと拡張性 | `reader.hpp` |
| `TypedRecordReader` | 低レベルインターフェースと拡張性 | `reader.hpp` |
| `IndexedMessageReader` | 読み込み機能 (`McapReader`) | `reader.hpp` |
| `LinearMessageView` | 読み込み機能 (`McapReader`) | `reader.hpp` |
| `Record` | データモデル | `types.hpp` |
| `Header` | データモデル | `types.hpp` |
| `Footer` | データモデル | `types.hpp` |
| `Schema` | データモデル | `types.hpp` |
| `Channel` | データモデル | `types.hpp` |
| `Message` | データモデル | `types.hpp` |
| `Chunk` | データモデル | `types.hpp` |
| `MessageIndex` | データモデル | `types.hpp` |
| `ChunkIndex` | データモデル | `types.hpp` |
| `Attachment` | データモデル | `types.hpp` |
| `AttachmentIndex` | データモデル | `types.hpp` |
| `Statistics` | データモデル | `types.hpp` |
| `Metadata` | データモデル | `types.hpp` |
| `MetadataIndex` | データモデル | `types.hpp` |
| `SummaryOffset` | データモデル | `types.hpp` |
| `DataEnd` | データモデル | `types.hpp` |
| `MessageView` | データモデル | `types.hpp` |
| `Status` | 基本型とユーティリティ | `errors.hpp` |
| `StatusCode` | 基本型とユーティリティ | `errors.hpp` |
| `OpCode` | 基本型とユーティリティ | `types.hpp` |
| `Compression` | 基本型とユーティリティ | `types.hpp` |
| `CompressionLevel` | 基本型とユーティリティ | `types.hpp` |
| `ReadSummaryMethod` | 基本型とユーティリティ | `reader.hpp` |
| `CRC32Table` | 低レベルインターフェースと拡張性 | `crc32.hpp` |
| `Interval` | 低レベルインターフェースと拡張性 | `intervaltree.hpp` |
| `IntervalTree` | 低レベルインターフェースと拡張性 | `intervaltree.hpp` |
| `ReadMessageJob` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` |
| `DecompressChunkJob` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` |
| `ReadJob` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` |
| `ReadJobQueue` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` |

# C++ MCAPライブラリ 内部ユーティリティ関数一覧

| 関数/定数名 | 機能カテゴリ | ヘッダーファイル名 |
|---|---|---|
| `MinHeaderLength` | Internalユーティリティ | `internal.hpp` |
| `FooterLength` | Internalユーティリティ | `internal.hpp` |
| `ToHex` | Internalユーティリティ | `internal.hpp` |
| `StrCat` | Internalユーティリティ | `internal.hpp` |
| `KeyValueMapSize` | Internalユーティリティ | `internal.hpp` |
| `CompressionString` | Internalユーティリティ | `internal.hpp` |
| `ParseUint16` | Internalユーティリティ | `internal.hpp` |
| `ParseUint32` | Internalユーティリティ | `internal.hpp` |
| `ParseUint64` | Internalユーティリティ | `internal.hpp` |
| `ParseStringView` | Internalユーティリティ | `internal.hpp` |
| `ParseString` | Internalユーティリティ | `internal.hpp` |
| `ParseByteArray` | Internalユーティリティ | `internal.hpp` |
| `ParseKeyValueMap` | Internalユーティリティ | `internal.hpp` |
| `MagicToHex` | Internalユーティリティ | `internal.hpp` |


