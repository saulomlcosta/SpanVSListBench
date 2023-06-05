using BenchmarkDotNet.Attributes;

namespace SpanVSList;

public class SpanVsListBenchmark
{
    private byte[] largeArray;

    [Params(1000000)]
    public int ArraySize { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        largeArray = new byte[ArraySize];

        for (int i = 0; i < largeArray.Length; i++)
        {
            largeArray[i] = (byte)(i % 256);
        }
    }

    [Benchmark]
    public void SpanApproach()
    {
        ProcessLargeArrayUsingSpan(largeArray);
    }

    [Benchmark]
    public void ListApproach()
    {
        ProcessLargeArrayUsingList(largeArray);
    }

    public void ProcessLargeArrayUsingSpan(byte[] largeArray)
    {
        const int chunkSize = 1024;
        for (int i = 0; i < largeArray.Length; i += chunkSize)
        {
            int length = Math.Min(chunkSize, largeArray.Length - i);
            Span<byte> chunk = largeArray.AsSpan(i, length);
            ProcessChunkSpan(chunk);
        }
    }

    public void ProcessChunkSpan(Span<byte> chunk)
    {
        // Process the chunk of data without creating a new array
        for (int i = 0; i < chunk.Length; i++)
        {
            chunk[i] *= 2;
        }
    }

    public void ProcessLargeArrayUsingList(byte[] largeArray)
    {
        const int chunkSize = 1024;
        List<byte> chunk = new List<byte>(chunkSize);

        for (int i = 0; i < largeArray.Length; i += chunkSize)
        {
            int length = Math.Min(chunkSize, largeArray.Length - i);

            chunk.Clear();
            for (int j = i; j < i + length; j++)
            {
                chunk.Add(largeArray[j]);
            }

            ProcessChunkList(chunk);
        }
    }

    public void ProcessChunkList(List<byte> chunk)
    {
        for (int i = 0; i < chunk.Count; i++)
        {
            chunk[i] *= 2;
        }
    }
}