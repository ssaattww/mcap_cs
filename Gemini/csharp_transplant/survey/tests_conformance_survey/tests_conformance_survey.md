# 適合性テストの仕組み調査結果

## 1. 概要

MCAPプロジェクトの適合性テストは、`tests/conformance` ディレクトリ以下に集約されており、各言語実装がMCAP仕様に準拠していることを検証するために使用されます。テストはTypeScriptで書かれたスクリプトによってオーケストレーションされ、各言語のテストバイナリを実行し、その出力を解析することで適合性を判断します。

## 2. 主要ディレクトリとファイル

-   `tests/conformance/data`: テストに使用されるMCAPファイルや関連データが格納されます。
-   `tests/conformance/scripts/generate-inputs.ts`: テスト用のMCAPファイルを生成するためのスクリプトです。
-   `tests/conformance/scripts/run-tests/`: 各言語のテストランナーを管理するディレクトリです。
    -   `tests/conformance/scripts/run-tests/runners/`: 各言語（C++, Go, Python, Rust, Swift, TypeScript）ごとのテストランナー（例: `CppIndexedReaderTestRunner.ts`）が配置されています。
-   `tests/conformance/variants`: テストバリアント（テストケースの種類）の定義が含まれます。

## 3. テスト実行フロー

適合性テストの一般的な実行フローは以下の通りです。

1.  **テスト入力の生成**: `generate-inputs.ts` スクリプトが実行され、様々なMCAP仕様のバリアントに対応するMCAPファイルが生成されます。これらのファイルは通常、`tests/conformance/data` ディレクトリに配置されます。

2.  **各言語テストバイナリの実行**: `run-tests` スクリプトが、`runners` ディレクトリ内の各言語のテストランナーを呼び出します。各テストランナーは、対応する言語でビルドされたテストバイナリ（例: C++の場合は `./indexed-reader-conformance`）を、生成されたMCAPファイルを引数として実行します。

3.  **テスト結果の出力**: 各言語のテストバイナリは、MCAPファイルを読み込み、その内容を解析した結果をJSON形式で標準出力（stdout）に出力します。

4.  **結果の検証**: テストランナーは、テストバイナリから出力されたJSONデータをパースし、事前に定義された期待される結果と比較します。これにより、MCAP仕様への適合性が検証されます。

## 4. C#実装への適用

C#実装の適合性テストを行うためには、以下の手順が必要となります。

1.  **C#テストバイナリの作成**: C#でMCAPファイルを読み込み、その内容をJSON形式で標準出力に出力する実行可能ファイル（またはライブラリとそれを呼び出すテストコード）を作成します。これは、既存のC++やGoのテストバイナリと同様のインターフェースを持つ必要があります。

2.  **C#テストランナーの作成**: `tests/conformance/scripts/run-tests/runners/` ディレクトリに、`CSharpIndexedReaderTestRunner.ts` や `CSharpStreamedReaderTestRunner.ts` のようなTypeScript製のテストランナーを作成します。このランナーは、上記で作成したC#テストバイナリを呼び出し、その出力を解析するロジックを実装します。

3.  **テストの統合**: `tests/conformance/scripts/run-tests/index.ts` や関連する設定ファイルに、新しいC#テストランナーを統合し、全体のテストスイートの一部として実行されるようにします。

これにより、C# MCAPライブラリが既存の適合性テストフレームワークに組み込まれ、MCAP仕様への準拠が自動的に検証できるようになります。

## 5. テストスクリプトの実行環境

適合性テストのTypeScriptスクリプトは、Node.js を用いて実行されます。これは、`tests/conformance/package.json` に定義されたスクリプトや依存関係に基づいています。通常、`npm run <script-name>` または `yarn run <script-name>` のようなコマンドで実行されます。
