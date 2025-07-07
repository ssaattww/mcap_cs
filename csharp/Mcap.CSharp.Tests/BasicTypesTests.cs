using Mcap.CSharp.Mcap;

namespace Mcap.CSharp.Tests;

public class BasicTypesTests
{
    [Fact]
    public void TestOpCodeValues()
    {
        Assert.Equal(0x01, (byte)OpCode.Header);
        Assert.Equal(0x0F, (byte)OpCode.DataEnd);
    }
}
