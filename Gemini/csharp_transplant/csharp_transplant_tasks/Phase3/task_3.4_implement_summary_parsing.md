# タスク 3.4: サマリー解析機能の実装

## 概要
`readSummary()` を実装し、MCAPファイルの末尾にあるサマリーセクションを解析して、インデックス情報をメモリにロードする機能を実装します。これにより、メッセージへの高速なランダムアクセスが可能になります。

## 手順
1.  `McapReader` クラスに `ReadSummary()` メソッドを実装します。
2.  フッターからサマリーセクションのオフセットを読み取り、その位置にシークします。
3.  サマリーセクション内の `Schema`, `Channel`, `MessageIndex`, `ChunkIndex`, `AttachmentIndex`, `MetadataIndex`, `Statistics`, `SummaryOffset` などのレコードを読み込み、メモリ上の適切なデータ構造に格納します。
4.  `ReadSummaryMethod` オプションを考慮し、サマリーの読み込み方法を制御できるようにします。

## 完了条件
-   `ReadSummary()` メソッドが正しく機能し、サマリー情報がメモリにロードされる。
-   各種インデックス情報にアクセスできる。
-   プロジェクトが正常にビルドできる。

## 参考 (C++)
-   `mcap/reader.hpp` の `McapReader` のサマリー解析関連ロジック