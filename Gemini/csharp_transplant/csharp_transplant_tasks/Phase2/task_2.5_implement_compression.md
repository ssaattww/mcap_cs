# タスク 2.5: 圧縮機能の実装

## 概要
ZstdおよびLZ4の.NETラッパーライブラリを導入し、チャンクを圧縮して書き込む機能を実装します。

## 手順
1.  `ZstdNet` および `lz4net` (または同等のライブラリ) をNuGetパッケージとしてプロジェクトに追加します。
2.  `McapWriter` クラスに、`McapWriterOptions` の `Compression` および `CompressionLevel` プロパティに基づいてチャンクデータを圧縮するロジックを追加します。
3.  `IChunkWriter` に相当するインターフェースを設計し、圧縮ロジックを抽象化することも検討します。
4.  `forceCompression` オプションが設定されている場合の挙動を実装します。

## 完了条件
-   ZstdおよびLZ4によるチャンクデータの圧縮書き込みが可能になっている。
-   `Compression` および `CompressionLevel` オプションが正しく機能する。
-   プロジェクトが正常にビルドできる。

## 参考 (C++)
-   `mcap/writer.hpp` の `LZ4Writer`, `ZStdWriter`