# Header

`Header` 構造体は、MCAPファイルの先頭に置かれ、プロファイル情報やライブラリ情報を含むレコードです。

**定義:** `cpp/mcap/include/mcap/types.hpp`

```cpp
struct MCAP_PUBLIC Header {
  std::string profile;
  std::string library;
};
```

**メンバー:**

*   `profile`: レコーディングプロファイルを示す文字列。
*   `library`: レコーディングライブラリの署名を示す文字列。
