# タスク 3.6: メッセージ読み込みとイテレーション機能の実装

GitHub Issue: #16

## 概要
`readMessages()` を実装し、MCAPファイル内のメッセージを反復処理するための機能を提供します。時間範囲フィルタやトピックフィルタリング機能を含めます。

## 手順
1.  `McapReader` クラスに `ReadMessages()` メソッドを実装します。これは `IEnumerable<MessageView>` を返すようにします。
2.  `ReadMessageOptions` を引数として受け取り、時間範囲 (`startTime`, `endTime`) やトピックフィルタ (`topicFilter`) を適用できるようにします。
3.  インデックス情報（特に `MessageIndex` と `ChunkIndex`）を活用して、効率的にメッセージをシークし、読み込みます。
4.  メッセージのソート順序（`FileOrder`, `LogTimeOrder`, `ReverseLogTimeOrder`）をサポートします。
5.  `MessageView` クラスを完成させ、メッセージデータとその関連情報（スキーマ、チャンネル）への参照を保持できるようにします。

## 完了条件
-   `ReadMessages()` メソッドが正しく機能し、メッセージを効率的にイテレーションできる。
-   時間範囲フィルタとトピックフィルタが適用できる。
-   メッセージのソート順序が選択できる。
-   プロジェクトが正常にビルドできる。

## 参考 (C++)
-   `mcap/reader.hpp` の `McapReader` の `readMessages()`, `LinearMessageView`, `IndexedMessageReader`