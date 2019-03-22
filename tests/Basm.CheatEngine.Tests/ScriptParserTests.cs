using System;
using Xunit;
using Basm.Scripts.CheatEngine.CodeAnalysis.Syntax;

namespace Basm.CheatEngine.Tests
{
    public class ScriptParserTests
    {
        [Fact]
        public void TestParseScriptSection()
        {
            var script = CheatEngineSyntaxTree.Parse(TestResources.GetScriptText("Script1")).Root.ScriptStatement;

        }
    }
}
