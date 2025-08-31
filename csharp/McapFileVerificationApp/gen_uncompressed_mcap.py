#!/usr/bin/env python3
"""
このスクリプトは、現在の C# McapReader で読み取り可能な「非圧縮」の MCAP ファイルを生成します。

使い方:
  PYTHONPATH=python/mcap python3 csharp/McapFileVerificationApp/gen_uncompressed_mcap.py 出力先.mcap

注意:
- python/examples/raw/writer.py を変更せず、直接 mcap の Writer を CompressionType.NONE で使用します。
"""
import json
import sys
from time import time_ns

try:
    from mcap.writer import Writer, CompressionType
except Exception as e:
    print("mcap.writer のインポートに失敗しました。PYTHONPATH に 'python/mcap'（リポジトリルート配下）を含めてください。", file=sys.stderr)
    raise


def main() -> int:
    if len(sys.argv) < 2:
        print("使い方: gen_uncompressed_mcap.py <出力先.mcap>")
        return 2
    out = sys.argv[1]
    with open(out, "wb") as f:
        w = Writer(f, compression=CompressionType.NONE)
        w.start()
        schema_id = w.register_schema(
            name="sample",
            encoding="jsonschema",
            data=json.dumps({
                "type": "object",
                "properties": {"sample": {"type": "string"}},
            }).encode("utf-8"),
        )
        channel_id = w.register_channel(
            schema_id=schema_id,
            topic="sample_topic",
            message_encoding="json",
        )
        w.add_message(
            channel_id=channel_id,
            log_time=time_ns(),
            publish_time=time_ns(),
            data=json.dumps({"sample": "test"}).encode("utf-8"),
        )
        w.finish()
    print(f"出力しました: {out}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
