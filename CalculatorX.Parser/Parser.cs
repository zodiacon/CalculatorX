using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorX.Core {

    // currently, very simple parser for assigning variables

    public class Parser {
        readonly IEvaluationContext _context;
        readonly RPNCalculator _calculator;

        public Parser(RPNCalculator calculator) {
            _calculator = calculator;
            _context = calculator.Context;
        }

        public double Parse(IEnumerable<Token> tokens) {
            var first = tokens.First();
            var second = tokens.ElementAtOrDefault(1);
            if (second != null) {
                if (first is VariableToken && second.Text == "=") {
                    var value = _calculator.CalculateFromInfix(tokens.Skip(2));
                    _context.SetVariableValue(first.Text, value);
                    return value;
                }
            }
            return _calculator.CalculateFromInfix(tokens);
        }

    }
}
