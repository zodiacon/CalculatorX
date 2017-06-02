using CalculatorX.Core;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCalc.ViewModels {
    class CalculatorViewModel : BindableBase {
        RPNCalculator _calculator;
        IEvaluationContext _context;
        Parser _parser;
        Tokenizer _tokenizer = new Tokenizer();
        static string NewLine = Environment.NewLine + Environment.NewLine;
        List<string> _expressionHistory = new List<string>(64);
        int _expressionHistoryIndex;

        public CalculatorViewModel() {
            _context = new EvaluationContext();
            _calculator = new RPNCalculator(_context);
            _parser = new Parser(_calculator);

            CalculateCommand = new DelegateCommand(() => DoCalculation(), () => !string.IsNullOrWhiteSpace(Text))
                .ObservesProperty(() => Text);
            ClearHistoryCommand = new DelegateCommand(() => HistoryText = string.Empty);

            HistoryDownCommand = new DelegateCommand(() => {
                if (_expressionHistoryIndex >= _expressionHistory.Count - 1)
                    return;
                Text = _expressionHistory[++_expressionHistoryIndex];
                //if (tb != null)
                //    tb.CaretIndex = CommandText.Length;
            });

            HistoryUpCommand = new DelegateCommand(() => {
                if (_expressionHistoryIndex == 0)
                    return;
                Text = _expressionHistory[--_expressionHistoryIndex];
                //if (tb != null)
                //    tb.CaretIndex = CommandText.Length;
            });
        }
        private void DoCalculation() {
            try {
                var result = _parser.Parse(_tokenizer.Tokenize(Text));
                _context.SetVariableValue("ans", result);
                HistoryText += Text + Environment.NewLine + result.ToString();
            }
            catch (Exception ex) {
                HistoryText += Text + Environment.NewLine + ex.Message;
            }
            HistoryText += NewLine;
            _expressionHistory.Add(Text);
            _expressionHistoryIndex = _expressionHistory.Count;
            Text = string.Empty;
        }

        private string _text;

        public string Text {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _historyText;

        public string HistoryText {
            get => _historyText;
            set => SetProperty(ref _historyText, value);
        }

        public DelegateCommandBase CalculateCommand { get; }
        public DelegateCommandBase ClearHistoryCommand { get; }
        public DelegateCommandBase HistoryDownCommand { get; }
        public DelegateCommandBase HistoryUpCommand { get; }


        private bool _isDegrees = true, _isRadians;

        public bool IsDegrees {
            get => _isDegrees;
            set {
                if (SetProperty(ref _isDegrees, value) && value) {
                    _context.DegreeMode = DegreesMode.Degrees;
                }
            }
        }

        public bool IsRadians {
            get => _isRadians;
            set {
                if (SetProperty(ref _isRadians, value) && value) {
                    _context.DegreeMode = DegreesMode.Radians;
                }
            }
        }

    }
}
