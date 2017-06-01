using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorX.Parser {
    public sealed class Tokenizer {
        enum State { None, Number, NumebrWithExp, Text, Operator, Function };

        public IEnumerable<Token> Tokenize(string text) {
            State state = State.None;
            string lexeme = string.Empty;
            Token previousToken = null;

            for (int i = 0; i < text.Length + 1; i++) {
                var ch = i == text.Length ? ' ' : text[i];

                switch (state) {
                    case State.None:
                        // skip whitespace
                        if (char.IsWhiteSpace(ch))
                            break;

                        if (ch == ')' || ch == '(') {
                            yield return (previousToken = new OperatorToken(ch.ToString(), OperatorType.Paren));
                            break;
                        }

                        if (ch == '-' || ch == '+') {
                            // could be an operator or a negative\positive sign - assume sign
                            lexeme += ch;
                            if (previousToken == null || (previousToken.Type == TokenType.Operator && previousToken.Text != ")")) {
                                // number
                                state = State.Number;
                            }
                            else {
                                state = State.Operator;
                            }
                            break;
                        }

                        if (IsOperator(ch)) {
                            // operator
                            lexeme += ch;
                            state = State.Operator;
                            break;
                        }

                        if (char.IsDigit(ch)) {
                            // number
                            lexeme += ch;
                            state = State.Number;
                        }
                        else if (char.IsLetter(ch) || ch == '_') {
                            // variable
                            lexeme += ch;
                            state = State.Text;
                        }
                        break;

                    case State.Number:
                        // is exponent used?
                        if (ch == 'E' || ch == 'e') {
                            // good mantissa?
                            if (double.TryParse(lexeme, out var mantissa)) {
                                lexeme += ch;
                                state = State.NumebrWithExp;
                                break;
                            }

                            // no, exponent premature
                            yield return new InvalidToken(lexeme + ch);
                            yield break;
                        }

                        // decimal digit?
                        if (ch == '.') {
                            if (lexeme.Contains(".")) {
                                // second digit, error
                                yield return new InvalidToken(lexeme + ch);
                                yield break;
                            }
                            lexeme += ch;
                            continue;
                        }

                        if (!char.IsDigit(ch)) {
                            if ((char.IsWhiteSpace(ch) || IsOperator(ch)) && double.TryParse(lexeme, out var value)) {
                                yield return (previousToken = new NumberToken(lexeme, value));
                                lexeme = string.Empty;
                                state = State.None;
                                i--;
                                continue;
                            }
                            yield return new InvalidToken(lexeme + ch);
                            yield break;
                        }
                        else {
                            lexeme += ch;
                        }
                        break;

                    case State.NumebrWithExp:
                        if (ch == '+' || ch == '-') {
                            // exponent sign
                            if (lexeme.IndexOf(ch) > 0) {
                                // already have one - error
                                yield return new InvalidToken(lexeme + ch);
                                yield break;
                            }
                            lexeme += ch;
                            break;
                        }
                        if (ch == '.') {
                            // error
                            yield return new InvalidToken(lexeme + ch);
                            yield break;
                        }

                        if (!char.IsDigit(ch)) {
                            // number end 
                            if (double.TryParse(lexeme, out var value)) {
                                yield return (previousToken = new NumberToken(lexeme, value));
                                lexeme = string.Empty;
                                state = State.None;
                                i--;
                                continue;
                            }
                            // error
                            yield return new InvalidToken(lexeme + ch);
                            yield break;
                        }
                        lexeme += ch;
                        break;

                    case State.Text:
                        if (char.IsLetterOrDigit(ch) || ch == '_') {
                            lexeme += ch;
                            break;
                        }

                        if (PeekOperator(text, i) == '(')
                            previousToken = new FunctionToken(lexeme);
                        else
                            previousToken = new VariableToken(lexeme);
                        yield return previousToken;
                        state = State.None;
                        lexeme = string.Empty;
                        i--;
                        break;

                    case State.Operator:
                        if (!IsParen(ch) && IsOperator(ch)) {
                            lexeme += ch;
                        }
                        else {
                            // end of operator
                            yield return (previousToken = new OperatorToken(lexeme, StringToOperatorType(lexeme)));
                            state = State.None;
                            i--;
                            lexeme = string.Empty;
                        }
                        break;
                }
            }
        }

        private bool IsParen(char ch) {
            return ch == '(' || ch == ')';
        }

        private char PeekOperator(string text, int i) {
            var len = text.Length;
            while (i < len && char.IsWhiteSpace(text[i]))
                i++;

            return i == text.Length ? '\0' : text[i];
        }

        const string operators = @"+-<>()*/\^%&|~#@!=";
        
        bool IsOperator(char ch) {
            return operators.Contains(ch);
        }

        private OperatorType StringToOperatorType(string lexeme) {
            switch (lexeme) {
                case "+": return OperatorType.Add;
                case "-": return OperatorType.Subtract;
            }
            return OperatorType.Unknown;
        }
    }
}
