# タスク 2.1: `IWritable` に相当するインターフェースの設計

## 概要
C++の `IWritable` に相当する、C#の `System.IO.Stream` を活用した書き込みインターフェースを設計します。これにより、ファイルだけでなく、メモリやネットワークストリームなど、様々な出力先への書き込みを抽象化します。

## 手順
1.  `Mcap.CSharp/Mcap/` ディレクトリ内に `Interfaces/` ディレクトリを作成します。
2.  `Interfaces/` ディレクトリに `IWritable.cs` ファイルを作成します。
3.  `IWritable` インターフェースを定義します。これは `System.IO.Stream` を継承するか、`Stream` をラップする形で、書き込み操作（`Write`, `Flush`, `Seek` など）を抽象化します。
4.  `FileWriter.cs` や `BufferWriter.cs` など、具体的な `IWritable` の実装クラスの骨格を作成します。

## 完了条件
-   `IWritable` インターフェースが定義されている。
-   `IWritable` を実装する基本的なライタークラスの骨格が作成されている。
-   プロジェクトが正常にビルドできる。

## 参考 (C++)
-   `mcap/writer.hpp` の `IWritable`, `FileWriter`, `StreamWriter`, `BufferWriter`