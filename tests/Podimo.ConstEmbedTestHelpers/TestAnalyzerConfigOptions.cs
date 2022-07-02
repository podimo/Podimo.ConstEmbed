using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Podimo.ConstEmbedTestHelpers;

public class TestAnalyzerConfigOptions : AnalyzerConfigOptions
{
    private readonly IImmutableDictionary<string, string> _options;

    public TestAnalyzerConfigOptions(IImmutableDictionary<string, string> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public static TestAnalyzerConfigOptions Empty => new(ImmutableDictionary<string, string>.Empty);

    public override bool TryGetValue(string key, out string value)
    {
        return _options.TryGetValue(key, out value!);
    }
}