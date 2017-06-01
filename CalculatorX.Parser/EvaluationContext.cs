using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorX.Parser {
    public class FunctionInfo {
        public readonly FunctionDelegate Delegate;
        public readonly int Arguments;

        public FunctionInfo(FunctionDelegate function, int arguments) {
            Delegate = function;
            Arguments = arguments;
        }
    }

    public class EvaluationContext : IEvaluationContext {
        Dictionary<string, double> _variables = new Dictionary<string, double>(64, StringComparer.InvariantCultureIgnoreCase);
        Dictionary<string, double> _constants = new Dictionary<string, double>(8, StringComparer.InvariantCultureIgnoreCase);
        Dictionary<string,  FunctionInfo> _functions = new Dictionary<string, FunctionInfo>(64, StringComparer.InvariantCultureIgnoreCase);

        public EvaluationContext() {
            // add common variables
            SetConstantValue("pi", Math.PI);
            SetConstantValue("e", Math.E);

            AddStandardFunctions();
        }

        public IReadOnlyList<string> Constants => _constants.Keys.ToList();

        public IReadOnlyList<string> Variables => _variables.Keys.ToList();

        public IReadOnlyList<string> Functions => _functions.Keys.ToList();

        public DegreesMode DegreeMode { get; set; } = DegreesMode.Degrees;

        public double EvalFunction(string name, params double[] args) {
            return _functions[name].Delegate(this, args);
        }

        public int GetFunctionArgumentCount(string name) {
            return _functions[name].Arguments;
        }

        public virtual double GetVariableValue(string name) {
            if (_constants.TryGetValue(name, out var value))
                return value;

            return _variables[name];
        }

        public void SetConstantValue(string name, double value) {
            FailIfConstant(name);
            _constants.Add(name, value);
        }

        public void SetFunctionExpression(string name, FunctionDelegate body, int arguments = 1) {
            _functions[name] = new FunctionInfo(body, arguments);
        }

        public virtual void SetVariableValue(string name, double value) {
            FailIfConstant(name);
            _variables[name] = value;
        }

        private void FailIfConstant(string name) {
            if (_constants.ContainsKey(name))
                throw new InvalidOperationException($"constant {name} cannot be changed");
        }

        void AddStandardFunctions() {
            SetFunctionExpression("sin", (ctx, x) => Math.Sin(ctx.DegreeMode == DegreesMode.Degrees ? x[0] * Math.PI / 180 : x[0]));
            SetFunctionExpression("cos", (ctx, x) => Math.Cos(ctx.DegreeMode == DegreesMode.Degrees ? x[0] * Math.PI / 180 : x[0]));
            SetFunctionExpression("tan", (ctx, x) => Math.Tan(ctx.DegreeMode == DegreesMode.Degrees ? x[0] * Math.PI / 180 : x[0]));
            SetFunctionExpression("cot", (ctx, x) => 1.0 / Math.Tan(ctx.DegreeMode == DegreesMode.Degrees ? x[0] * Math.PI / 180 : x[0]));
            SetFunctionExpression("ln", (ctx, x) => Math.Log(x[0]));
            SetFunctionExpression("exp", (ctx, x) => Math.Exp(x[0]));
            SetFunctionExpression("max", (ctx, x) => Math.Max(x[0], x[1]), 2);
            SetFunctionExpression("min", (ctx, x) => Math.Min(x[0], x[1]), 2);
        }
    }
}
