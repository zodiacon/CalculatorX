using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorX.Core {
    public sealed class VariableExpression : Expression {
        public string Name { get; }

        public VariableExpression(string name) {
            Name = name;
        }
        public override Expression Eval(IEvaluationContext context) {
            return context.GetVariableValue(Name);
        }
    }
}
