using McapCs.Writer;
using McapCs.Record;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace McapFileVerificationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputPath = "output.mcap";
            using (var writer = new McapWriter())
            {
                var options = new McapWriterOptions("test_profile", "mcap_cs_app")
                {
                    compression = McapCs.Types.Compression.None,
                    chunkSize = ulong.MaxValue
                };

                Console.WriteLine($"Opening MCAP writer with options:");
                Console.WriteLine($"  Library: {options.Library}");
                Console.WriteLine($"  Profile: {options.Profile}");
                Console.WriteLine($"  Compression: {options.compression}");
                Console.WriteLine($"  ChunkSize: {options.chunkSize}");
                writer.Open(outputPath, options);

                // Add a schema
                var schema = new Schema(
                    name: "ExampleSchema",
                    encoding: "json",
                    data: Encoding.UTF8.GetBytes("{\"type\":\"object\",\"properties\":{\"data\":{\"type\":\"string\"}}}").ToList());
                schema.id = 1;
                writer.AddSchema(schema);

                // Add a channel
                var channel = new Channel(
                    topic: "/example_topic",
                    messageEncoding: "json",
                    schemaId: 1,
                    metadata: new Dictionary<string, string>());
                channel.id = 1;
                writer.AddChannel(channel);

                // Write a single message
                var data = Encoding.UTF8.GetBytes("{\"data\":\"Hello from C#\"}");
                Console.WriteLine($"Data length: {data.Length}");
                var message = new Message
                {
                    channelId = 1,
                    sequence = 0,
                    logTime = 1678886400000000000UL,
                    publishTime = 1678886400000000000UL,
                    data = data.ToList(),
                    dataSize = (ulong)data.Length
                };
                writer.Write(message);

                writer.Close();
            }

            Console.WriteLine($"MCAP file created at: {Path.GetFullPath(outputPath)}");
            Console.WriteLine($"IsLittleEndian: {BitConverter.IsLittleEndian}");
            Console.WriteLine($"JSON data length: {Encoding.UTF8.GetBytes("{\"type\":\"object\",\"properties\":{\"data\":{\"type\":\"string\"}}}").Length}");
        }
    }
}
