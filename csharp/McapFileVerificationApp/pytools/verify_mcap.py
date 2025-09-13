#!/usr/bin/env python3
"""
MCAP 読取の相互検証用スクリプト。

使い方:
  python3 verify_mcap.py <file1.mcap> [file2.mcap ...]

前提:
  python3 -m venv .venv && . .venv/bin/activate && python -m pip install mcap
"""
from __future__ import annotations

import argparse
import sys
from pathlib import Path
from typing import Tuple, Set, Optional

try:
    from mcap.reader import make_reader
    from mcap.stream_reader import StreamReader
    from mcap.records import Chunk
except Exception:
    print("[ERROR] mcap がインポートできません。'python -m pip install mcap' を実行してください。", file=sys.stderr)
    raise


# 圧縮方式（chunk header の compression）は公式 Python のハイレベル API では
# 直接提供されないため、本ツールでは収集しません。
Compressions = Set[str]


def summarize_messages(path: Path) -> Tuple[int, int, int, Set[str], Set[Tuple[str, str]], list[Tuple[Optional[str], str, int, bytes]]]:
    """メッセージ件数、最小/最大 log_time、トピック集合、(schema_name, encoding) 集合、
    さらに (schema名, topic, log_time, data) のリスト（順不同・全件）を返す。"""
    count = 0
    tmin = None
    tmax = None
    topics: Set[str] = set()
    schemas: Set[Tuple[str, str]] = set()
    samples: list[Tuple[Optional[str], str, int, bytes]] = []
    with path.open("rb") as f:
        r = make_reader(f)
        for schema, channel, message in r.iter_messages():
            count += 1
            topics.add(getattr(channel, "topic", ""))
            schemas.add((getattr(schema, "name", ""), getattr(schema, "encoding", "")))
            ts = getattr(message, "log_time", None)
            if ts is not None:
                tmin = ts if tmin is None else min(tmin, ts)
                tmax = ts if tmax is None else max(tmax, ts)
            samples.append((getattr(schema, "name", None), getattr(channel, "topic", ""), ts or 0, getattr(message, "data", b"")))
    return count, (tmin or 0), (tmax or 0), topics, schemas, samples


def get_compressions_via_api(path: Path) -> Set[str]:
    """公式 Python API だけを使って圧縮方式を収集する（Summary優先、無ければChunk走査）。"""
    comps: Set[str] = set()
    with path.open("rb") as f:
        r = make_reader(f)
        try:
            summary = r.get_summary()
        except Exception:
            summary = None
        if summary and summary.chunk_indexes:
            for ci in summary.chunk_indexes:
                comps.add((ci.compression or "none").strip())
            return comps
    # フォールバック: Chunk レコードを直接走査
    with path.open("rb") as f:
        for rec in StreamReader(f, skip_magic=False).records:
            if isinstance(rec, Chunk):
                comps.add((rec.compression or "none").strip())
    return comps


def safe_preview(data: bytes, max_bytes: int = 256) -> str:
    """payload の先頭 max_bytes を UTF-8 で表示。不可ならバイト長のみ表示。"""
    view = data[:max_bytes]
    try:
        text = view.decode("utf-8", errors="replace")
        if len(data) > max_bytes:
            text += f" …(+{len(data)-max_bytes}B)"
        return text
    except Exception:
        return f"<{len(data)} bytes>"


def verify_one(path: Path, show_messages: bool = True, limit: int = 10, max_bytes: int = 256) -> int:
    try:
        count, tmin, tmax, topics, schemas, samples = summarize_messages(path)
        comps = get_compressions_via_api(path)
        topics_s = ",".join(sorted(t for t in topics if t)) or "-"
        schemas_s = ",".join(sorted(f"{n}({e})" for n, e in schemas if n or e)) or "-"
        print(
            f"OK: {path} | messages={count} | time=[{tmin},{tmax}] | topics=[{topics_s}] | schemas=[{schemas_s}] | chunks.compression=[{','.join(sorted(comps)) or '-'}]"
        )
        if show_messages and samples:
            for i, (sname, topic, ts, data) in enumerate(samples):
                if i >= limit:
                    print(f"  ... and {len(samples)-limit} more messages")
                    break
                preview = safe_preview(data, max_bytes=max_bytes)
                sdisp = sname if sname else " None"
                print(f"  [{i}] topic={topic} time={ts} schema={sdisp} size={len(data)} data={preview}")
        return 0
    except Exception as ex:
        print(f"NG: {path} | {ex}", file=sys.stderr)
        return 1


def main(argv: list[str]) -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("files", nargs="+", help="検証する MCAP ファイル")
    ap.add_argument("--no-messages", action="store_true", help="メッセージ本文を表示しない")
    ap.add_argument("--limit", type=int, default=10, help="表示するメッセージの最大件数（各ファイル）")
    ap.add_argument("--max-bytes", type=int, default=256, help="本文プレビューの最大バイト数")
    args = ap.parse_args(argv)

    rc = 0
    for s in args.files:
        rc |= verify_one(Path(s), show_messages=not args.no_messages, limit=args.limit, max_bytes=args.max_bytes)
    return rc


if __name__ == "__main__":
    raise SystemExit(main(sys.argv[1:]))
