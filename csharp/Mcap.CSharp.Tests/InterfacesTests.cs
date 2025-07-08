using Mcap.CSharp.Mcap.Interfaces;
using Xunit;

namespace Mcap.CSharp.Tests;

public class InterfacesTests
{
    [Fact]
    public void CanCreateFileWriter()
    {
        var writer = new FileWriter();
        Assert.NotNull(writer);
        Assert.IsAssignableFrom<IWritable>(writer);
    }

    [Fact]
    public void CanCreateBufferWriter()
    {
        var writer = new BufferWriter();
        Assert.NotNull(writer);
        Assert.IsAssignableFrom<IWritable>(writer);
    }

    [Fact]
    public void IWritable_HasExpectedMembers()
    {
        // This test primarily checks for compilation errors after interface changes
        // and ensures the members exist.
        var writer = new MockIWritable();
        writer.CrcEnabled = true;
        writer.Write(Array.Empty<byte>(), 0);
        writer.End();
        writer.Size();
        writer.Crc();
        writer.ResetCrc();
        writer.Flush();

        Assert.True(true); // If we reach here, compilation passed and members exist.
    }

    [Fact]
    public void IWritable_WriteMethodIsCalledAndDataIsCorrect()
    {
        var mockWriter = new MockIWritable();
        byte[] testData = { 0x01, 0x02, 0x03 };
        ulong testSize = (ulong)testData.Length;

        mockWriter.Write(testData, testSize);

        Assert.Equal(testData, mockWriter.LastWrittenData);
        Assert.Equal(testSize, mockWriter.LastWrittenSize);
    }

    [Fact]
    public void FileWriter_WritesDataToFile()
    {
        string tempFilePath = Path.GetTempFileName();
        try
        {
            var writer = new FileWriter();
            writer.Open(tempFilePath);

            byte[] testData = { 0x01, 0x02, 0x03, 0x04, 0x05 };
            writer.Write(testData, (ulong)testData.Length);
            writer.End();

            byte[] readData = File.ReadAllBytes(tempFilePath);
            Assert.Equal(testData, readData);
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void FileWriter_SizeReturnsCorrectValue()
    {
        string tempFilePath = Path.GetTempFileName();
        try
        {
            var writer = new FileWriter();
            writer.Open(tempFilePath);

            byte[] testData1 = { 0x01, 0x02 };
            writer.Write(testData1, (ulong)testData1.Length);
            Assert.Equal((ulong)testData1.Length, writer.Size());

            byte[] testData2 = { 0x03, 0x04, 0x05 };
            writer.Write(testData2, (ulong)testData2.Length);
            Assert.Equal((ulong)(testData1.Length + testData2.Length), writer.Size());

            writer.End();
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void FileWriter_EndClosesFileStream()
    {
        string tempFilePath = Path.GetTempFileName();
        var writer = new FileWriter();
        writer.Open(tempFilePath);
        writer.Write(new byte[] { 0x01 }, 1);
        writer.End();

        // Attempt to open the file again to ensure it's closed by FileWriter.End()
        using (var stream = File.OpenRead(tempFilePath))
        {
            Assert.NotNull(stream);
        }
        File.Delete(tempFilePath);
    }

    [Fact]
    public void BufferWriter_WritesDataToBuffer()
    {
        var writer = new BufferWriter();
        byte[] testData = { 0x0A, 0x0B, 0x0C };
        writer.Write(testData, (ulong)testData.Length);
        writer.End();

        byte[] readData = writer.ToArray();
        Assert.Equal(testData, readData);
        Assert.Equal((ulong)testData.Length, writer.Size());
    }

    // Mock implementation for testing interface contract
    private class MockIWritable : IWritable
    {
        public bool CrcEnabled { get; set; }
        public byte[] LastWrittenData { get; private set; }
        public ulong LastWrittenSize { get; private set; }

        public void Write(byte[] data, ulong size)
        {
            LastWrittenData = data;
            LastWrittenSize = size;
        }
        public void End() { }
        public ulong Size() { return 0; }
        public uint Crc() { return 0; }
        public void ResetCrc() { }
        public void Flush() { }
    }
}
