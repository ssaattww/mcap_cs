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
                var options = new McapWriterOptions("ros2") { compression = McapCs.Types.Compression.None }; // Use a profile, e.g., "ros2"
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

                // Write some messages
                for (int i = 0; i < 5; i++)
                {
                    var message = new Message
                    {
                        channelId = 1,
                        sequence = (uint)i,
                        logTime = (ulong)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000000),
                        publishTime = (ulong)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000000),
                        data = Encoding.UTF8.GetBytes($"{{\"data\":\"Hello from C# {i}\"}}").ToList()
                    };
                    writer.Write(message);
                }

                writer.Close();
            }

            Console.WriteLine($"MCAP file created at: {Path.GetFullPath(outputPath)}");
        }
    }
}
