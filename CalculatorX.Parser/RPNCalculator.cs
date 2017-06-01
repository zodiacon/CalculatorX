using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace CalculatorX.Core {
    public class RPNCalculator {
        public readonly IEvaluationContext Context;
        public RPNCalculator(IEvaluationContext context) {
            Context = context;
        }

        static readonly IDictionary<string, OperatorInfo> OperatorMap =
            StandardOperators.GetAllOperators().ToDictionary(op => op.Text);

        public double Calculate(IEnumerable<Token> tokens) {
            Stack<double> stack = new Stack<double>(16);

            foreach (var token in tokens) {
                switch (token) {
                    case NumberToken n:
                        stack.Push(n.Value);
                        break;

                    case VariableToken v:
                        stack.Push(Context.GetVariableValue(v.Text));
                        break;

                    case OperatorToken op:
                        stack.Push(OperatorMap[op.Text].Eval(null, stack));
                        break;

                    case FunctionToken func:
                        var count = Context.GetFunctionArgumentCount(func.Text);
                        var args = new List<double>(count);
                        for (int i = 0; i < count; i++) {
                            args.Add(stack.Pop());
                        }

                        stack.Push(Context.EvalFunction(func.Text, args.ToArray()));
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }

            return stack.Peek();
        }

        public double Calculate(params Token[] tokens) {
            return Calculate((IEnumerable<Token>)(tokens));
        }

        public double CalculateFromInfix(IEnumerable<Token> tokens) {
            var output = new Queue<Token>(16);
            var stack = new Stack<Token>(16);

            foreach (var token in tokens) {
                switch (token) {
                    case InvalidToken _:
                        throw new ArgumentException($"Invalid token: {token.Text}");

                    case NumberToken _:
                    case VariableToken _:
                        // if a token is a number or variable, push to output
                        output.Enqueue(token);
                        break;

                    case OperatorToken _ when token.Text == ",":
                        // function argument separator
                        while (stack.Count > 0 && stack.Peek().Text != "(") {
                            output.Enqueue(stack.Pop());
                        }
                        if (stack.Count == 0)
                            throw new ArgumentException("Unexpected ','");
                        break;

                    case OperatorToken _ when token.Text == "(":
                        stack.Push(token);
                        break;

                    case OperatorToken _ when token.Text == ")":
                        try {
                            int args = 0;
                            while (stack.Peek().Text != "(") {
                                output.Enqueue(stack.Pop());
                                args++;
                            }
                            
                        }
                        catch (InvalidOperationException) {
                            // no left paren - mismatch
                            throw new ArgumentException($"Missing '('");
                        }

                        stack.Pop();    // left paren popped

                        if (stack.Count > 0 && stack.Peek().Type == TokenType.Function) {
                            output.Enqueue(stack.Pop());
                        }

                        break;

                    case OperatorToken op:
                        var operatorInfo = OperatorMap[op.Text];
                        Debug.Assert(operatorInfo != null);
                        if (operatorInfo == null)
                            throw new ArgumentException($"Unknown operator: '{op.Text}'");

                        if (stack.Count > 0) {
                            for (var op2 = stack.Peek() as OperatorToken; op2 != null && op2.Text != "(" && op2.Text != ")" &&
                                (operatorInfo.Associativity == OperatorAssociativity.LeftAssociative && operatorInfo.Precedence <= OperatorMap[op2.Text].Precedence) ||
                                (operatorInfo.Associativity == OperatorAssociativity.RightAssociative && operatorInfo.Precedence < OperatorMap[op2.Text].Precedence);
                                op2 = stack.Count > 0 ? stack.Peek() as OperatorToken : null) {
                                var t = stack.Pop();
                                output.Enqueue(t);
                            }
                        }
                        stack.Push(op);            
                        break;

                    case FunctionToken _:
                        stack.Push(token);
                        break;

                }
            }

            while (stack.Count > 0) {
                var item = stack.Peek();
                if (item.Text == "(" || item.Text == ")")
                    throw new ArgumentException($"Mismatched '{item.Text}'");
                output.Enqueue(stack.Pop());
            }

            return Calculate(output);
        }
    }
}
