using System;
using System.Linq;
using System.Reflection;

namespace CalculatorX.Parser {
    public static class StandardOperators {
        public static readonly OperatorInfo Add = new OperatorInfo {
            Text = "+",
            Type = OperatorType.Add,
            Precedence = 100,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => stack.Pop() + stack.Pop()
        };

        public static readonly OperatorInfo Subtract = new OperatorInfo {
            Text = "-",
            Type = OperatorType.Subtract,
            Precedence = 100,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => -stack.Pop() + stack.Pop()
        };

        public static readonly OperatorInfo Multiply = new OperatorInfo {
            Text = "*",
            Type = OperatorType.Multiply,
            Precedence = 150,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => stack.Pop() * stack.Pop()
        };

        public static readonly OperatorInfo Divide = new OperatorInfo {
            Text = "/",
            Type = OperatorType.Divide,
            Precedence = 150,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => {
                var b = stack.Pop();
                if (b == 0)
                    throw new DivideByZeroException();
                var a = stack.Pop();
                return a / b;
            }
        };

        public static readonly OperatorInfo Power = new OperatorInfo {
            Text = "**",
            Type = OperatorType.Power,
            Precedence = 200,
            Associativity = OperatorAssociativity.RightAssociative,
            Eval = (context, stack) => {
                var b = stack.Pop();
                var a = stack.Pop();
                return Math.Pow(a, b);
            }
        };

        static readonly OperatorInfo[] _allOperators = typeof(StandardOperators).GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(fi => fi.GetValue(null)).Cast<OperatorInfo>().ToArray();

        public static OperatorInfo[] GetAllOperators() => _allOperators;
    }
}
