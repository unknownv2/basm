using System;
using Xunit;
using Basm.Scripts.CheatEngine.CodeAnalysis.Syntax;

namespace Basm.CheatEngine.Tests
{
    public class ScriptParserTests
    {
        [Fact]
        public void ShouldParseScriptSectionName()
        {
            var section = ParseSection("Script1");

            Assert.Equal("ENABLE", section.SectionName.Text);
        }

        private static SectionStatementSyntax ParseSection(string scriptName)
        {
            var section = CheatEngineSyntaxTree.Parse(TestResources.GetScriptText(scriptName)).Root.SectionStatement;
            Assert.NotNull(section);
            return section;
        }
    }
}
