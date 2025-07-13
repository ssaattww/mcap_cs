# Status

`Status` 構造体は、ステータスコードと追加のコンテキストを運ぶ文字列メッセージをラップします。

**定義:** `cpp/mcap/include/mcap/errors.hpp`

```cpp
struct [[nodiscard]] Status {
  StatusCode code;
  std::string message;

  Status()
      : code(StatusCode::Success) {}

  Status(StatusCode code)
      : code(code) {
    // ... (constructor implementation)
  }

  Status(StatusCode code, const std::string& message)
      : code(code)
      , message(message) {}

  bool ok() const {
    return code == StatusCode::Success;
  }
};
```

**メンバー:**

*   `code`: 操作のステータスを示す `StatusCode`。
*   `message`: エラーに関する追加情報を提供する文字列。

**メソッド:**

*   `Status()`: デフォルトコンストラクタ。`StatusCode::Success` で初期化します。
*   `Status(StatusCode code)`: `StatusCode` を受け取り、対応するデフォルトメッセージで初期化するコンストラクタ。
*   `Status(StatusCode code, const std::string& message)`: `StatusCode` とカスタムメッセージで初期化するコンストラクタ。
*   `ok() const`: ステータスが `StatusCode::Success` の場合に `true` を返します。
