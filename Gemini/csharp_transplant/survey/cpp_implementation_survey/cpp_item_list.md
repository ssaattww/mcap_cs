# C++ MCAPライブラリ クラス・構造体一覧

| クラス/構造体名 | 機能カテゴリ | ヘッダーファイル名 | 詳細 |
|---|---|---|---|
| `McapWriter` | 書き込み機能 (`McapWriter`) | `writer.hpp` | [McapWriter](./header/writer/McapWriter.md) |
| `McapWriterOptions` | 基本型とユーティリティ | `writer.hpp` | [McapWriterOptions](./header/writer/McapWriterOptions.md) |
| `IWritable` | 低レベルインターフェースと拡張性 | `writer.hpp` | [IWritable](./header/writer/IWritable.md) |
| `FileWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` | [FileWriter](./header/writer/FileWriter.md) |
| `StreamWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` | [StreamWriter](./header/writer/StreamWriter.md) |
| `IChunkWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` | [IChunkWriter](./header/writer/IChunkWriter.md) |
| `BufferWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` | [BufferWriter](./header/writer/BufferWriter.md) |
| `LZ4Writer` | 低レベルインターフェースと拡張性 | `writer.hpp` | [LZ4Writer](./header/writer/LZ4Writer.md) |
| `ZStdWriter` | 低レベルインターフェースと拡張性 | `writer.hpp` | [ZStdWriter](./header/writer/ZStdWriter.md) |
| `McapReader` | 読み込み機能 (`McapReader`) | `reader.hpp` | [McapReader](./header/reader/McapReader.md) |
| `ReadMessageOptions` | 基本型とユーティリティ | `reader.hpp` | [ReadMessageOptions](./header/reader/ReadMessageOptions.md) |
| `IReadable` | 低レベルインターフェースと拡張性 | `reader.hpp` | [IReadable](./header/reader/IReadable.md) |
| `FileReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [FileReader](./header/reader/FileReader.md) |
| `FileStreamReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [FileStreamReader](./header/reader/FileStreamReader.md) |
| `ICompressedReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [ICompressedReader](./header/reader/ICompressedReader.md) |
| `BufferReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [BufferReader](./header/reader/BufferReader.md) |
| `LZ4Reader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [LZ4Reader](./header/reader/LZ4Reader.md) |
| `ZStdReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [ZStdReader](./header/reader/ZStdReader.md) |
| `RecordReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [RecordReader](./header/reader/RecordReader.md) |
| `TypedChunkReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [TypedChunkReader](./header/reader/TypedChunkReader.md) |
| `TypedRecordReader` | 低レベルインターフェースと拡張性 | `reader.hpp` | [TypedRecordReader](./header/reader/TypedRecordReader.md) |
| `IndexedMessageReader` | 読み込み機能 (`McapReader`) | `reader.hpp` | [IndexedMessageReader](./header/reader/IndexedMessageReader.md) |
| `LinearMessageView` | 読み込み機能 (`McapReader`) | `reader.hpp` | [LinearMessageView](./header/reader/LinearMessageView.md) |
| `Record` | データモデル | `types.hpp` | [Record](./header/types/Record.md) |
| `Header` | データモデル | `types.hpp` | [Header](./header/types/Header.md) |
| `Footer` | データモデル | `types.hpp` | [Footer](./header/types/Footer.md) |
| `Schema` | データモデル | `types.hpp` | [Schema](./header/types/Schema.md) |
| `Channel` | データモデル | `types.hpp` | [Channel](./header/types/Channel.md) |
| `Message` | データモデル | `types.hpp` | [Message](./header/types/Message.md) |
| `Chunk` | データモデル | `types.hpp` | [Chunk](./header/types/Chunk.md) |
| `MessageIndex` | データモデル | `types.hpp` | [MessageIndex](./header/types/MessageIndex.md) |
| `ChunkIndex` | データモデル | `types.hpp` | [ChunkIndex](./header/types/ChunkIndex.md) |
| `Attachment` | データモデル | `types.hpp` | [Attachment](./header/types/Attachment.md) |
| `AttachmentIndex` | データモデル | `types.hpp` | [AttachmentIndex](./header/types/AttachmentIndex.md) |
| `Statistics` | データモデル | `types.hpp` | [Statistics](./header/types/Statistics.md) |
| `Metadata` | データモデル | `types.hpp` | [Metadata](./header/types/Metadata.md) |
| `MetadataIndex` | データモデル | `types.hpp` | [MetadataIndex](./header/types/MetadataIndex.md) |
| `SummaryOffset` | データモデル | `types.hpp` | [SummaryOffset](./header/types/SummaryOffset.md) |
| `DataEnd` | データモデル | `types.hpp` | [DataEnd](./header/types/DataEnd.md) |
| `MessageView` | データモデル | `types.hpp` | [MessageView](./header/types/MessageView.md) |
| `Status` | 基本型とユーティリティ | `errors.hpp` | [Status](./header/errors/Status.md) |
| `StatusCode` | 基本型とユーティリティ | `errors.hpp` | [StatusCode](./header/errors/StatusCode.md) |
| `OpCode` | 基本型とユーティリティ | `types.hpp` | [OpCode](./header/types/OpCode.md) |
| `Compression` | 基本型とユーティリティ | `types.hpp` | [Compression](./header/types/Compression.md) |
| `CompressionLevel` | 基本型とユーティリティ | `types.hpp` | [CompressionLevel](./header/types/CompressionLevel.md) |
| `ReadSummaryMethod` | 基本型とユーティリティ | `reader.hpp` | [ReadSummaryMethod](./header/reader/ReadSummaryMethod.md) |
| `CRC32Table` | 低レベルインターフェースと拡張性 | `crc32.hpp` | [CRC32Table](./header/crc32/CRC32Table.md) |
| `Interval` | 低レベルインターフェースと拡張性 | `intervaltree.hpp` | [Interval](./header/intervaltree/Interval.md) |
| `IntervalTree` | 低レベルインターフェースと拡張性 | `intervaltree.hpp` | [IntervalTree](./header/intervaltree/IntervalTree.md) |
| `ReadMessageJob` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` | [ReadMessageJob](./header/read_job_queue/ReadMessageJob.md) |
| `DecompressChunkJob` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` | [DecompressChunkJob](./header/read_job_queue/DecompressChunkJob.md) |
| `ReadJob` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` | [ReadJob](./header/read_job_queue/ReadJob.md) |
| `ReadJobQueue` | 低レベルインターフェースと拡張性 | `read_job_queue.hpp` | [ReadJobQueue](./header/read_job_queue/ReadJobQueue.md) |

# C++ MCAPライブラリ 内部ユーティリティ関数一覧

| 関数/定数名 | 機能カテゴリ | ヘッダーファイル名 | 詳細 |
|---|---|---|---|
| `MinHeaderLength` | Internalユーティリティ | `internal.hpp` | [MinHeaderLength](./header/internal/MinHeaderLength.md) |
| `FooterLength` | Internalユーティリティ | `internal.hpp` | [FooterLength](./header/internal/FooterLength.md) |
| `ToHex` | Internalユーティリティ | `internal.hpp` | [ToHex](./header/internal/ToHex.md) |
| `StrCat` | Internalユーティリティ | `internal.hpp` | [StrCat](./header/internal/StrCat.md) |
| `KeyValueMapSize` | Internalユーティリティ | `internal.hpp` | [KeyValueMapSize](./header/internal/KeyValueMapSize.md) |
| `CompressionString` | Internalユーティリティ | `internal.hpp` | [CompressionString](./header/internal/CompressionString.md) |
| `ParseUint16` | Internalユーティリティ | `internal.hpp` | [ParseUint16](./header/internal/ParseUint16.md) |
| `ParseUint32` | Internalユーティリティ | `internal.hpp` | [ParseUint32](./header/internal/ParseUint32.md) |
| `ParseUint64` | Internalユーティリティ | `internal.hpp` | [ParseUint64](./header/internal/ParseUint64.md) |
| `ParseStringView` | Internalユーティリティ | `internal.hpp` | [ParseStringView](./header/internal/ParseStringView.md) |
| `ParseString` | Internalユーティリティ | `internal.hpp` | [ParseString](./header/internal/ParseString.md) |
| `ParseByteArray` | Internalユーティリティ | `internal.hpp` | [ParseByteArray](./header/internal/ParseByteArray.md) |
| `ParseKeyValueMap` | Internalユーティリティ | `internal.hpp` | [ParseKeyValueMap](./header/internal/ParseKeyValueMap.md) |
| `MagicToHex` | Internalユーティリティ | `internal.hpp` | [MagicToHex](./header/internal/MagicToHex.md) |



