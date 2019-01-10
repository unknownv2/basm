using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Basm.Core.CodeAnalysis.Binding
{
    public abstract class BoundInstructionNode : BoundNode
    {
        public object Instruction;
        public ImmutableArray<BoundExpression> Operands;
    }
}
