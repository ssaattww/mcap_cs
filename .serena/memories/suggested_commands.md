# Suggested Commands

## Prerequisites
- Git LFS (for conformance data): `git lfs install && git lfs fetch`
- Node/Yarn: `corepack enable` if needed; uses `yarn@4.5.1`.

## Monorepo / Root
- Format (write): `yarn prettier`
- Format (check): `yarn prettier:check`
- Spell check: `yarn spellcheck`
- Build TS docs: `yarn typedoc`
- Start website: `yarn start` (delegates to `website` workspace)
- TypeScript build (all packages): `yarn typescript:build`
- TypeScript tests: `yarn typescript:test`
- TypeScript clean: `yarn typescript:clean`
- Conformance: `yarn test:conformance`
- Conformance (generate inputs): `yarn test:conformance:generate-inputs`

## C#
- Restore: `dotnet restore csharp/McapCs.sln`
- Build: `dotnet build csharp/McapCs.sln -c Release`
- Test: `dotnet test csharp/McapCs.sln -c Release`
- Run app: `dotnet run --project csharp/McapFileVerificationApp -- --help`

## Python
- Test: `make -C python test`
- Lint: `make -C python lint`
- Build: `make -C python build`

## Go
- Test: `make -C go test`
- Lint: `make -C go lint`
- Bench: `make -C go bench`

## Rust
- Build: `pushd rust && cargo build`
- Test: `pushd rust && cargo test`

## C++
- Docker build/test: `make -C cpp`
- Host toolchain build: `make -C cpp build-host`

## Swift
- Build: `pushd swift && swift build`
- Test: `pushd swift && swift test`

## Conformance Data
- Ensure LFS data present: `git lfs install && git lfs fetch`
- Run tests: `yarn test:conformance`