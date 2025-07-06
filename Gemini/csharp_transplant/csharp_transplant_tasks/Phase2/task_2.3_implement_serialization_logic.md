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