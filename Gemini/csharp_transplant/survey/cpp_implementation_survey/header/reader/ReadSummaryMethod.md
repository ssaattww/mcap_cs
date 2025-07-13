# ReadSummaryMethod

`ReadSummaryMethod` 列挙型は、サマリーセクションの読み込み方法を指定します。

**定義:** `cpp/mcap/include/mcap/reader.hpp`

```cpp
enum struct ReadSummaryMethod {
  NoFallbackScan,
  AllowFallbackScan,
  ForceScan,
};
```

**メンバー:**

*   `NoFallbackScan`: サマリーセクションを解析して、シーキングインデックスとサマリー統計を生成します。サマリーセクションが存在しないか破損している場合、失敗ステータスが返されます。
*   `AllowFallbackScan`: サマリーセクションが見つからないか不完全な場合、ファイルを順次読み取ってシーキングインデックスとサマリー統計を生成するフォールバックを許可します。
*   `ForceScan`: ファイルをヘッダーからデータエンドまで順次読み取って、シーキングインデックスとサマリー統計を生成します。
