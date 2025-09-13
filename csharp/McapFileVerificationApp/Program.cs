using McapCs.Writer;
using McapCs.Record;
using McapCs.Reader;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace McapFileVerificationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1 && args[0] == "--help")
            {
                PrintHelp();
                return;
            }

            if (args.Length >= 2 && args[0] == "--read")
            {
                var path = args[1];
                ReadAndPrint(path);
                return;
            }

            if (args.Length >= 2 && args[0] == "--write")
            {
                var outputPath = args[1];
                WriteSample(outputPath);
                return;
            }

            if (args.Length >= 3 && args[0] == "--write-compressed")
            {
                var algo = args[1]; // none|lz4|zstd
                var outputPath = args[2];
                WriteSampleCompressed(outputPath, algo);
                return;
            }

            // デフォルト動作: ヘルプ表示（日本語）
            PrintHelp();
        }

        static void PrintHelp()
        {
            Console.WriteLine("使い方:");
            Console.WriteLine("  --read <path>                 指定した MCAP ファイルを読み込み、内容を表示します");
            Console.WriteLine("  --write <path>                サンプルの MCAP ファイル（非圧縮）を出力します");
            Console.WriteLine("  --write-compressed <algo> <path>  圧縮アルゴリズム（none|lz4|zstd）で出力します");
        }

        static void ReadAndPrint(string path)
        {
            Console.WriteLine($"MCAP を読み込み中: {Path.GetFullPath(path)}");
            var reader = new McapReader();
            reader.Open(path);

            if (reader.Header != null)
            {
                Console.WriteLine($"ヘッダー: profile={reader.Header?.profile}, library={reader.Header?.library}");
            }
            Console.WriteLine($"スキーマ数: {reader.Schemas.Count}");
            foreach (var kv in reader.Schemas)
            {
                var s = kv.Value;
                Console.WriteLine($"  [Schema] id={s.id} name={s.name} encoding={s.encoding} data長={s.data.Count}");
            }

            Console.WriteLine($"チャネル数: {reader.Channels.Count}");
            foreach (var kv in reader.Channels)
            {
                var c = kv.Value;
                Console.WriteLine($"  [Channel] id={c.id} topic={c.topic} encoding={c.messageEncoding} schemaId={c.schemaId}");
            }

            Console.WriteLine($"メッセージ数: {reader.Messages.Count}");
            foreach (var m in reader.ReadMessages())
            {
                string topic = reader.Channels.TryGetValue(m.channelId, out var ch) ? ch.topic : "(unknown)";
                string payloadStr = SafeUtf8(m.data);
                Console.WriteLine($"  [Message] ch={m.channelId} topic={topic} seq={m.sequence} log={m.logTime} size={m.data.Count} data={payloadStr}");
            }
        }

        static string SafeUtf8(List<byte> data)
        {
            try
            {
                return Encoding.UTF8.GetString(data.ToArray());
            }
            catch
            {
                return $"<{data.Count} bytes>";
            }
        }

        static void WriteSample(string outputPath)
        {
            using var writer = new McapWriter();
            var options = new McapWriterOptions("test_profile", "mcap_cs_app")
            {
                compression = McapCs.Types.Compression.None,
                chunkSize = 1024
            };
            writer.Open(outputPath, options);

            var schema = new Schema(
                name: "sample",
                encoding: "jsonschema",
                data: Encoding.UTF8.GetBytes("{\"type\":\"object\",\"properties\":{\"sample\":{\"type\":\"string\"}}}").ToList());
            schema.id = 1;
            writer.AddSchema(schema);

            var channel = new Channel(
                topic: "sample_topic",
                messageEncoding: "json",
                schemaId: 1,
                metadata: new Dictionary<string, string>());
            channel.id = 1;
            writer.AddChannel(channel);

            var data = Encoding.UTF8.GetBytes("{\"sample\":\"test\"}");
            var message = new Message
            {
                channelId = 1,
                sequence = 1,
                logTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                publishTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                data = data.ToList(),
                dataSize = (ulong)data.Length
            };
            writer.Write(message);
            writer.Close();

            Console.WriteLine($"MCAP ファイルを出力しました: {Path.GetFullPath(outputPath)}");
        }

        static void WriteSampleCompressed(string outputPath, string algo)
        {
            using var writer = new McapWriter();
            var compression = algo.ToLower() switch
            {
                "none" => McapCs.Types.Compression.None,
                "lz4" => McapCs.Types.Compression.Lz4,
                "zstd" => McapCs.Types.Compression.Zstd,
                _ => McapCs.Types.Compression.None,
            };
            var options = new McapWriterOptions("test_profile", "mcap_cs_app")
            {
                compression = compression,
                chunkSize = 1024,
                forceCompression = true,
                compressionLevel = McapCs.Types.CompressionLevel.Default,
            };
            writer.Open(outputPath, options);

            var schema = new Schema(
                name: "sample",
                encoding: "jsonschema",
                data: Encoding.UTF8.GetBytes("{\"type\":\"object\",\"properties\":{\"sample\":{\"type\":\"string\"}}}").ToList());
            writer.AddSchema(schema);

            var channel = new Channel(
                topic: "sample_topic",
                messageEncoding: "json",
                schemaId: schema.id,
                metadata: new Dictionary<string, string>());
            writer.AddChannel(channel);

            // 圧縮効果が出やすい繰り返しデータ
            var sb = new StringBuilder();
            for (int i = 0; i < 2000; i++) sb.Append('A');
            var payload = Encoding.UTF8.GetBytes($"{{\"sample\":\"{sb}\"}}");

            var now = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var message = new Message
            {
                channelId = 1,
                sequence = 1,
                logTime = now,
                publishTime = now,
                data = payload.ToList(),
                dataSize = (ulong)payload.Length,
            };
            writer.Write(message);
            writer.Close();

            Console.WriteLine($"MCAP ファイルを出力しました (algo={algo}): {Path.GetFullPath(outputPath)}");
        }
    }
}
