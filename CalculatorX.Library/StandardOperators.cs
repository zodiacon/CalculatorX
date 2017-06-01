using CalculatorX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorX.Library {
    public static class StandardOperators {
        public static readonly IBinaryOperator Add = new AddOperator();
        public static readonly IBinaryOperator Subtract = new SubtractOperator();

    }
}
