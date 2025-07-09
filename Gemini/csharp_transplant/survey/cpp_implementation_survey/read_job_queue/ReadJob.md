# ReadJob

`ReadJob` は、インデックス付きMCAPリーダーが実行するジョブの共用体 (union) です。

**定義:** `cpp/mcap/include/mcap/read_job_queue.hpp`

```cpp
using ReadJob = std::variant<ReadMessageJob, DecompressChunkJob>;
```

**概要:**

`ReadJob` は、`std::variant` を使用して、以下の2種類のジョブのいずれかを保持することができます。

*   `ReadMessageJob`: 特定のメッセージを読み取るジョブ。
*   `DecompressChunkJob`: チャンクを伸長するジョブ。

これにより、`ReadJobQueue` は異なる種類の読み取りタスクを単一のキューで管理できます。
