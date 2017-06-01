using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorX.Core {
    public sealed class ConstantExpression : Expression {
        public double Value { get; }
        public bool IsConstant { get; }

        public ConstantExpression(double value, bool constant = false) {
            Value = value;
            IsConstant = constant;
        }

        public override Expression Eval(IEvaluationContext context) {
            return this;
        }

        public override double AsNumber(IEvaluationContext context) => Value;

    }
}
