using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorX.Core {
    public enum DegreesMode {
        Degrees,
        Radians
    }

    public interface IEvaluationContext {
        double GetVariableValue(string name);
        void SetVariableValue(string name, double value);
        void SetConstantValue(string name, double value);
        void SetFunctionExpression(string name, FunctionDelegate body, int args);

        double EvalFunction(string name, params double[] args);
        int GetFunctionArgumentCount(string name);

        IReadOnlyList<string> Constants { get; }
        IReadOnlyList<string> Variables { get; }
        IReadOnlyList<string> Functions { get; }

        DegreesMode DegreeMode { get; set; }
    }

    public delegate double FunctionDelegate(IEvaluationContext context, params double[] args);
}
