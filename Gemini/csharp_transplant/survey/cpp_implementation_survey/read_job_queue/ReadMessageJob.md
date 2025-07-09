# ReadMessageJob

`ReadMessageJob` 構造体は、特定のメッセージを読み取るジョブを表します。

**定義:** `cpp/mcap/include/mcap/read_job_queue.hpp`

```cpp
struct ReadMessageJob {
  Timestamp timestamp;
  RecordOffset offset;
  size_t chunkReaderIndex;
};
```

**メンバー:**

*   `timestamp`: ジョブを他のジョブと相対的に順序付けるためのタイムスタンプ。
*   `offset`: 伸長されたチャンク内の特定のメッセージのオフセット。
*   `chunkReaderIndex`: 伸長されたチャンクが格納されている `chunkReader` のインデックス。
