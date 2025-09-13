import struct
from pathlib import Path
import sys

EXPECTED = b"\x89MCAP\r\n\x1a\n"

def verify_mcap_file(file_path: str) -> None:
    with open(file_path, "rb") as f:
        file_content = f.read()
        magic_bytes_start = file_content[: len(EXPECTED)]
        assert magic_bytes_start == EXPECTED, (
            f"Magic bytes start mismatch: {magic_bytes_start.hex()} != {EXPECTED.hex()}"
        )

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python3 verify_magic.py <file.mcap>")
        raise SystemExit(2)
    try:
        verify_mcap_file(sys.argv[1])
        print("OK: magic bytes match")
    except AssertionError as e:
        print(f"NG: {e}")
