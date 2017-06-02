using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCalc.ViewModels {
    class MainViewModel : BindableBase {
        public CalculatorViewModel Calculator { get; } = new CalculatorViewModel();


    }
}
