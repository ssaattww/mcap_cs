#!/usr/bin/env python3
"""
Generate an uncompressed MCAP file compatible with the current C# McapReader.

Usage:
  PYTHONPATH=python/mcap python3 csharp/McapFileVerificationApp/gen_uncompressed_mcap.py output.mcap

This script intentionally does not modify or depend on python/examples/raw/writer.py.
It uses the mcap Writer directly with CompressionType.NONE.
"""
import json
import sys
from time import time_ns

try:
    from mcap.writer import Writer, CompressionType
except Exception as e:
    print("Failed to import mcap.writer. Ensure PYTHONPATH includes 'python/mcap' (repo root).", file=sys.stderr)
    raise


def main() -> int:
    if len(sys.argv) < 2:
        print("Usage: gen_uncompressed_mcap.py <output.mcap>")
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
    print(f"Wrote {out}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())

