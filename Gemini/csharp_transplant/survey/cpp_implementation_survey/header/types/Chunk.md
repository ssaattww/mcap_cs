# Chunk

`Chunk` 構造体は、スキーマ、チャンネル、メッセージのコレクションを圧縮およびインデックス化をサポートする形でまとめたものです。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC Chunk {
  Timestamp messageStartTime;
  Timestamp messageEndTime;
  ByteOffset uncompressedSize;
  uint32_t uncompressedCrc;
  std::string compression;
  ByteOffset compressedSize;
  const std::byte* records = nullptr;
};
```

**メンバー:**

*   `messageStartTime`: チャンク内のメッセージの最小ログタイムスタンプ。
*   `messageEndTime`: チャンク内のメッセージの最大ログタイムスタンプ。
*   `uncompressedSize`: 非圧縮時のチャンクデータのサイズ（バイト単位）。
*   `uncompressedCrc`: 非圧縮チャンクデータのCRC32値。
*   `compression`: チャンクに適用された圧縮アルゴリズム（例: `"zstd"`, `"lz4"`, `""`）。
*   `compressedSize`: 圧縮後のチャンクデータのサイズ（バイト単位）。
*   `records`: チャンク内のレコードデータへのポインタ。
