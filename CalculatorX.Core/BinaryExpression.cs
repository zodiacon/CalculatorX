using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorX.Core {
    public sealed class BinaryExpression : Expression {
        public Expression Left { get; }
        public Expression Right { get; }
        public IBinaryOperator Operator { get; }

        public BinaryExpression(Expression left, IBinaryOperator @operator, Expression right) {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            Left = left;
            Right = right;
            Operator = @operator;
        }

        public override Expression Eval(IEvaluationContext context) {
            return Operator.Eval(Left, Right, context);
        }
    }
}
