using CalculatorX.Parser;
using System;
using System.Linq.Expressions;

namespace CalcX {
    class Program {
        static void Main(string[] args) {
            Console.Title = "CalcX (C)2017 by Pavel Yosifovich";

            var context = new EvaluationContext();
            var calculator = new RPNCalculator(context);
            var tokenizer = new Tokenizer();

            do {
                Console.Write(">> ");
                var input = Console.ReadLine();
                if (input.ToLowerInvariant() == "exit")
                    break;

                if (input[0] == '$') {
                    HandleSpecialCommand(context, input);
                    Console.WriteLine();
                    continue;
                }
                try {
                    var result = calculator.CalculateFromInfix(tokenizer.Tokenize(input));
                    Console.WriteLine(result);
                    context.SetVariableValue("ans", result);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine();
            } while (true);
        }

        private static void HandleSpecialCommand(IEvaluationContext context, string input) {
            switch (input.ToUpper()) {
                case "$RAD":
                    context.DegreeMode = DegreesMode.Radians;
                    Console.WriteLine("Degree mode set to radians.");
                    break;

                case "$DEG":
                    context.DegreeMode = DegreesMode.Degrees;
                    Console.WriteLine("Degree mode set to degrees.");
                    break;

                case string set when set.StartsWith("$SET"):
                    Console.WriteLine($"Mode={context.DegreeMode}");
                    break;

                case "$VAR":
                    foreach (var name in context.Variables) {
                        Console.WriteLine($"{name}={context.GetVariableValue(name)}");
                    }
                    break;

                case "$CONST":
                    foreach (var name in context.Constants) {
                        Console.WriteLine($"{name}={context.GetVariableValue(name)}");
                    }
                    break;

                default:
                    Console.WriteLine("Unknown directive.");
                    break;
            }
        }
    }
}
