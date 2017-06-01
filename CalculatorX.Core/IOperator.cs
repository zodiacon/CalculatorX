using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorX.Core {
    public interface IBinaryOperator {
        Expression Eval(Expression left, Expression right, IEvaluationContext context);
    }
}
