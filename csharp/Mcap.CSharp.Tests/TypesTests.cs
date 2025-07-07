using Mcap.CSharp.Mcap;

namespace Mcap.CSharp.Tests;

public class TypesTests
{
    [Fact]
    public void CanCreateMcapWriterOptions()
    {
        var options = new McapWriterOptions();
        Assert.NotNull(options);
    }

    [Fact]
    public void McapWriterOptions_CanSetProfile()
    {
        var options = new McapWriterOptions { Profile = "test_profile" };
        Assert.Equal("test_profile", options.Profile);
    }

    [Fact]
    public void McapWriterOptions_CanSetChunkSize()
    {
        var options = new McapWriterOptions { ChunkSize = 1024 };
        Assert.Equal(1024UL, options.ChunkSize);
    }

    [Fact]
    public void McapWriterOptions_CanSetCompression()
    {
        var options = new McapWriterOptions { Compression = Compression.Lz4 };
        Assert.Equal(Compression.Lz4, options.Compression);
    }

    [Fact]
    public void McapWriterOptions_CanSetCompressionLevel()
    {
        var options = new McapWriterOptions { CompressionLevel = CompressionLevel.Fast };
        Assert.Equal(CompressionLevel.Fast, options.CompressionLevel);
    }

    [Fact]
    public void McapWriterOptions_CanSetBooleanFlags()
    {
        var options = new McapWriterOptions
        {
            NoChunking = true,
            ForceCompression = true
        };
        Assert.True(options.NoChunking);
        Assert.True(options.ForceCompression);
    }
}
