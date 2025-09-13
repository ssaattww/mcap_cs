# McapWriter ドキュメント

本ディレクトリの `McapWriter` に関する使用方法と圧縮仕様の要点をまとめます。

## MCAP Writer 圧縮の使い方（C#）

- オプション例:

  ```csharp
  // 圧縮を有効化した基本オプション例
  var options = new McapWriterOptions(
      profile: "default",
      library: $"libmcap {Constants.MCAP_LIBRARY_VERSION}")
  {
      noChunking = false,
      compression = Compression.Zstd,
      compressionLevel = CompressionLevel.Default,
      forceCompression = true
  };
  ```

- コード例:

  ```csharp
  var w = new McapWriter();
  w.Open("out.mcap", options);
  // ここでスキーマ/チャネル登録 → メッセージ書き込みを行う
  w.Close();
  ```

- 仕様: 書き込み中は非圧縮で蓄積し、`CloseLastChunk/Close` 時に `WriteChunk` 内で圧縮を試行。
  - しきい値: サイズ >= 約 1KB、縮小率 >= 2% を満たすと `compression: lz4|zstd` を採用。満たさない場合は `none`。

## テスト/検証方針（抜粋）

- `dotnet test csharp/McapCs.sln -c Release` で C# テストを実行。
- Python 公式 `mcap` で相互検証（任意）:
  - インストール: `pip install mcap`
  - 読み出しスモーク: `python -c "from mcap.reader import make_reader; import sys; f=open(sys.argv[1],'rb'); r=make_reader(f); [(_ for _ in r.iter_messages())]; print('ok')" out.mcap`
  - 圧縮名確認: `mcap info out.mcap` に `Compression: lz4|zstd|none` が表示されること

---

補足や詳細は `csharp/McapCs/Writer/McapWriter.cs` を参照してください。

