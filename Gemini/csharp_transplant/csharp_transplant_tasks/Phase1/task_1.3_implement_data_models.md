# タスク 1.3: データモデルのクラス実装

GitHub Issue: #4

## 概要

MCAPフォーマットの各レコードタイプに対応するC#のクラスを実装します。この段階では、プロパティの定義を主に行い、シリアライズ/デシリアライズのロジックは含めません。

## 手順

1.  **`Records/` ディレクトリの作成**:
    -   `Mcap.CSharp/Mcap/` ディレクトリ内に `Records` という名前のディレクトリを作成します。

2.  **各レコードクラスの作成**:
    -   `Records/` ディレクトリに、以下の各レコードに対応するC#クラスファイルを作成します。
        -   `Header.cs`
        -   `Footer.cs`
        -   `Schema.cs`
        -   `Channel.cs`
        -   `Message.cs`
        -   `Chunk.cs`
        -   `MessageIndex.cs`
        -   `ChunkIndex.cs`
        -   `Attachment.cs`
        -   `AttachmentIndex.cs`
        -   `Statistics.cs`
        -   `Metadata.cs`
        -   `MetadataIndex.cs`
        -   `SummaryOffset.cs`
        -   `DataEnd.cs`

3.  **プロパティの定義**:
    -   C++の `types.hpp` を参考に、各クラスにMCAP仕様で定められたフィールドに対応するプロパティを定義します。
    -   C#の命名規則（パスカルケース）に従います。
    -   データ型は、C#の適切な型（例: `ulong` for `uint64_t`, `string` for `std::string_view`, `IReadOnlyDictionary<string, string>` for `std::map`）を選択します。

## 完了条件

-   すべてのMCAPレコードに対応するC#クラスが作成されている。
-   各クラスには、仕様に基づいたプロパティが定義されている。
-   プロジェクトが正常にビルドできる。

## 未実装項目 (サブIssue)


## 参考 (C++)

-   `mcap/types.hpp`
-   `Gemini/cpp/cpp_item_list.md`

## 作業状況

### 1. `Records/` ディレクトリの作成

- `Mcap.CSharp/Mcap/` ディレクトリ内に `Records` ディレクトリを作成しました。

### 2. 各レコードクラスの作成

- `Records/` ディレクトリに、MCAPの各レコードに対応するC#クラスファイル（`Header.cs`, `Footer.cs`, `Schema.cs`, `Channel.cs`, `Message.cs`, `Chunk.cs`, `MessageIndex.cs`, `ChunkIndex.cs`, `Attachment.cs`, `AttachmentIndex.cs`, `Statistics.cs`, `Metadata.cs`, `MetadataIndex.cs`, `SummaryOffset.cs`, `DataEnd.cs`）を作成しました。この段階では、プロパティはTBDコメントのままです。
- 各クラスの定義に、移植元であるC++の構造体名をコメントとして追記しました。

### 3. プロパティの定義 (TDDによる実装)

- `RecordsTests.cs` テストファイルを作成し、`Header` および `Footer` クラスの基本的なインスタンス化テストを追加しました。
- `Header` クラスにC++の `mcap::Header` 構造体を参考に `Profile` (string) と `Library` (string) プロパティを実装しました。
- `Footer` クラスにC++の `mcap::Footer` 構造体を参考に `SummaryStart` (ulong), `SummaryOffsetStart` (ulong), `SummaryCrc` (uint) プロパティを実装しました。
- `Schema` クラスにC++の `mcap::Schema` 構造体を参考に `Id` (ushort), `Name` (string), `Encoding` (string), `Data` (byte[]) プロパティを実装しました。
- `Channel` クラスにC++の `mcap::Channel` 構造体を参考に `Id` (ushort), `Topic` (string), `MessageEncoding` (string), `SchemaId` (ushort), `Metadata` (IReadOnlyDictionary<string, string>) プロパティを実装しました。
- `Message` クラスにC++の `mcap::Message` 構造体を参考に `ChannelId` (ushort), `Sequence` (uint), `LogTime` (ulong), `PublishTime` (ulong), `Data` (byte[]) プロパティを実装しました。
- `Chunk` クラスにC++の `mcap::Chunk` 構造体を参考に `MessageStartTime` (ulong), `MessageEndTime` (ulong), `UncompressedSize` (ulong), `UncompressedCrc` (uint), `Compression` (string), `CompressedSize` (ulong), `Records` (byte[]) プロパティを実装しました。
- `MessageIndex` クラスにC++の `mcap::MessageIndex` 構造体を参考に `ChannelId` (ushort), `Records` (List<Tuple<ulong, ulong>>) プロパティを実装しました。
- `ChunkIndex` クラスにC++の `mcap::ChunkIndex` 構造体を参考に `MessageStartTime` (ulong), `MessageEndTime` (ulong), `ChunkStartOffset` (ulong), `ChunkLength` (ulong), `MessageIndexOffsets` (IReadOnlyDictionary<ushort, ulong>), `MessageIndexLength` (ulong), `Compression` (string), `CompressedSize` (ulong), `UncompressedSize` (ulong) プロパティを実装しました。
- `Attachment` クラスにC++の `mcap::Attachment` 構造体を参考に `LogTime` (ulong), `CreateTime` (ulong), `Name` (string), `MediaType` (string), `Data` (byte[]), `Crc` (uint) プロパティを実装しました。
- `AttachmentIndex` クラスにC++の `mcap::AttachmentIndex` 構造体を参考に `Offset` (ulong), `Length` (ulong), `LogTime` (ulong), `CreateTime` (ulong), `DataSize` (ulong), `Name` (string), `MediaType` (string) プロパティを実装しました。
- `Statistics` クラスにC++の `mcap::Statistics` 構造体を参考に `MessageCount` (ulong), `SchemaCount` (ushort), `ChannelCount` (uint), `AttachmentCount` (uint), `MetadataCount` (uint), `ChunkCount` (uint), `MessageStartTime` (ulong), `MessageEndTime` (ulong), `ChannelMessageCounts` (IReadOnlyDictionary<ushort, ulong>) プロパティを実装しました。
- `Metadata` クラスにC++の `mcap::Metadata` 構造体を参考に `Name` (string), `MetadataMap` (IReadOnlyDictionary<string, string>) プロパティを実装しました。
- `MetadataIndex` クラスにC++の `mcap::MetadataIndex` 構造体を参考に `Offset` (ulong), `Length` (ulong), `Name` (string) プロパティを実装しました。
- `SummaryOffset` クラスにC++の `mcap::SummaryOffset` 構造体を参考に `GroupOpCode` (OpCode), `GroupStart` (ulong), `GroupLength` (ulong) プロパティを実装しました。
- `DataEnd` クラスにC++の `mcap::DataEnd` 構造体を参考に `DataSectionCrc` (uint) プロパティを実装しました。
- 各プロパティに対応するテストを `RecordsTests.cs` に追加し、すべて成功することを確認しました。