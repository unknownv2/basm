using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Basm.CheatEngine.Tests
{
    public static class TestResources
    {
        private static string TestScriptDir =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Scripts");

        public static string GetScriptText(string fileName)
        {
            return File.ReadAllText($"{Path.Combine(TestScriptDir, fileName)}.asm");
        }
    }
}
