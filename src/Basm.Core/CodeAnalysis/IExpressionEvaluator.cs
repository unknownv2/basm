using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Binding;

namespace Basm.Core.CodeAnalysis
{
    public interface IExpressionEvaluator
    {
        object EvaluateExpression(BoundExpression node);
    }
}
