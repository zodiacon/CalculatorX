using System;

namespace CalculatorX.Core {
    public abstract class Expression {
        public abstract Expression Eval(IEvaluationContext context);
        public virtual double AsNumber(IEvaluationContext context) => Eval(context).AsNumber(context);

        public static implicit operator Expression(double x) {
            return new ConstantExpression(x);
        }

    }
}
