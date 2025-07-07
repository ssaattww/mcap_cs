using Mcap.CSharp.Mcap;
using Xunit;

namespace Mcap.CSharp.Tests;

public class ConstantsTests
{
    [Fact]
    public void TestLibraryVersion()
    {
        Assert.Equal("2.0.2", Constants.LibraryVersion);
    }

    [Fact]
    public void TestSpecVersion()
    {
        Assert.Equal('0', Constants.SpecVersion);
    }

    [Fact]
    public void TestMagicBytes()
    {
        byte[] expectedMagic = { 137, 77, 67, 65, 80, (byte)Constants.SpecVersion, 13, 10 };
        Assert.Equal(expectedMagic, Constants.Magic);
    }

    [Fact]
    public void TestDefaultChunkSize()
    {
        Assert.Equal(1024UL * 768, Constants.DefaultChunkSize);
    }

    [Fact]
    public void TestEndOffset()
    {
        Assert.Equal(ulong.MaxValue, Constants.EndOffset);
    }

    [Fact]
    public void TestMaxTime()
    {
        Assert.Equal(ulong.MaxValue, Constants.MaxTime);
    }
}
