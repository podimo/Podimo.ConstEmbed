using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

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
                .Select(static (pair, _) =>
                {
                    var @class = pair.Right.GetAdditionalFileMetadata(pair.Left, "ConstEmbed");
                    return (Class: @class, File: pair.Left);
                })
                .Where(static pair => pair.Class is not null);

        var combined = additionalFiles.Combine(globalOptions);

        context.RegisterSourceOutput(combined, static (spc, pair) =>
        {
            var @namespace = pair.Right.Namespace;
            var visibility = pair.Right.Visibility;
            var @class = pair.Left.Class;
            var file = pair.Left.File;
            var filename = Path.GetFileNameWithoutExtension(file.Path);
            var sourceText = file.GetText();

            if (@class is null) return;
            if (sourceText is null) return;

            var outputSourceText = BuildWithStringReplace(filename, @namespace, visibility, @class, sourceText);

            spc.AddSource(
                hintName: $"{@class}.{filename}.g.cs",
                sourceText: outputSourceText
            );
        });
    }

    private static readonly StringBuilder Sb = new();

    public static SourceText BuildWithStringBuilder(
        string filename,
        string @namespace,
        string visibility,
        string @class,
        SourceText sourceText)
    {
        Sb.Clear();
        Sb.Append(sourceText).Replace("\"", "\"\"");

        return SourceText.From(Sb.ToString(), sourceText.Encoding);
    }

    public static SourceText BuildWithSourceTextChanges(
        string filename,
        string @namespace,
        string visibility,
        string @class,
        SourceText sourceText)
    {
        List<TextChange> changes = new()
        {
            new TextChange(
                span: TextSpan.FromBounds(0, 0),
                newText: $@"namespace {@namespace}
{{
    {visibility} static partial class {@class}
    {{
        public const string {filename} = @"""
            ),
            new TextChange(
                span: TextSpan.FromBounds(sourceText.Length, sourceText.Length),
                newText: @""";
    }}
}}"
            )
        };

        for (int i = 0; i < sourceText.Length; i++)
        {
            if (sourceText[i] == '"')
            {
                changes.Add(new TextChange(TextSpan.FromBounds(i, i + 1), @""""""));
            }
        }

        return sourceText.WithChanges(changes);
    }

    public static SourceText BuildWithStringReplace(
        string filename,
        string @namespace,
        string visibility,
        string @class,
        SourceText sourceText)
    {
        var escapedOriginalFileContents = sourceText.ToString().Replace("\"", "\"\"");
        var outputFileContents = $@"namespace {@namespace}
{{
    {visibility} static partial class {@class}
    {{
        public const string {filename} = @""{escapedOriginalFileContents}"";
    }}
}}";
        return SourceText.From(outputFileContents, sourceText.Encoding);
    }
}