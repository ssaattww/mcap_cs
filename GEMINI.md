# Geminiプロジェクトのコンテキスト: mcap

このドキュメントは、Gemini AIアシスタントのためのmcapプロジェクトに関するコンテキストを提供します。

## プロジェクト概要

MCAPは、任意のメッセージシリアライゼーションを使用したpub/subメッセージのためのモジュール式コンテナフォーマットおよびロギングライブラリです。主にロボット工学アプリケーションでの使用を目的としています。

## 会話ガイドライン

- 常に日本語で会話する

## 開発方法

### 開発ルール

- 原則としてテスト駆動開発（TDD）で進める
- 期待される入出力に基づき、まずテストを作成する
- 実装コードは書かず、テストのみを用意する
- テストを実行し、失敗を確認する
- テストが正しいことを確認できた段階でコミットする
- その後、テストをパスさせる実装を進める
- 実装中はテストを変更せず、コードを修正し続ける
- すべてのテストが通過するまで繰り返す
### 手順

#### 計画段階

- `Gemini/hoge/hoge_plan.md`へ大まかな開発方針を記載する。
  - 開発方針は適度にフェーズ/タスクに分割して記載する
- `Gemini/hoge/hoge_plan.md`に従って、`Gemini/hoge/hoge_tasks/Phase*/task_piyo.md`へ`Gemini/hoge/hoge_plan.md`で挙げた各タスクの詳細を記載する。
  - `Gemini/sample_task_1_1_create_project.md`へ`task_piyo.md`の1例を示す

#### 実装段階

- **TDDに従って開発を行う。**
  - プロジェクトファイル作成以外の場合、必ずTDDに従う
    - テストを先に書く
    - テスト実行時に失敗した場合でもテストを変更しない
- `Gemini/hoge_tasks/Phase*/task_piyo.md`に記載の手順に必ず従う

- 計画に変更が必要な場合は、Userに計画の変更を進言する。
  - 計画段階に戻って`Gemini/hoge_tasks/Phase*/task_piyo.md`の修正を行う
  - 計画変更はgit バージョン管理で追跡できるようにする
- 移植作業のときには、移植元の処理がどこにあるのかわかるようにコメントを書くこと
##### タスクの管理
- 実施したコマンド等作業内容は作業状況セクションに記載する
  - 失敗したときにも原因を調査し作業状況セクションへ簡潔に記載する
  - **コミット前に作業状況は記入すること**
- 各タスクごとにfeature/hoge_taskのような名前でdevelopブランチをもとにブランチを作成して作業を行うこと
- taskの完了時には以下の２つの作業を行うこと
  - task完了後はdevelopブランチへプルリクエストを作成すること
  - taskの実施状況の最後に完了と入力する
  - 終了したtaskのgithub issueはclose
- 「TBD」「Todo」がある場合は、該当のtaskに残件があることを記載する。
  - 付随してgithubにサブissueを作成すること

##### git commit
- gitのコミットメッセージは、`feat:` `fix:` `docs:` 等から始める。
- どのタスクの内容かわかるようにする。
  - コミットメッセージにgithub issue番号を記載する
    - issueは`github`mcp serverを用いて管理する
    - issueがなければmcp serverを用いて追加する
- taskが完了した場合、プルリクエストを出す
## 言語と実装

このリポジトリには、いくつかの言語でのMCAP仕様の実装が含まれています。各実装には、独自のディレクトリ、ビルドプロセス、およびリリースプロセスがあります。

- **C++:** `cpp/` ディレクトリにあります。
- **Go:** `go/` ディレクトリにあります。
- **Python:** `python/` ディレクトリにあります。
- **TypeScript/JavaScript:** `typescript/` ディレクトリにあります。
- **Swift:** `swift/` ディレクトリにあります。
- **Rust:** `rust/` ディレクトリにあります。

## 主要なディレクトリ

- `/cpp`: C++のソースコードとビルドファイル。
- `/go`: Goのソースコードとビルドファイル。
- `/python`: Pythonのソースコードとパッケージングファイル。
- `/rust`: RustのソースコードとCargoファイル。
- `/swift`: Swiftのソースコードとパッケージファイル。
- `/typescript`: TypeScript/JavaScriptのソースコードとパッケージファイル。
- `/website`: ドキュメントウェブサイト（https://mcap.dev）のソース。
- `/Gemini`: Gemini AIアシスタント用のmcapプロジェクトに関するコンテキスト群を提供するディレクトリ。C++からC#への移植計画書（`csharp_transplant_plan.md`）などが含まれます。
- `/tests/conformance`: MCAP仕様の適合性テスト。

## ビルドとテスト

- 各言語には独自のビルドとテストのプロセスがあり、通常は`Makefile`またはその言語の他の標準的なビルドツール（例：Rustの場合は`Cargo`、Goの場合は`go build`）によって管理されます。
- 適合性テストは`tests/conformance`にあり、テストデータがLFSに保存されているため、Git LFSをインストールする必要があります。

## リリースプロセス

リリースプロセスは、メインの`README.md`ファイルに記載されています。これには、各言語の特定のファイルでバージョン番号を更新し、特定の形式でgitタグを作成することが含まれます。

- **Go:** `go/mcap/vX.Y.Z`
- **CLI:** `releases/mcap-cli/vX.Y.Z`
- **C++:** `releases/cpp/vX.Y.Z`
- **Python:** `releases/python/PACKAGE/vX.Y.Z`
- **TypeScript:** `releases/typescript/PACKAGE/vX.Y.Z`
- **Swift:** `releases/swift/vX.Y.Z`
- **Rust:** `releases/rust/vX.Y.Z`
