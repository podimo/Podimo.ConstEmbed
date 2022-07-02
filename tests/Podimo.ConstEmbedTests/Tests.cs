using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Podimo.ConstEmbedTestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Podimo.ConstEmbedTests
{
    public class Tests
    {
        private readonly ITestOutputHelper _output;

        public Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ConstEmbedGenerator_ProducesExpectedFile_WithDefaults()
        {
            var globalOptions = TestAnalyzerConfigOptions.Empty;
            var textOptions = new Dictionary<AdditionalText, AnalyzerConfigOptions>
            {
                [new TestAdditionalText("Text1", "content1")] = new TestAnalyzerConfigOptions(
                    new Dictionary<string, string>
                    {
                        ["build_metadata.AdditionalFiles.ConstEmbed"] = "Text1",
                    }.ToImmutableDictionary()),
                [new TestAdditionalText("Text2", "content2")] = TestAnalyzerConfigOptions.Empty,
            };

            var (diagnostics, output) = OutputGenerator.GetGeneratedOutput(
                globalOptions,
                textOptions
            );

            _output.WriteLine(output);

            Assert.Empty(diagnostics);
            Assert.Contains("namespace Podimo.ConstEmbed", output);
            Assert.Contains("internal", output);
            Assert.Contains("Text1", output);
            Assert.Contains("content1", output);
            Assert.DoesNotContain("Text2", output);
            Assert.DoesNotContain("content2", output);
        }

        [Fact]
        public void ConstEmbedGenerator_ProducesExpectedNamespaceAndVisibility_WithGlobalOptionsSet()
        {
            var globalOptions = new TestAnalyzerConfigOptions(new Dictionary<string, string>
            {
                ["build_property.ConstEmbedNamespace"] = "Different.Namespace",
                ["build_property.ConstEmbedVisibility"] = "public",
            }.ToImmutableDictionary());

            var textOptions = new Dictionary<AdditionalText, AnalyzerConfigOptions>
            {
                [new TestAdditionalText("Text1", "content1")] = new TestAnalyzerConfigOptions(
                    new Dictionary<string, string>
                    {
                        ["build_metadata.AdditionalFiles.ConstEmbed"] = "Text1",
                    }.ToImmutableDictionary()),
                [new TestAdditionalText("Text2", "content2")] = TestAnalyzerConfigOptions.Empty,
            };

            var (diagnostics, output) = OutputGenerator.GetGeneratedOutput(
                globalOptions,
                textOptions
            );

            _output.WriteLine(output);

            Assert.Empty(diagnostics);
            Assert.Contains("namespace Different.Namespace", output);
            Assert.Contains("public", output);
            Assert.Contains("Text1", output);
            Assert.Contains("content1", output);
            Assert.DoesNotContain("Text2", output);
            Assert.DoesNotContain("content2", output);
        }
    }
}