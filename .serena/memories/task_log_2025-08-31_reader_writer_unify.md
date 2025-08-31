# Task Log: C# Reader/Writer magic unification + Python interop

## Task-level summary
- Unify MCAP magic bytes per spec (8B: 0x89,'M','C','A','P','0','\r','\n')
- C# Reader: use Constants.Magic for header/trailer; stop on trailing magic
- C# Reader: parse Message payload as (record_len - 22B); add legacy-compatible branch for optional 4B data length
- C# Reader: support non-compressed chunks; throw NotSupportedException on compressed chunks (lz4/zstd)
- C# Writer: still writes legacy message layout; kept for backward-compat; Reader handles both
- App: add --read/--write to McapFileVerificationApp
- Added helper script to generate non-compressed MCAP without modifying python/examples/raw/writer.py

## Impact
- Python MCAP (CompressionType.NONE) can be read by C# Reader
- Interop validated end-to-end; unit tests updated/added (Reader tests) and all pass (19/19)
- Future: optional path to migrate Writer to spec-only message layout and adjust tests

## Verification log
- Build & tests:
  - dotnet build csharp/McapCs.sln -c Release → OK
  - dotnet test csharp/McapCs.sln -c Release → 19 tests, all pass
- Python MCAP generation (non-compressed):
  - PYTHONPATH=python/mcap python3 csharp/McapFileVerificationApp/gen_uncompressed_mcap.py output_from_app.mcap → OK
- Read with C# app:
  - dotnet run --project csharp/McapFileVerificationApp -- --read output_from_app.mcap → Header/Schemas/Channels/Message printed as expected
- Additional interop check (manual):
  - PYTHONPATH=python/mcap python3 -c "...Writer(..., compression=CompressionType.NONE)..." → output_py_uncompressed.mcap
  - dotnet run --project csharp/McapFileVerificationApp -- --read output_py_uncompressed.mcap → OK

## Notes / follow-ups
- Consider migrating Writer Message to spec-only layout (no extra 4B length) and updating MessageTests accordingly
- Plan future work to add LZ4/Zstd decompression behind an interface (IChunkDecompressor) and corresponding tests