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

## 参考 (C++)

-   `mcap/types.hpp`
-   `Gemini/cpp/cpp_item_list.md`
