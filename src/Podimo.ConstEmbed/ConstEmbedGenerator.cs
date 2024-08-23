using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Podimo.ConstEmbed;

[Generator]
public class ConstEmbedGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var globalOptions = context.AnalyzerConfigOptionsProvider.Select(static (provider, _) =>
        (
            Namespace: provider.GetGlobalOptionOrDefault("ConstEmbedNamespace", "Podimo.ConstEmbed"),
            Visibility: provider.GetGlobalOptionOrDefault("ConstEmbedVisibility", "internal")
        ));

        var additionalFiles =
            context.AdditionalTextsProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Select(static (pair, token) =>
                {
                    var @class = pair.Right.GetAdditionalFileMetadata(pair.Left, "ConstEmbed");
                    return (Class: @class, File: pair.Left);
                })
                .Where(static pair => !string.IsNullOrEmpty(pair.Class));

        var combined = additionalFiles.Combine(globalOptions);

        context.RegisterSourceOutput(combined, static (spc, pair) =>
        {
            var @namespace = pair.Right.Namespace;
            var visibility = pair.Right.Visibility;
            var @class = pair.Left.Class;
            var file = pair.Left.File;
            var filename = Path.GetFileNameWithoutExtension(file.Path);
            var escapedContent = file
                .GetText()?
                .ToString()?
                .Replace("\"", "\"\"");

            spc.AddSource(
                hintName: $"{@class}.{filename}.g.cs",
                source: $@"namespace {@namespace}
{{
    {visibility} static partial class {@class}
    {{
        public const string {filename} = @""{escapedContent}"";
    }}
}}");
        });
    }
}