using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Podimo.ConstEmbedTestHelpers;

public class TestAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
{
    private readonly ImmutableDictionary<AdditionalText, AnalyzerConfigOptions> _textOptions;

    public TestAnalyzerConfigOptionsProvider(AnalyzerConfigOptions globalOptions,
        IEnumerable<KeyValuePair<AdditionalText, AnalyzerConfigOptions>> textOptions)
    {
        GlobalOptions = globalOptions ?? throw new ArgumentNullException(nameof(globalOptions));
        _textOptions = textOptions?.ToImmutableDictionary() ?? throw new ArgumentNullException(nameof(textOptions));
    }

    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
    {
        throw new NotSupportedException("GetOptions for SyntaxTree not supported.");
    }

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
    {
        if (_textOptions.TryGetValue(textFile, out var options))
            return options;

        return TestAnalyzerConfigOptions.Empty;
    }

    public override AnalyzerConfigOptions GlobalOptions { get; }
}