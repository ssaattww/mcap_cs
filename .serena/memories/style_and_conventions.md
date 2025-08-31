# Style and Conventions

## TypeScript
- Formatting: Prettier (`yarn prettier` / `yarn prettier:check`), configs at repo root and per-package.
- Testing: Jest via `yarn typescript:test`.
- Naming: camelCase for variables/functions; PascalCase for types.
- Linting: Packages use ESLint configs (e.g., `tests/conformance/.eslintrc.js`, `website/.eslintrc.js`).

## Python
- Format: `black` (check in `make -C python lint`).
- Imports: `isort` (checked in lint target).
- Lint: `flake8`.
- Type check: `pyright`.
- Naming: snake_case modules/functions, PascalCase classes.

## Swift
- Formatting: `swiftformat .` (config `.swiftformat` at root).
- Lint: `swiftlint --fix` (config `.swiftlint.yml` at root).

## C++
- Formatting: `.clang-format` in `cpp/`. Makefile provides format-check/fix via container.
- Naming: CamelCase types; snake_case functions.

## Go / Rust / C#
- Follow language norms (gofmt/golangci, rustfmt/clippy if applicable, dotnet format).
- C#: format/lint with `dotnet format`; .NET naming (PascalCase types/members, camelCase locals).

## Testing Guidelines
- Run language tests for any changes in that language.
- Run cross-language conformance tests for format/IO changes; ensure Git LFS test data present.
- Name tests after the unit under test (e.g., `XyzReader_test`, `test_xyz_reader.rs`).

## Workflow / PRs
- Prefer TDD: write failing tests first, then implement; do not alter tests to pass code.
- Branch: `feature/<task>` from `develop`; open PRs into `develop`.
- Commits: conventional prefixes (`feat:`, `fix:`, `docs:`, etc.) and reference issues (e.g., `#123`).
- PRs: concise rationale, steps, logs/screens for behavior changes; CI must pass.
- Security: never commit secrets; large binaries via Git LFS; avoid network access in tests.