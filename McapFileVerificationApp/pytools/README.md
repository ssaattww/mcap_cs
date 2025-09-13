MCAP 検証用 Python ツール集

概要

- C# 実装で生成した MCAP を Python 公式実装（mcap）で読み取り、相互運用を確認するためのスクリプトをまとめています。

セットアップ

- 仮想環境の作成と mcap のインストール:
  - `python3 -m venv .venv && . .venv/bin/activate && python -m pip install mcap`
    - すでにインストール済なら`source .venv/bin/activate`
スクリプト一覧

- `verify_mcap.py`: MCAP を読み取り、メッセージ件数とヘッダ情報を表示します。
  - 例: `python3 verify_mcap.py ../none.mcap ../lz4.mcap ../zstd.mcap`
- `verify_magic.py`: 先頭マジックバイトのみを簡易検証します。
  - 例: `python3 verify_magic.py ../none.mcap`
- `gen_uncompressed.py`: Python mcap で非圧縮の MCAP を生成します。
  - 例: `python3 gen_uncompressed.py ./out.mcap`
- `read.py`: 固定パス（`csharp/McapFileVerificationApp/output.mcap`）の MCAP を読み出して内容を表示します。
  - 例: `python3 read.py`

補足（C# サンプルアプリでの生成）

- 圧縮種別ごとの出力例:
  - `dotnet run --project .. -- --write-compressed none ../none.mcap`
  - `dotnet run --project .. -- --write-compressed lz4 ../lz4.mcap`
  - `dotnet run --project .. -- --write-compressed zstd ../zstd.mcap`
