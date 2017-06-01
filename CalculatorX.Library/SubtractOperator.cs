using CalculatorX.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorX.Library {
    sealed class SubtractOperator : IBinaryOperator {
        public Expression Eval(Expression left, Expression right, IEvaluationContext context) {
            var leftValue = left.AsNumber(context);
            var rightValue = right.AsNumber(context);

            return new ConstantExpression(leftValue - rightValue);
        }
    }
}
