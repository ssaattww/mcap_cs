# タスク 2.3: シリアライズロジックの実装

GitHub Issue: #7

## 概要
各データモデルクラス（`Header`, `Schema`, `Channel`, `Message` など）をMCAP仕様に従ってバイト配列に変換するシリアライズロジックを実装します。

## 手順
1.  `Mcap.CSharp/Mcap/Records/` ディレクトリ内の各レコードクラスに、バイト配列へのシリアライズを行うメソッド（例: `WriteTo(Stream stream)` や `ToByteArray()`）を追加します。
2.  MCAP仕様のエンディアンネス（リトルエンディアン）とデータ型（`uint16`, `uint32`, `uint64` など）に注意して実装します。
3.  文字列やマップなどの可変長データのエンコード方法も考慮します。
4.  `McapWriter` クラスの各 `Write` メソッドから、これらのシリアライズロジックを呼び出すように修正します。

## 完了条件
-   すべての主要なレコードクラスが、MCAP仕様に準拠したバイト配列へのシリアライズ機能を備えている。
-   `McapWriter` がこれらのシリアライズ機能を利用してデータを書き込める。
-   プロジェクトが正常にビルドできる。

## 参考 (C++)
-   `mcap/internal.hpp` の `ParseUint16`, `ParseUint32`, `ParseUint64`, `ParseStringView`, `ParseKeyValueMap` など、および各レコードのシリアライズロジック

#### 作業状況
- [x] `feature/csharp_task_2.3`ブランチの作成
- [x] `Header` クラスのシリアライズロジックの実装
    - `Header.cs` に `Write(BinaryWriter writer)` メソッドを実装。
    - `IWritable` インターフェースに `Write(BinaryWriter writer)` メソッドを追加。
    - `Header.cs` に `IWritable` インターフェースを実装。
    - `McapWriter.cs` の `WriteRecord` メソッドから `Header` のシリアライズロジックを削除し、`record.Write(recordBinaryWriter)` を呼び出すように修正。
    - `Header.cs` に C++ の対応する構造体 (`mcap::Header`) とヘッダーファイル (`cpp/mcap/include/mcap/types.hpp`) のコメントを追加。
- [x] `Schema` クラスのシリアライズロジックの実装
    - `Schema.cs` に `Write(BinaryWriter writer)` メソッドを実装。
    - `Schema.cs` に `IWritable` インターフェースを実装。
    - `Schema.cs` に C++ の対応する構造体 (`mcap::Schema`) とヘッダーファイル (`cpp/mcap/include/mcap/types.hpp`) のコメントを追加。
- [x] `Channel` クラスのシリアライズロジックの実装
    - `Channel.cs` に `Write(BinaryWriter writer)` メソッドを実装。
    - `Channel.cs` に `IWritable` インターフェースを実装。
    - `Channel.cs` に C++ の対応する構造体 (`mcap::Channel`) とヘッダーファイル (`cpp/mcap/include/mcap/types.hpp`) のコメントを追加。
- [x] `Message` クラスのシリアライズロジックの実装
    - `Message.cs` に `Write(BinaryWriter writer)` メソッドを実装。
    - `Message.cs` に `IWritable` インターフェースを実装。
    - `Message.cs` に C++ の対応する構造体 (`mcap::Message`) とヘッダーファイル (`cpp/mcap/include/mcap/types.hpp`) のコメントを追加。
- [ ] その他のレコードクラスのシリアライズロジックの実装
- [x] `McapWriter` からシリアライズロジックを呼び出すように修正

## 発生した問題と解決策

### 1. `IWritable.Write` メソッドの引数不一致
- **問題:** `McapWriter.WriteMagic` メソッドで `_writer.Write(Constants.Magic)` を呼び出した際、`IWritable.Write` が `byte[] data, ulong size` の2つの引数を期待しているにもかかわらず、`size` 引数が渡されていなかったためコンパイルエラーが発生しました。
- **原因:** `IWritable.Write` のシグネチャを正しく理解していなかったためです。
- **解決策:** `_writer.Write(Constants.Magic, (ulong)Constants.Magic.Length)` のように、`Constants.Magic` のバイト配列の長さを明示的に `size` 引数として渡すように修正しました。

