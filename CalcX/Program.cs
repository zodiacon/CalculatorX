using CalculatorX.Parser;
using System;
using System.Linq.Expressions;

namespace CalcX {
    class Program {
        static void Main(string[] args) {
            var context = new EvaluationContext();
            context.SetVariableValue("x", 5);

            var rpnCalc = new RPNCalculator(context);
            var result = rpnCalc.Calculate(new NumberToken("10", 10), new VariableToken("x"), new OperatorToken("+", OperatorType.Add));
            Console.WriteLine(result);

            var text = "12.9+5-3.12*cos(12) + zzz";
            var tokenizer = new Tokenizer();
            foreach (var token in tokenizer.Tokenize(text)) {
                Console.WriteLine($"{token.Text} {token.GetType().Name}");
            }

            context.SetFunctionExpression("sin", (ctx, x) => Math.Sin(ctx.DegreeMode == DegreesMode.Degrees ? x[0] * Math.PI / 180 : x[0]), 1);
            context.SetFunctionExpression("sin", (ctx, x) => Math.Sin(ctx.DegreeMode == DegreesMode.Degrees ? x[0] * Math.PI / 180 : x[0]), 1);

            var expr = "3+4-(2-4)+sin(30)";
            result = rpnCalc.CalculateFromInfix(tokenizer.Tokenize(expr));
            Console.WriteLine($"{expr}={result}");

            expr = "3+(sin(60)-4) +sin(30)";
            result = rpnCalc.CalculateFromInfix(tokenizer.Tokenize(expr));
            Console.WriteLine($"{expr}={result}");

            expr = "3*12+2**3";
            result = rpnCalc.CalculateFromInfix(tokenizer.Tokenize(expr));
            Console.WriteLine($"{expr}={result}");
        }
    }
}
