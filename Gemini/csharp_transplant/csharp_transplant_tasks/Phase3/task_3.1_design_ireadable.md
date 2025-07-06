# タスク 3.1: `IReadable` に相当するインターフェースの設計

## 概要
C++の `IReadable` に相当する、C#の `System.IO.Stream` を活用した読み込みインターフェースを設計します。これにより、ファイルだけでなく、メモリやネットワークストリームなど、様々な入力元からの読み込みを抽象化します。

## 手順
1.  `Mcap.CSharp/Mcap/Interfaces/` ディレクトリに `IReadable.cs` ファイルを作成します。
2.  `IReadable` インターフェースを定義します。これは `System.IO.Stream` を継承するか、`Stream` をラップする形で、読み込み操作（`Read`, `Seek` など）を抽象化します。
3.  `FileReader.cs` や `BufferReader.cs` など、具体的な `IReadable` の実装クラスの骨格を作成します。

## 完了条件
-   `IReadable` インターフェースが定義されている。
-   `IReadable` を実装する基本的なリーダークラスの骨格が作成されている。
-   プロジェクトが正常にビルドできる。

## 参考 (C++)
-   `mcap/reader.hpp` の `IReadable`, `FileReader`, `FileStreamReader`, `BufferReader`