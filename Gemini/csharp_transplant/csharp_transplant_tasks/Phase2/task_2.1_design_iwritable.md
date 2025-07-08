# タスク 2.1: `IWritable` に相当するインターフェースの設計

GitHub Issue: #5

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

## 未実装項目 (サブIssue)

-   `Crc()` メソッドの実装 (Issue #57)
-   `ResetCrc()` メソッドの実装 (Issue #58)
-   `Flush()` メソッドの実装 (Issue #59)

## 参考 (C++)
-   `mcap/writer.hpp` の `IWritable`, `FileWriter`, `StreamWriter`, `BufferWriter`

## 作業状況

### 手順1: `Mcap.CSharp/Mcap/` ディレクトリ内に `Interfaces/` ディレクトリを作成

- `csharp/Mcap.CSharp/Mcap/Interfaces` ディレクトリを作成しました。

### 手順2: `Interfaces/` ディレクトリに `IWritable.cs` ファイルを作成

- `IWritable.cs` ファイルを作成し、C++の `mcap::IWritable` インターフェースを参考に、`CrcEnabled` プロパティと `Write`, `End`, `Size`, `Crc`, `ResetCrc`, `Flush` メソッドを定義しました。

### 手順3: `IWritable` インターフェースを定義

- `IWritable.cs` に `IWritable` インターフェースを定義しました。

### 手順4: `FileWriter.cs` や `BufferWriter.cs` など、具体的な `IWritable` の実装クラスの骨格を作成

- `FileWriter.cs` と `BufferWriter.cs` の骨格を作成しました。
- `InterfacesTests.cs` を作成し、`FileWriter` と `BufferWriter` のインスタンス化テストを追加しました。
- `FileWriter` の `Write`, `End`, `Size` メソッドを実装し、テストが成功することを確認しました。
- `BufferWriter` の `Write`, `End`, `Size` メソッドを実装し、テストが成功することを確認しました。
- `Crc()`, `ResetCrc()`, `Flush()` メソッドは、現時点ではダミー実装です。
