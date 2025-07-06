# タスク 1.1: C# プロジェクトの作成

## 概要

C#でのMCAPライブラリ開発の基盤となる、クラスライブラリとテストプロジェクトを作成します。

## 手順

1.  **クラスライブラリプロジェクトの作成**:
    -   ターミナルまたはコマンドプロンプトを開きます。
    -   適切なディレクトリ（例: `csharp` フォルダをリポジトリルートに作成）に移動します。
    -   `dotnet new classlib -n Mcap.CSharp -f netstandard2.0` コマンドを実行して、.NET Standard 2.0 をターゲットとするクラスライブラリプロジェクトを作成します。

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

## 参考

なし

## 作業状況

### 手順1

- ターミナルを作成しcsharpディレクトリを作成し作成したディレクトリへ移動
- `dotnet new classlib -n Mcap.CSharp -f netstandard2.0`コマンドを実行
- クラスライブラリの作成に失敗したことを確認
  - すでにクラスライブラリが作成されていることが原因
- すでにクラスライブラリが作成されているため完了

### 手順2

- `dotnet new xunit -n Mcap.CSharp.Tests` コマンドを実行して、xUnitテストプロジェクトを作成
- テストプロジェクトが作成されたことを確認

### 手順3

- `dotnet new sln -n Mcap.CSharp` コマンドでソリューションファイルを作成
- `dotnet sln add Mcap.CSharp/Mcap.CSharp.csproj` でクラスライブラリをソリューションに追加
- `dotnet sln add Mcap.CSharp.Tests/Mcap.CSharp.Tests.csproj` でテストプロジェクトをソリューションに追加
- `dotnet add Mcap.CSharp.Tests/Mcap.CSharp.Tests.csproj reference Mcap.CSharp/Mcap.CSharp.csproj` で、テストプロジェクトからクラスライブラリへの参照を追加
- 作業完了

### 手順4

- ソリューションが作成され、各プロジェクトがソリューションに追加されたことを確認
- `dotnet build` コマンドで、ソリューション全体が正常にビルドできることを確認
- 作業完了
