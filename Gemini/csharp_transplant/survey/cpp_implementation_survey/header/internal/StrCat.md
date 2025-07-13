# StrCat

`StrCat` は、複数の引数を連結して `std::string` を作成するインライン関数です。

**定義:** `cpp/mcap/include/mcap/internal.hpp`

```cpp
template <typename... T>
[[nodiscard]] inline std::string StrCat(T&&... args) {
  using mcap::internal::to_string;
  using std::to_string;
  return ("" + ... + to_string(std::forward<T>(args)));
}
```

**概要:**

この関数は、可変長テンプレート引数を受け取り、それぞれを文字列に変換した後、すべてを連結して一つの `std::string` を返します。C++17の畳み込み式 (fold expression) を利用しています。
