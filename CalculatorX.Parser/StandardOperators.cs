using System;
using System.Linq;
using System.Reflection;

namespace CalculatorX.Core {
    public static class StandardOperators {
        public static readonly OperatorInfo Add = new OperatorInfo {
            Text = "+",
            Precedence = 100,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => stack.Pop() + stack.Pop()
        };

        public static readonly OperatorInfo Subtract = new OperatorInfo {
            Text = "-",
            Precedence = 100,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => -stack.Pop() + stack.Pop()
        };

        public static readonly OperatorInfo Multiply = new OperatorInfo {
            Text = "*",
            Precedence = 150,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => stack.Pop() * stack.Pop()
        };

        public static readonly OperatorInfo Divide = new OperatorInfo {
            Text = "/",
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

        public static readonly OperatorInfo Modulo = new OperatorInfo {
            Text = "%",
            Precedence = 150,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => {
                var b = stack.Pop();
                if (b == 0)
                    throw new DivideByZeroException();
                var a = stack.Pop();
                return a % b;
            }
        };

        public static readonly OperatorInfo Power = new OperatorInfo {
            Text = "**",
            Precedence = 200,
            Associativity = OperatorAssociativity.RightAssociative,
            Eval = (context, stack) => {
                var b = stack.Pop();
                var a = stack.Pop();
                return Math.Pow(a, b);
            }
        };

        public static readonly OperatorInfo And = new OperatorInfo {
            Text = "&",
            Precedence = 250,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => (long)stack.Pop() & (long)stack.Pop()
        };

        public static readonly OperatorInfo Or = new OperatorInfo {
            Text = "|",
            Precedence = 240,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => (long)stack.Pop() | (long)stack.Pop()
        };

        public static readonly OperatorInfo Xor = new OperatorInfo {
            Text = "^",
            Precedence = 240,
            Associativity = OperatorAssociativity.LeftAssociative,
            Eval = (context, stack) => (long)stack.Pop() ^ (long)stack.Pop()
        };

        public static readonly OperatorInfo Not = new OperatorInfo {
            Text = "~",
            Precedence = 300,
            Eval = (context, stack) => ~(long)stack.Pop()
        };

        static readonly OperatorInfo[] _allOperators = typeof(StandardOperators)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(fi => fi.GetValue(null)).Cast<OperatorInfo>().ToArray();

        public static OperatorInfo[] GetAllOperators() => _allOperators;
    }
}
