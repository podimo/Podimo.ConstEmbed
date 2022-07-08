using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Podimo.ConstEmbedTestHelpers;

public class TestAdditionalText : AdditionalText
{
    private readonly SourceText _content;

    public TestAdditionalText(string path, string content)
    {
        if (content == null) throw new ArgumentNullException(nameof(content));
        Path = path ?? throw new ArgumentNullException(nameof(path));
        _content = SourceText.From(content, Encoding.UTF8);
    }

    public override string Path { get; }

    public override SourceText GetText(CancellationToken cancellationToken = default) => _content;
}