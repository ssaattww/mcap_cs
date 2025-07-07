# タスク 1.2: 基本的な型と定数の定義

GitHub Issue: #3

## 概要

MCAPライブラリ全体で共通して使用される基本的な列挙型、定数、および単純なデータ構造を定義します。

## 手順

1.  **`Mcap/` ディレクトリの作成**:
    -   `Mcap.CSharp` プロジェクト内に、`Mcap` という名前のディレクトリを作成します。

2.  **`Enums.cs` の作成**:
    -   `Mcap/` ディレクトリに `Enums.cs` ファイルを作成します。
    -   以下の列挙型を定義します:
        -   `public enum OpCode : byte { ... }`
        -   `public enum Compression { None, Lz4, Zstd }`

3.  **`Types.cs` の作成**:
    -   `Mcap/` ディレクトリに `Types.cs` ファイルを作成します。
    -   以下のクラスまたは構造体を定義します:
        -   `public readonly struct Status { ... }`
        -   `public class McapWriterOptions { ... }`
        -   `public class ReadMessageOptions { ... }`
        -   `public class MessageView { ... }`

4.  **定数の定義**:
    -   必要に応じて、マジックナンバーなどを定義する `Constants.cs` を作成します。

## 完了条件

-   `OpCode` と `Compression` 列挙型が定義されている。
-   `Status`, `McapWriterOptions`, `ReadMessageOptions`, `MessageView` の基本的なクラス/構造体が定義されている。
-   プロジェクトが正常にビルドできる。

## 未実装項目 (サブIssue)

-   `Status` 構造体の実装 (Issue #39)
-   `ReadMessageOptions` クラスの実装 (Issue #40)
-   `MessageView` クラスの実装 (Issue #41)

## 参考 (C++)

-   `mcap/types.hpp`
-   `mcap/errors.hpp`
-   `mcap/reader.hpp`
-   `mcap/writer.hpp`

## 作業状況

### 手順1: `Mcap/` ディレクトリの作成

- `Mcap.CSharp` プロジェクト内に `Mcap` ディレクトリを作成しました。

### 手順2: `Enums.cs` の作成

- `Mcap/` ディレクトリに `Enums.cs` ファイルを作成し、`OpCode`, `Compression`, `CompressionLevel` 列挙型を定義しました。
- TDDの原則に従い、`OpCode` の値を検証するテスト (`BasicTypesTests.cs`) を作成し、成功することを確認しました。

### 手順3: `Types.cs` の作成

- `Mcap/` ディレクトリに `Types.cs` ファイルを作成し、`Status`, `McapWriterOptions`, `ReadMessageOptions`, `MessageView` の骨格を定義しました。
- TDDの原則に従い、C++の実装を参考に `McapWriterOptions` の以下のプロパティを実装しました。
    - `Profile` (string)
    - `ChunkSize` (ulong)
    - `Compression` (enum)
    - `CompressionLevel` (enum)
    - `NoChunking` (bool)
    - `ForceCompression` (bool)
- 各プロパティに対応するテストを `TypesTests.cs` に追加し、すべて成功することを確認しました。
- `McapWriterOptions` の定義に、移植元であるC++の構造体名をコメントとして追記しました。

### 手順4: 定数の定義

- `Constants.cs` ファイルを作成し、MCAPの主要な定数（`LibraryVersion`, `SpecVersion`, `Magic`, `DefaultChunkSize`, `EndOffset`, `MaxTime`）を定義しました。
- 各定数に対応するテストを `ConstantsTests.cs` に追加し、すべて成功することを確認しました。
