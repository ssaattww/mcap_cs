# タスク 2.2: `McapWriter` クラスの実装 (コアロジック)

GitHub Issue: #6

## 概要
MCAPファイルの書き込みを行う `McapWriter` クラスのコアロジックを実装します。まずは圧縮なし、チャンク化なしの単純な書き込み機能から着手します。

## 手順
1.  `Mcap.CSharp/Mcap/` ディレクトリ内に `McapWriter.cs` ファイルを作成します。
2.  `McapWriter` クラスを定義し、コンストラクタで `IWritable` のインスタンスを受け取るようにします。
3.  `Open()` メソッドを実装し、MCAPヘッダーの書き込みと初期化処理を行います。
4.  `AddSchema(Schema schema)` メソッドを実装し、スキーマレコードを書き込みます。
5.  `AddChannel(Channel channel)` メソッドを実装し、チャンネルレコードを書き込みます。
6.  `Write(Message message)` メソッドを実装し、メッセージレコードを書き込みます。
7.  `Close()` メソッドを実装し、フッターとマジックバイトの書き込みを行います。

## 完了条件
-   `McapWriter` クラスが定義され、基本的な書き込み操作のメソッドが実装されている。
-   ヘッダー、スキーマ、チャンネル、メッセージ、フッターの書き込みが（単純な形式で）可能になっている。
-   プロジェクトが正常にビルドできる。

## 参考 (C++)
-   `mcap/writer.hpp` の `McapWriter`

## 作業状況

### 手順1: `Mcap.CSharp/Mcap/` ディレクトリ内に `McapWriter.cs` ファイルを作成

- `csharp/Mcap.CSharp/Mcap/McapWriter.cs` ファイルを作成し、`McapWriter` クラスのスケルトンを定義しました。

### 手順2: `McapWriter` クラスを定義し、コンストラクタで `IWritable` のインスタンスを受け取るようにします。

- `McapWriter` クラスのコンストラクタで `IWritable` のインスタンスを受け取るように定義しました。

### 手順3: `Open()` メソッドを実装し、MCAPヘッダーの書き込みと初期化処理を行います。

- `WriteMagic()` メソッドを実装し、MCAPマジックバイトを書き込む機能を追加しました。
- `WriteHeader()` メソッドを実装し、MCAPヘッダーレコードを書き込む機能を追加しました。
- `McapWriterTests.cs` に `WriteMagic_WritesCorrectMagicBytes` テストと `WriteHeader_WritesCorrectHeaderRecord` テストを追加し、テストがパスすることを確認しました。

### 手順7: `Close()` メソッドを実装し、フッターとマジックバイトの書き込みを行います。

- `WriteFooter()` メソッドを実装し、MCAPフッターレコードを書き込む機能を追加しました。
- `McapWriter` の `Dispose()` メソッドを実装し、フッターの書き込みとリソースの解放を行うようにしました。
- `McapWriterTests.cs` に `WriteFooter_WritesCorrectFooterRecord` テストを追加し、テストがパスすることを確認しました。

### 共通ロジック

- `WriteRecord<T>(OpCode opCode, T record)` プライベートヘルパーメソッドを実装し、任意の `IWritable` レコードのOpCode、データサイズ、およびレコードデータを書き込む共通ロジックを実装しました。
- `Header` および `Footer` クラスが `IWritable` インターフェースを実装するように修正しました。



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