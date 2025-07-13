# LinearMessageView

`LinearMessageView` は、MCAPファイル内のメッセージのイテレート可能なビューを提供します。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
struct MCAP_PUBLIC LinearMessageView {
  struct MCAP_PUBLIC Iterator {
    using iterator_category = std::input_iterator_tag;
    using difference_type = int64_t;
    using value_type = MessageView;
    using pointer = const MessageView*;
    using reference = const MessageView&;

    reference operator*() const;
    pointer operator->() const;
    Iterator& operator++();
    void operator++(int);
    MCAP_PUBLIC friend bool operator==(const Iterator& a, const Iterator& b);
    MCAP_PUBLIC friend bool operator!=(const Iterator& a, const Iterator& b);

  private:
    friend LinearMessageView;

    Iterator() = default;
    Iterator(LinearMessageView& view);

    class Impl;
    std::unique_ptr<Impl> impl_;
  };

  LinearMessageView(McapReader& mcapReader, const ProblemCallback& onProblem);
  LinearMessageView(McapReader& mcapReader, ByteOffset dataStart, ByteOffset dataEnd,
                    Timestamp startTime, Timestamp endTime, const ProblemCallback& onProblem);
  LinearMessageView(McapReader& mcapReader, const ReadMessageOptions& options, ByteOffset dataStart,
                    ByteOffset dataEnd, const ProblemCallback& onProblem);

  Iterator begin();
  Iterator end();

private:
  McapReader& mcapReader_;
  ByteOffset dataStart_;
  ByteOffset dataEnd_;
  ReadMessageOptions readMessageOptions_;
  const ProblemCallback onProblem_;
};
```

**メンバー:**

*   `Iterator`: メッセージをイテレートするための内部イテレータクラス。標準の入力イテレータとして動作します。
*   `LinearMessageView(...)`: `McapReader` と読み取りオプションで初期化するコンストラクタ。
*   `begin()`: イテレーションの開始を示すイテレータを返します。
*   `end()`: イテレーションの終了を示すイテレータを返します。

**内部実装:**

`Iterator::Impl` クラスが実際のイテレーションロジックをカプセル化しています。`ReadMessageOptions` の `readOrder` に応じて、`TypedRecordReader`（ファイル順）または `IndexedMessageReader`（ログ時刻順）のいずれかを使用してメッセージを読み込みます。
