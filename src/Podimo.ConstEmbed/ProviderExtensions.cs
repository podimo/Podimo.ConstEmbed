using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Podimo.ConstEmbed;

public static class ProviderExtensions
{
    public static string GetGlobalOptionOrDefault(
        this AnalyzerConfigOptionsProvider provider,
        string name,
        string @default)
    {
        provider.GlobalOptions.TryGetValue($"build_property.{name}", out var value);

        return value ?? @default;
    }

    public static string? GetAdditionalFileMetadata(
        this AnalyzerConfigOptionsProvider provider,
        AdditionalText file,
        string name)
    {
        provider.GetOptions(file)
            .TryGetValue(
                $"build_metadata.AdditionalFiles.{name}",
                out var value
            );
        return value;
    }
}