### 2. `Header` クラスが `IWritable` を実装していない
- **問題:** `McapWriter.WriteRecord<T>(OpCode opCode, T record) where T : IWritable` メソッドに `Header` オブジェクトを渡した際、`Header` クラスが `IWritable` インターフェースを実装していないため、型変換エラーが発生しました。
- **原因:** `WriteRecord` メソッドのジェネリック制約を満たしていなかったためです。
- **解決策:** `csharp/Mcap.CSharp/Mcap/Records/Header.cs` を修正し、`Header` クラスが `IWritable` インターフェースを実装するように変更しました。

### 3. `Header.cs` および `McapWriterTests.cs` の構文エラー
- **問題:** `Header.cs` に `IWritable` メンバーを追加した際、および `McapWriterTests.cs` に新しいテストメソッドを追加した際に、閉じ括弧 `}` が不足しているというコンパイルエラーが発生しました。
- **原因:** コードの追加時に、クラスやメソッドのスコープを閉じる `}` が欠落していました。
- **解決策:** 不足している `}` をそれぞれのファイルに追加し、構文エラーを解消しました。

### 4. `McapWriter.WriteRecord` におけるレコードのシリアライズロジックの誤り
- **問題:** `McapWriter.WriteRecord` メソッド内で、`record.Write` を呼び出すことでレコードのシリアライズを行おうとしましたが、`IWritable.Write` はオブジェクト自体をシリアライズするのではなく、バイト配列を基になるストリームに書き込むためのメソッドでした。このため、`recordBytes` が常に空のバイト配列となり、テストが失敗しました。
- **原因:** `IWritable.Write` の役割と、レコードオブジェクトのシリアライズ方法に関する理解の誤りがありました。
- **解決策:** `McapWriter.WriteRecord` メソッド内で `MemoryStream` と `BinaryWriter` を使用して、`Header` および `Footer` オブジェクトの内容を明示的にシリアライズするように修正しました。これにより、`recordBytes` に正しいシリアライズされたデータが格納されるようになりました。

### 5. `McapWriterTests.cs` における変数名の重複
- **問題:** `McapWriterTests.cs` のテストメソッド内で、`using var writer = new BinaryWriter(stream);` のように `writer` という変数を宣言した際、既に `using var writer = new McapWriter(bufferWriter, options);` で `writer` が定義されていたため、変数名の重複エラーが発生しました。
- **原因:** ローカルスコープ内での変数名の重複です。
- **解決策:** 重複していた変数名を `binaryWriter` および `recordBinaryWriter` に変更し、衝突を解消しました。

### 6. `Footer` クラスのプロパティ名変更によるテストの失敗
- **問題:** `Footer` クラスの `SummaryOffsetStart` プロパティを `SummaryOffset` に変更した際、`RecordsTests.cs` の既存のテストが `SummaryOffsetStart` を参照していたため、コンパイルエラーが発生しました。
- **原因:** プロパティ名の変更が、そのプロパティを使用しているテストコードに反映されていなかったためです。
- **解決策:** `RecordsTests.cs` を修正し、新しいプロパティ名 `SummaryOffset` を使用するように変更しました。

### 7. `IWritable` インターフェースの `Write(BinaryWriter writer)` メソッドの追加と実装
- **問題:** `McapWriter.WriteRecord` メソッドで各レコードクラスの `Write(BinaryWriter writer)` メソッドを呼び出すように修正した際、`IWritable` インターフェースに `Write(BinaryWriter writer)` メソッドが定義されていなかったため、コンパイルエラーが発生しました。
- **原因:** インターフェースの定義と実装の不一致です。
- **解決策:** `IWritable` インターフェースに `Write(BinaryWriter writer)` メソッドを追加し、`Header`, `Footer`, `Schema`, `Channel`, `Message` クラスでこのメソッドを実装しました。また、`BufferWriter` と `FileWriter` の `Write(BinaryWriter writer)` メソッドも `NotImplementedException` をスローするように修正しました。

### 8. `McapWriter.WriteRecord` からの `Write(BinaryWriter writer)` の呼び出し
- **問題:** `McapWriter.WriteRecord` メソッド内で `record.Write(recordBinaryWriter);` を呼び出した際、`IWritable` インターフェースの `Write(byte[] data, ulong size)` メソッドが呼び出されてしまい、期待通りに各レコードクラスの `Write(BinaryWriter writer)` メソッドが呼び出されませんでした。
- **原因:** インターフェースのオーバーロード解決の仕組みによるものです。
- **解決策:** `((dynamic)record).Write(recordBinaryWriter);` のように `dynamic` キーワードを使用して、実行時に適切な `Write` メソッドが呼び出されるように修正しました。