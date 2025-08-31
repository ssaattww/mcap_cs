# Completion Checklist

- C# ビルド・テスト
  - `dotnet restore McapCs.sln`
  - `dotnet build McapCs.sln -c Release`
  - `dotnet test McapCs.sln -c Release`
  - (必要に応じ) カバレッジ: `dotnet test McapCs.sln -c Release --collect:"XPlat Code Coverage"`
  - 検証アプリ実行確認: `dotnet run --project csharp/McapFileVerificationApp -- --help`
- 他言語の変更がある場合は各言語のテスト/リンタ/ビルドも実行
  - TS: `yarn typescript:test`
  - Python: `make -C python test`
  - Go: `make -C go test`
  - Rust: `pushd rust && cargo test`
  - Swift: `pushd swift && swift test`
  - C++: `make -C cpp`
- 形式・リンタ
  - TS: `yarn prettier:check`
  - Python: `make -C python lint`
  - C#: `dotnet format`（任意）
- コンフォーマンステスト（フォーマット/IO 変更時）
  - LFS データ: `git lfs install && git lfs fetch`
  - 実行: `yarn test:conformance`
- ドキュメント・PR
  - 変更点に応じて README/website を更新
  - Conventional commits + Issue 番号、`feature/<task>` ブランチ、PR into `develop`、CI パス
- セキュリティ
  - 秘密情報をコミットしない。大きなバイナリは Git LFS。テストでのネットワークアクセス回避。

