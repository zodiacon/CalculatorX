using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorX.Parser {
    public enum TokenType {
        Invalid,
        Number,
        Variable,
        Operator,
        Function
    }

    public enum OperatorType {
        Unknown,
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulu,
        Power,
        And,
        Or,
        Xor,
        Not,
        Paren
    }

    [DebuggerDisplay("Type={GetType().Name}, Text={Text}")]
    public abstract class Token {
        public string Text { get; }
        public TokenType Type { get; }

        protected Token(string text, TokenType type) {
            Text = text;
            Type = type;
        }
    }

    public sealed class InvalidToken : Token {
        public InvalidToken(string text) : base(text, TokenType.Invalid) {
        }
    }

    public sealed class VariableToken : Token {
        public VariableToken(string text) : base(text, TokenType.Variable) {
        }
    }

    public sealed class FunctionToken : Token {
        public FunctionToken(string text) : base(text, TokenType.Function) {
        }
    }

    public sealed class OperatorToken : Token {
        public OperatorType Operator { get; }

        public OperatorToken(string text, OperatorType @operator) : base(text, TokenType.Operator) {
            Operator = @operator;
        }
    }

    public sealed class NumberToken : Token {
        public double Value { get; }

        public NumberToken(string text, double value) : base(text, TokenType.Number) {
            Value = value;
        }
    }
}
