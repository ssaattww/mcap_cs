import struct

def hex_to_bytes(hex_string):
    return bytes.fromhex(hex_string.replace('-', ''))

def verify_mcap_file(file_path):
    with open(file_path, "rb") as f:
        file_content = f.read()

        # Verify Magic Bytes (start)
        magic_bytes_start = file_content[0:9]
        expected_magic_bytes = b'\x89MCAP\r\n\x1a\n'
        assert magic_bytes_start == expected_magic_bytes, f"Magic bytes start mismatch: {magic_bytes_start.hex()} != {expected_magic_bytes.hex()}"

        # For simplicity, we'll just check the magic bytes for now.
        # Further verification of records would require parsing the file manually.

    print("MCAP file verification successful!")

if __name__ == "__main__":
    try:
        verify_mcap_file("csharp/McapFileVerificationApp/output.mcap")
    except AssertionError as e:
        print(f"Verification failed: {e}")
    except Exception as e:
        print(f"An unexpected error occurred: {e}")
