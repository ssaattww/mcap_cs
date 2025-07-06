# タスク 1.1: C# プロジェクトの作成

## 概要

C#でのMCAPライブラリ開発の基盤となる、クラスライブラリとテストプロジェクトを作成します。

## 手順

1.  **クラスライブラリプロジェクトの作成**:
    -   ターミナルまたはコマンドプロンプトを開きます。
    -   適切なディレクトリ（例: `csharp` フォルダをリポジトリルートに作成）に移動します。
    -   `dotnet new classlib -n Mcap.CSharp -f net8.0` コマンドを実行して、.NET 8.0 をターゲットとするクラスライブラリプロジェクトを作成します.

2.  **テストプロジェクトの作成**:
    -   `dotnet new xunit -n Mcap.CSharp.Tests` コマンドを実行して、xUnitテストプロジェクトを作成します。

3.  **ソリューションの作成と設定**:
    -   `dotnet new sln -n Mcap.CSharp` コマンドでソリューションファイルを作成します。
    -   `dotnet sln add Mcap.CSharp/Mcap.CSharp.csproj` でクラスライブラリをソリューションに追加します。
    -   `dotnet sln add Mcap.CSharp.Tests/Mcap.CSharp.Tests.csproj` でテストプロジェクトをソリューションに追加します。
    -   `dotnet add Mcap.CSharp.Tests/Mcap.CSharp.Tests.csproj reference Mcap.CSharp/Mcap.CSharp.csproj` で、テストプロジェクトからクラスライブラリへの参照を追加します。

## 完了条件

-   `Mcap.CSharp` ソリューションファイルが作成されている。
-   `Mcap.CSharp` クラスライブラリプロジェクトが作成されている。
-   `Mcap.CSharp.Tests` テストプロジェクトが作成され、`Mcap.CSharp` プロジェクトを参照している。
-   `dotnet build` コマンドで、ソリューション全体が正常にビルドできる。
