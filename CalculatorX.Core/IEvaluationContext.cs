using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorX.Core {
    public enum DegreesMode {
        Degrees,
        Radians
    }

    public interface IEvaluationContext {
        Expression GetVariableValue(string name);
        void SetVariableValue(string name, Expression value);
        void SetConstantValue(string name, double value);
        //void SetFunctionExpression(string name, Expression body);

        IReadOnlyList<string> Constants { get; }
        IReadOnlyList<string> Variables { get; }
    }
}
