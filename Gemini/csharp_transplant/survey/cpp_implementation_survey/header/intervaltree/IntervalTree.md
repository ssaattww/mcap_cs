# IntervalTree

`IntervalTree` は、区間を効率的に格納し、特定の点や範囲に重複する区間を検索するためのデータ構造です。

**定義:** `cpp/mcap/include/mcap/intervaltree.hpp`

```cpp
template <class Scalar, class Value>
class IntervalTree {
public:
  using interval = Interval<Scalar, Value>;
  using interval_vector = std::vector<interval>;

  IntervalTree(interval_vector&& ivals, std::size_t depth = 16, std::size_t minbucket = 64,
               std::size_t maxbucket = 512, Scalar leftextent = 0, Scalar rightextent = 0);

  template <class UnaryFunction>
  void visit_overlapping(const Scalar& start, const Scalar& stop, UnaryFunction f) const;

  interval_vector find_overlapping(const Scalar& start, const Scalar& stop) const;

  // Other methods...

private:
  interval_vector intervals;
  std::unique_ptr<IntervalTree> left;
  std::unique_ptr<IntervalTree> right;
  Scalar center;
};
```

**概要:**

このクラスは、区間のセットをツリー構造に整理します。各ノードは中心点 (`center`) を持ち、その中心点と重なる区間を `intervals` ベクタに格納します。中心点より完全に左にある区間は `left` サブツリーに、完全に右にある区間は `right` サブツリーに再帰的に格納されます。

**主要なメンバー:**

*   `IntervalTree(interval_vector&& ivals, ...)`: 区間のベクタから区間ツリーを構築するコンストラクタ。
*   `visit_overlapping(const Scalar& start, const Scalar& stop, UnaryFunction f) const`: 指定された範囲 `[start, stop]` と重なるすべての区間に対して、関数 `f` を適用します。
*   `find_overlapping(const Scalar& start, const Scalar& stop) const`: 指定された範囲と重なるすべての区間をベクタとして返します。

MCAPライブラリ内では、`McapReader` がチャンクインデックス (`ChunkIndex`) をこの `IntervalTree` に格納し、特定の時間範囲 (`[startTime, endTime]`) に含まれるメッセージを持つチャンクを効率的に見つけるために使用されます。この場合、`Scalar` は `Timestamp`、`Value` は `ChunkIndex` となります。
