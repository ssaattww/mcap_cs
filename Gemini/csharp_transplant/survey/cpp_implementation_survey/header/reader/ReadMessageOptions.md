# ReadMessageOptions

`ReadMessageOptions` 構造体は、MCAPファイルからメッセージを読み込む際のオプションを定義します。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
struct MCAP_PUBLIC ReadMessageOptions {
public:
  Timestamp startTime = 0;
  Timestamp endTime = MaxTime;
  std::function<bool(std::string_view)> topicFilter;
  enum struct ReadOrder { FileOrder, LogTimeOrder, ReverseLogTimeOrder };
  ReadOrder readOrder = ReadOrder::FileOrder;

  ReadMessageOptions(Timestamp start, Timestamp end)
      : startTime(start)
      , endTime(end) {}

  ReadMessageOptions() = default;

  Status validate() const;
};
```

**メンバー:**

*   `startTime`: メッセージのログタイムスタンプの開始時刻（ナノ秒単位）。この時刻以降のメッセージが含まれます。
*   `endTime`: メッセージのログタイムスタンプの終了時刻（ナノ秒単位）。この時刻より前のメッセージが含まれます。
*   `topicFilter`: オプションのトピックフィルター関数。指定された場合、MCAPファイル内のすべてのトピックに対して呼び出され、`true` を返したチャンネルのメッセージのみが含まれます。
*   `readOrder`: メッセージが返される順序を定義する `ReadOrder` 列挙型。
    *   `FileOrder`: ファイル内の出現順。
    *   `LogTimeOrder`: ログタイムスタンプの昇順。
    *   `ReverseLogTimeOrder`: ログタイムスタンプの降順。

**メソッド:**

*   `validate() const`: 設定が有効かどうかを検証します。
