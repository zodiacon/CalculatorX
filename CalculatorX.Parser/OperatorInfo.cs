using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorX.Parser {
    public enum OperatorAssociativity {
        LeftAssociative,
        RightAssociative
    }

    public delegate double OperatorDelegate(IEvaluationContext context, Stack<double> stack);

    public class OperatorInfo {
        public string Text { get; set; }
        public OperatorType Type { get; set; }
        public int Precedence { get; set; }
        public OperatorAssociativity Associativity { get; set; }
        public OperatorDelegate Eval { get; set; }
    }
}
