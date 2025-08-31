# Agent 運用ガイド（Serena 活用）

このリポジトリで作業するエージェントは、Serena のツールを積極的に活用して計画・記録・参照を行います。以下の指針に従ってください。

## 基本方針
- 日本語で簡潔に記録・報告する（コマンドは必要に応じて併記）。
- 必要最小限の探索・変更に留め、計画を小さく素早く回す。
- TDD を優先し、テストで挙動を確認する。

## Serena の使い方
- 計画管理（必須）: `update_plan`
  - ステップは短文（5～7語程度）で、状態は `pending`/`in_progress`/`completed` を維持。
  - フェーズ変更や追加があれば計画を更新し、理由を一行で付記。
- オンボーディング/メモ: `write_memory` / `read_memory`
  - 以下を最低限維持: `project_overview`/`suggested_commands`/`style_and_conventions`/`completion_checklist`。
  - 重要変更はサマリを新規メモ（例: `task_log_YYYY-MM-DD_topic`）に記録。
  - 作業サマリの参照を徹底: 直近の `work_summary_YYYY-MM-DD` を開始前に確認し、継続性を維持する（今回作成: `work_summary_2025-08-31`）。
- 参照/探索: `find_file`/`search_for_pattern`/`find_symbol`/`get_symbols_overview`
  - 広く読むのではなく、目的に応じてピンポイントに検索。
- 思考チェック: `think_about_task_adherence`/`think_about_collected_information`/`think_about_whether_you_are_done`
  - 編集前後で自己点検し、脱線や取りこぼしを避ける。

## 記録粒度（既定）
- 既定は「タスク単位＋検証ログ」。
  - タスク単位: 主要変更点・影響・検証方法を短く列挙。
  - 検証ログ: 実行コマンド・結果要約・失敗時の原因/対処。
- 必要に応じて詳細化:
  - 変更単位（変更ファイル/関数/APIと理由・リスク）
  - 設計決定（選択肢・採用理由・影響・将来拡張）
  - 詳細トレース（入出力・境界条件・疑似コード/図）

## 実務ルール
- コード編集は `apply_patch` で行い、差分を最小に保つ。
- 影響範囲が広い変更は計画を分割し、段階的に適用。
- CI/テストは必要時のみ実行し、ログは簡潔に共有。
- セキュリティ: 秘密情報をコミットしない。大きなバイナリは Git LFS。

## 例（ワークフロー）
1. `update_plan` で小さな計画を作成
2. `find_file`/`search_for_pattern` で関連箇所を特定
3. `apply_patch` で変更
4. ビルド/テスト（必要に応じて）
5. `write_memory` にタスク要約＋検証ログを記録
6. `think_about_whether_you_are_done` で完了確認
