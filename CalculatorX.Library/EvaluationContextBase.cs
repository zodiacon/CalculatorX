using CalculatorX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorX.Library {
    public class EvaluationContextBase : IEvaluationContext {
        Dictionary<string, Expression> _variables = new Dictionary<string, Expression>(64, StringComparer.InvariantCultureIgnoreCase);
        Dictionary<string, double> _constants = new Dictionary<string, double>(8, StringComparer.InvariantCultureIgnoreCase);

        public EvaluationContextBase() {
            // add common variables
            SetConstantValue("pi", Math.PI);
            SetConstantValue("e", Math.E);
        }

        public IReadOnlyList<string> Constants => _constants.Keys.ToList();

        public IReadOnlyList<string> Variables => _variables.Keys.ToList();

        public virtual Expression GetVariableValue(string name) {
            if (_constants.TryGetValue(name, out var value))
                return value;

            return _variables.TryGetValue(name, out var expression) ? expression : null;
        }

        public void SetConstantValue(string name, double value) {
            FailIfConstant(name);
            _constants.Add(name, value);
        }

        public virtual void SetVariableValue(string name, Expression value) {
            FailIfConstant(name);
            _variables[name] = value;
        }

        private void FailIfConstant(string name) {
            if (_constants.ContainsKey(name))
                throw new InvalidOperationException($"constant {name} cannot be changed");
        }
    }
}
