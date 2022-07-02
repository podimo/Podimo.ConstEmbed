using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Podimo.ConstEmbedTestHelpers;

var textOptions = new Dictionary<AdditionalText, AnalyzerConfigOptions>
{
    [new TestAdditionalText("Text1", "content1")] = new TestAnalyzerConfigOptions(
        new Dictionary<string, string>
        {
            ["build_metadata.AdditionalFiles.ConstEmbed"] = "Text1",
        }.ToImmutableDictionary()
    ),
    [new TestAdditionalText("Text2", "content2")] = TestAnalyzerConfigOptions.Empty,
};

var (diagnostics, output) = OutputGenerator.GetGeneratedOutput(
    TestAnalyzerConfigOptions.Empty,
    textOptions
);

if (diagnostics.Length > 0)
{
    Console.WriteLine("Diagnostics:");
    foreach (var diagnostic in diagnostics)
    {
        Console.WriteLine("   " + diagnostic);
    }

    Console.WriteLine();
    Console.WriteLine("Output:");
}

Console.WriteLine(output);