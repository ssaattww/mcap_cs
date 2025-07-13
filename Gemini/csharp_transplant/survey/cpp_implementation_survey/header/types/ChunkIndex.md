# ChunkIndex

`ChunkIndex` 構造体は、サマリーセクションに存在し、単一のチャンクのサマリー情報を提供し、そのチャンクに関連付けられた各メッセージインデックスレコードを指します。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC ChunkIndex {
  Timestamp messageStartTime;
  Timestamp messageEndTime;
  ByteOffset chunkStartOffset;
  ByteOffset chunkLength;
  std::unordered_map<ChannelId, ByteOffset> messageIndexOffsets;
  ByteOffset messageIndexLength;
  std::string compression;
  ByteOffset compressedSize;
  ByteOffset uncompressedSize;
};
```

**メンバー:**

*   `messageStartTime`: チャンク内のメッセージの最小ログタイムスタンプ。
*   `messageEndTime`: チャンク内のメッセージの最大ログタイムスタンプ。
*   `chunkStartOffset`: ファイル先頭からのチャンクの開始オフセット。
*   `chunkLength`: チャンクの長さ（バイト単位）。
*   `messageIndexOffsets`: チャンネルIDとメッセージインデックスのオフセットのマップ。チャンク内の各チャンネルのメッセージインデックスレコードへのオフセットを示します。
*   `messageIndexLength`: メッセージインデックスの長さ（バイト単位）。
*   `compression`: チャンクに適用された圧縮アルゴリズム。
*   `compressedSize`: 圧縮後のチャンクデータのサイズ（バイト単位）。
*   `uncompressedSize`: 非圧縮時のチャンクデータのサイズ（バイト単位）。
