# Suggested Commands

## Prerequisites
- Git LFS (for conformance data): `git lfs install && git lfs fetch`
- Node/Yarn: `corepack enable` if needed; uses `yarn@4.5.1`.
- .NET SDK: 8.x (`dotnet --info`)

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

## C# (.NET 8)
- Restore: `dotnet restore McapCs.sln`
- Build: `dotnet build McapCs.sln -c Release`
- Test: `dotnet test McapCs.sln -c Release`
- Test with coverage (coverlet.collector): `dotnet test McapCs.sln -c Release --collect:"XPlat Code Coverage"`
- Run verification app: `dotnet run --project csharp/McapFileVerificationApp -- --help`
- Optional format: `dotnet format` (if used locally)

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

