using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorX.Parser {
    public interface IOperator {
        double Eval(Stack<double> stack);
    }
}
