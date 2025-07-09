# Interval

`Interval` 構造体は、区間ツリー (`IntervalTree`) で使用される半開区間 `[start, stop)` を表します。

**定義:** `cpp/mcap/include/mcap/intervaltree.hpp`

```cpp
template <class Scalar, typename Value>
class Interval {
public:
  Scalar start;
  Scalar stop;
  Value value;
  Interval(const Scalar& s, const Scalar& e, const Value& v)
      : start(std::min(s, e))
      , stop(std::max(s, e))
      , value(v) {}
};
```

**メンバー:**

*   `start`: 区間の開始点。テンプレートパラメータ `Scalar` 型。
*   `stop`: 区間の終了点。テンプレートパラメータ `Scalar` 型。
*   `value`: 区間に関連付けられた値。テンプレートパラメータ `Value` 型。

**コンストラクタ:**

*   `Interval(const Scalar& s, const Scalar& e, const Value& v)`: 開始点 `s`、終了点 `e`、値 `v` で区間を初期化します。`start` には `s` と `e` の小さい方が、`stop` には大きい方が自動的に設定されます。
