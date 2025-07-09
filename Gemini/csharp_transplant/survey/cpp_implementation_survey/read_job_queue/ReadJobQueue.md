# ReadJobQueue

`ReadJobQueue` 構造体は、インデックス付きMCAPリーダーが実行するジョブの優先度付きキューです。

**定義:** `cpp/mcap/include/mcap/read_job_queue.hpp`

```cpp
struct ReadJobQueue {
private:
  bool reverse_ = false;
  std::vector<ReadJob> heap_;

  static Timestamp TimeComparisonKey(const ReadJob& job, bool reverse);
  static RecordOffset PositionComparisonKey(const ReadJob& job, bool reverse);
  static bool CompareForward(const ReadJob& a, const ReadJob& b);
  static bool CompareReverse(const ReadJob& a, const ReadJob& b);

public:
  explicit ReadJobQueue(bool reverse);
  void push(DecompressChunkJob&& decompressChunkJob);
  void push(ReadMessageJob&& readMessageJob);
  ReadJob pop();
  size_t len() const;
};
```

**概要:**

このキューは、内部的にバイナリヒープ (`std::vector` と `std::push_heap`, `std::pop_heap`) を使用して実装されています。ジョブは、タイムスタンプとオフセットに基づいて順序付けられます。

**メンバー:**

*   `ReadJobQueue(bool reverse)`: キューの順序を逆にするかどうかを指定するコンストラクタ。
*   `push(...)`: `DecompressChunkJob` または `ReadMessageJob` をキューに追加します。
*   `pop()`: キューから最も優先度の高いジョブを取り出して返します。
*   `len() const`: キュー内のジョブの数を返します。

**比較ロジック:**

*   `TimeComparisonKey`: ジョブの比較に使用するタイムスタンプキーを返します。`DecompressChunkJob` の場合、順方向では `messageStartTime`、逆方向では `messageEndTime` を使用します。
*   `PositionComparisonKey`: タイムスタンプが同じ場合に、ジョブの比較に使用するオフセットキーを返します。
*   `CompareForward` / `CompareReverse`: ヒープの順序付けに使用される比較関数です。
