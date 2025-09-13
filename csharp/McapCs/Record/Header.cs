namespace McapCs.Record;

/// <summary>
/// File header written after magic; contains recording profile and library string.
/// 先頭マジック直後に現れるヘッダ。プロファイルとライブラリ文字列を保持します。
/// </summary>
public struct Header
{
  public string profile;
  public string library;
}
