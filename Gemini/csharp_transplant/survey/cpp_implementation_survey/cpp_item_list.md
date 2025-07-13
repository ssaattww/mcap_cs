# C++ MCAPライブラリ クラス・構造体一覧

| クラス/構造体名 | 機能カテゴリ | ヘッダーファイル名 | 詳細 |
|---|---|---|---|
| `McapWriter` | 書き込み機能 (`McapWriter`) | `writer.hpp` | [McapWriter](./writer/McapWriter.md) |
| `McapWriterOptions` | 基本型とユーティリティ | `writer.hpp` | [McapWriterOptions](./writer/McapWriterOptions.md) |
| `IWritable` | 低レベルインターフェースと拡張性 | `writer.hpp` | [IWritable](./writer/IWritable.md) |
| `FileWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` | [FileWriter](./writer/FileWriter.md) |
| `StreamWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` | [StreamWriter](./writer/StreamWriter.md) |
| `IChunkWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` | [IChunkWriter](./writer/IChunkWriter.md) |
| `BufferWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` | [BufferWriter](./writer/BufferWriter.md) |
| `LZ4Writer` | 低レベルインターフェースと拡張性 | `writer.hpp` | [LZ4Writer](./writer/LZ4Writer.md) |
| `ZStdWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` | [ZStdWriter](./writer/ZStdWriter.md) |
| `McapReader` | 読み込み機能 (`McapReader`) | `reader.hpp` | [McapReader](./reader/McapReader.md) |
| `ReadMessageOptions` | 基本型とユーティリティ | `reader.hpp` | [ReadMessageOptions](./reader/ReadMessageOptions.md) |
| `IReadable` | 低レベルインターフェースと拡張性 | `reader.hpp` | [IReadable](./reader/IReadable.md) |
| `FileReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [FileReader](./reader/FileReader.md) |
| `FileStreamReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [FileStreamReader](./reader/FileStreamReader.md) |
| `ICompressedReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [ICompressedReader](./reader/ICompressedReader.md) |
| `BufferReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [BufferReader](./reader/BufferReader.md) |
| `LZ4Reader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [LZ4Reader](./reader/LZ4Reader.md) |
| `ZStdReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [ZStdReader](./reader/ZStdReader.md) |
| `RecordReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [RecordReader](./reader/RecordReader.md) |
| `TypedChunkReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [TypedChunkReader](./reader/TypedChunkReader.md) |
| `TypedRecordReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [TypedRecordReader](./reader/TypedRecordReader.md) |
| `IndexedMessageReader` | 読み込み機能 (`McapReader`) | `reader.hpp` | [IndexedMessageReader](./reader/IndexedMessageReader.md) |
| `LinearMessageView` | 読み込み機能 (`McapReader`) | `reader.hpp` | [LinearMessageView](./reader/LinearMessageView.md) |
| `Record` | データモデル | `types.hpp` | [Record](./types/Record.md) |
| `Header` | データモデル | `types.hpp` | [Header](./types/Header.md) |
| `Footer` | データモデル | `types.hpp` | [Footer](./types/Footer.md) |
| `Schema` | データモデル | `types.hpp` | [Schema](./types/Schema.md) |
| `Channel` | データモデル | `types.hpp` | [Channel](./types/Channel.md) |
| `Message` | データモデル | `types.hpp` | [Message](./types/Message.md) |
| `Chunk` | データモデル | `types.hpp` | [Chunk](./types/Chunk.md) |
| `MessageIndex` | データモデル | `types.hpp` | [MessageIndex](./types/MessageIndex.md) |
| `ChunkIndex` | データモデル | `types.hpp` | [ChunkIndex](./types/ChunkIndex.md) |
| `Attachment` | データモデル | `types.hpp` | [Attachment](./types/Attachment.md) |
| `AttachmentIndex` | データモデル | `types.hpp` | [AttachmentIndex](./types/AttachmentIndex.md) |
| `Statistics` | データモデル | `types.hpp` | [Statistics](./types/Statistics.md) |
| `Metadata` | データモデル | `types.hpp` | [Metadata](./types/Metadata.md) |
| `MetadataIndex` | データモデル | `types.hpp` | [MetadataIndex](./types/MetadataIndex.md) |
| `SummaryOffset` | データモデル | `types.hpp` | [SummaryOffset](./types/SummaryOffset.md) |
| `DataEnd` | データモデル | `types.hpp` | [DataEnd](./types/DataEnd.md) |
| `MessageView` | データモデル | `types.hpp` | [MessageView](./types/MessageView.md) |
| `Status` | 基本型とユーティリティ | `errors.hpp` | [Status](./errors/Status.md) |
| `StatusCode` | 基本型とユーティリティ | `errors.hpp` | [StatusCode](./errors/StatusCode.md) |
| `OpCode` | 基本型とユーティリティ | `types.hpp` | [OpCode](./types/OpCode.md) |
| `Compression` | 基本型とユーティリティ | `types.hpp` | [Compression](./types/Compression.md) |
| `CompressionLevel` | 基本型とユーティリティ | `types.hpp` | [CompressionLevel](./types/CompressionLevel.md) |
| `ReadSummaryMethod` | 基本型とユーティリティ | `reader.hpp` | [ReadSummaryMethod](./reader/ReadSummaryMethod.md) |
| `CRC32Table` | 低レベルインターフェースと拡張性 | `crc32.hpp` | [CRC32Table](./crc32/CRC32Table.md) |
| `Interval` | 低レベルインターフェースと拡張性 | `intervaltree.hpp` | [Interval](./intervaltree/Interval.md) |
| `IntervalTree` | 低レベルインターフェースと拡張性 | `intervaltree.hpp` | [IntervalTree](./intervaltree/IntervalTree.md) |
| `ReadMessageJob` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` | [ReadMessageJob](./read_job_queue/ReadMessageJob.md) |
| `DecompressChunkJob` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` | [DecompressChunkJob](./read_job_queue/DecompressChunkJob.md) |
| `ReadJob` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` | [ReadJob](./read_job_queue/ReadJob.md) |
| `ReadJobQueue` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` | [ReadJobQueue](./read_job_queue/ReadJobQueue.md) |

# C++ MCAPライブラリ 内部ユーティリティ関数一覧

| 関数/定数名 | 機能カテゴリ | ヘッダーファイル名 | 詳細 |
|---|---|---|---|
| `MinHeaderLength` | Internalユーティリティ | `internal.hpp` | [MinHeaderLength](./internal/MinHeaderLength.md) |
| `FooterLength` | Internalユーティリティ | `internal.hpp` | [FooterLength](./internal/FooterLength.md) |
| `ToHex` | Internalユーティリティ | `internal.hpp` | [ToHex](./internal/ToHex.md) |
| `StrCat` | Internalユーティリティ | `internal.hpp` | [StrCat](./internal/StrCat.md) |
| `KeyValueMapSize` | Internalユーティリティ | `internal.hpp` | [KeyValueMapSize](./internal/KeyValueMapSize.md) |
| `CompressionString` | Internalユーティリティ | `internal.hpp` | [CompressionString](./internal/CompressionString.md) |
| `ParseUint16` | Internalユーティリティ | `internal.hpp` | [ParseUint16](./internal/ParseUint16.md) |
| `ParseUint32` | Internalユーティリティ | `internal.hpp` | [ParseUint32](./internal/ParseUint32.md) |
| `ParseUint64` | Internalユーティリティ | `internal.hpp` | [ParseUint64](./internal/ParseUint64.md) |
| `ParseStringView` | Internalユーティリティ | `internal.hpp` | [ParseStringView](./internal/ParseStringView.md) |
| `ParseString` | Internalユーティリティ | `internal.hpp` | [ParseString](./internal/ParseString.md) |
| `ParseByteArray` | Internalユーティリティ | `internal.hpp` | [ParseByteArray](./internal/ParseByteArray.md) |
| `ParseKeyValueMap` | Internalユーティリティ | `internal.hpp` | [ParseKeyValueMap](./internal/ParseKeyValueMap.md) |
| `MagicToHex` | Internalユーティリティ | `internal.hpp` | [MagicToHex](./internal/MagicToHex.md) |


