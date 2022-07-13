using BenchmarkDotNet.Running;

namespace Podimo.ConstEmbedBenchmarks;

public class Program
{
    public static void Main()
    {
        var summary = BenchmarkRunner.Run<TextProcessingMethods>();
    }
}
