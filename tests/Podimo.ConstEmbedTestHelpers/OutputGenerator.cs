using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Podimo.ConstEmbed;

namespace Podimo.ConstEmbedTestHelpers;

public static class OutputGenerator
{
    public static (ImmutableArray<Diagnostic>, string) GetGeneratedOutput(
        AnalyzerConfigOptions globalOptions,
        Dictionary<AdditionalText, AnalyzerConfigOptions> textOptions
    )
    {
        var compilation = CSharpCompilation.Create(
            assemblyName: null,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        var diagnostics = compilation.GetDiagnostics();

        if (diagnostics.Any())
        {
            return (diagnostics, "");
        }

        var generator = new ConstEmbedGenerator();

        CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(textOptions.Keys.ToImmutableArray())
            .WithUpdatedAnalyzerConfigOptions(newOptions: new TestAnalyzerConfigOptionsProvider(
                globalOptions: globalOptions,
                textOptions: textOptions
            ))
            .RunGeneratorsAndUpdateCompilation(
                compilation,
                out var outputCompilation,
                out var generateDiagnostics
            );

        return (generateDiagnostics, outputCompilation.SyntaxTrees.Last().ToString());
    }
}