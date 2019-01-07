using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Scripting;

namespace Basm.Assemblers.ScriptAssembler
{
    public interface ISection
    {
        string Name { get; }
        IScript State { get; }
    }
}
