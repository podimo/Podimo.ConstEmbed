using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.Text;
using Podimo.ConstEmbed;

namespace Podimo.ConstEmbedBenchmarks;

public class TextProcessingMethods
{
    private readonly SourceText _ipsum = SourceText.From(Generated.Files.LoremIpsum);
    private readonly SourceText _quotes = SourceText.From(Generated.Files.LotsOfQuotes);

    private static SourceText BuildWrapper(
        Func<string, string, string, string, SourceText, SourceText> fn,
        SourceText text
    ) => fn("Filename", "Namespace", "internal", "class", text);


    [Benchmark]
    public SourceText SourceTextChanges_Ipsum() =>
        BuildWrapper(ConstEmbedGenerator.BuildWithSourceTextChanges, _ipsum);

    [Benchmark]
    public SourceText StringReplace_Ipsum() =>
        BuildWrapper(ConstEmbedGenerator.BuildWithStringReplace, _ipsum);

    [Benchmark]
    public SourceText StringBuilder_Ipsum() =>
        BuildWrapper(ConstEmbedGenerator.BuildWithStringBuilder, _ipsum);

    [Benchmark]
    public SourceText SourceTextChanges_Quotes() =>
        BuildWrapper(ConstEmbedGenerator.BuildWithSourceTextChanges, _quotes);

    [Benchmark]
    public SourceText StringReplace_Quotes() =>
        BuildWrapper(ConstEmbedGenerator.BuildWithStringReplace, _quotes);

    [Benchmark]
    public SourceText StringBuilder_Quotes() =>
        BuildWrapper(ConstEmbedGenerator.BuildWithStringBuilder, _quotes);
}