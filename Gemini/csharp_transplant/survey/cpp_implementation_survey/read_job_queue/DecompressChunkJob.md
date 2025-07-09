# DecompressChunkJob

`DecompressChunkJob` 構造体は、チャンクを伸長するジョブを表します。

**定義:** `cpp/mcap/include/mcap/read_job_queue.hpp`

```cpp
struct DecompressChunkJob {
  Timestamp messageStartTime;
  Timestamp messageEndTime;
  ByteOffset chunkStartOffset;
  ByteOffset messageIndexEndOffset;
};
```

**メンバー:**

*   `messageStartTime`: チャンク内のメッセージの最小タイムスタンプ。
*   `messageEndTime`: チャンク内のメッセージの最大タイムスタンプ。
*   `chunkStartOffset`: チャンクの開始オフセット。
*   `messageIndexEndOffset`: チャンクに関連するメッセージインデックスレコード群の終了オフセット。
