# タスク 1.2: 基本的な型と定数の定義

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

## 参考 (C++)

-   `mcap/types.hpp`
-   `mcap/errors.hpp`
-   `mcap/reader.hpp`
-   `mcap/writer.hpp`
