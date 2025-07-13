using Xunit;
using McapCs.Writer;
using System.Linq;

namespace McapCs.Test.Writer;

public class BufferWriterTests
{
    [Fact]
    public void Write_AppendsDataToBuffer()
    {
        var writer = new BufferWriter();
        var data = new byte[] { 0x01, 0x02, 0x03 };
        writer.Write(data);

        Assert.Equal(data.Length, (int)writer.Size);
        Assert.True(data.SequenceEqual(writer.Data));
    }
}
