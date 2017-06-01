using System;

namespace CalculatorX.Library {
    sealed class AddOperator : IBinaryOperator {

        public Expression Eval(Expression left, Expression right, IEvaluationContext context) {
            var leftValue = left.AsNumber(context);
            var rightValue = right.AsNumber(context);

            return new ConstantExpression((double)leftValue + (double)rightValue);
        }
    }
}
