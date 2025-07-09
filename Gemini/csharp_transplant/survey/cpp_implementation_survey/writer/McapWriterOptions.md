# McapWriterOptions

`McapWriterOptions` 構造体は、`McapWriter` の動作を構成するためのオプションを提供します。

**定義:** `cpp/mcap/include/mcap/writer.hpp`

```cpp
struct MCAP_PUBLIC McapWriterOptions {
  bool noChunkCRC = false;
  bool noAttachmentCRC = false;
  bool enableDataCRC = false;
  bool noSummaryCRC = false;
  bool noChunking = false;
  bool noMessageIndex = false;
  bool noSummary = false;
  uint64_t chunkSize = DefaultChunkSize;
  Compression compression = Compression::Zstd;
  CompressionLevel compressionLevel = CompressionLevel::Default;
  bool forceCompression = false;
  std::string profile;
  std::string library = "libmcap " MCAP_LIBRARY_VERSION;

  // The following options are less commonly used, providing more fine-grained
  // control of index records and the Summary section

  bool noRepeatedSchemas = false;
  bool noRepeatedChannels = false;
  bool noAttachmentIndex = false;
  bool noMetadataIndex = false;
  bool noChunkIndex = false;
  bool noStatistics = false;
  bool noSummaryOffsets = false;

  McapWriterOptions(const std::string_view profile)
      : profile(profile) {}
};
```

**メンバー:**

*   `noChunkCRC`: チャンクのCRC計算を無効にします。
*   `noAttachmentCRC`: アタッチメントのCRC計算を無効にします。
*   `enableDataCRC`: データセクションの全レコードのCRC計算を有効にします。
*   `noSummaryCRC`: サマリーセクションのCRC計算を無効にします。
*   `noChunking`: チャンクを書き込まず、スキーマ、チャンネル、メッセージレコードを直接データセクションに書き込みます。
*   `noMessageIndex`: メッセージインデックスレコードを書き込みません。
*   `noSummary`: サマリーまたはサマリーオフセットセクションを書き込みません。
*   `chunkSize`: ターゲットの非圧縮チャンクペイロードサイズ（バイト単位）。
*   `compression`: チャンク書き込み時に使用する圧縮アルゴリズム。
*   `compressionLevel`: チャンク書き込み時に使用する圧縮レベル。
*   `forceCompression`: 全てのチャンクで強制的に圧縮を使用します。
*   `profile`: レコーディングプロファイル。
*   `library`: レコーディングライブラリの文字列。
*   `noRepeatedSchemas`: スキーマの繰り返し書き込みを無効にします。
*   `noRepeatedChannels`: チャンネルの繰り返し書き込みを無効にします。
*   `noAttachmentIndex`: アタッチメントインデックスを書き込みません。
*   `noMetadataIndex`: メタデータインデックスを書き込みません。
*   `noChunkIndex`: チャンクインデックスを書き込みません。
*   `noStatistics`: 統計情報を書き込みません。
*   `noSummaryOffsets`: サマリーオフセットを書き込みません。
