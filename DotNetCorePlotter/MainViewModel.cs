using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using DotNetCorePlotter.Mvvm;
using DotNetCorePlotter.Utils;
using OxyPlot;

namespace DotNetCorePlotter
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private FunctionType functionType = FunctionType.Linear;
        private string variableA = string.Empty;

        private string variableB = string.Empty;

        public MainViewModel(IFileLoader fileLoader)
        {
            this.FileLoader = fileLoader;
            LoadFileCommand = new DelegateCommand(LoadFileCallback);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<DataPoint> DataPoints { get; private set; } = null;

        public FunctionType FunctionType
        {
            get => functionType;
            set
            {
                if (functionType != value)
                {
                    functionType = value;
                    OnPropertyChanged(nameof(FunctionType));
                    OnPropertyChanged(nameof(ResultingFunction));
                }
            }
        }

        public bool IsValidPlotLoaded { get; private set; }
        public ICommand LoadFileCommand { get; }

        public string ResultingFunction
        {
            get
            {
                string functionString = null;
                switch (FunctionType)
                {
                    case FunctionType.Linear:
                        functionString = "y = (a * x) + b";
                        break;

                    case FunctionType.Exponential:
                        functionString = "y = a * exp (b * x)";
                        break;

                    case FunctionType.PowerFunction:
                        functionString = "y = a * (x ^ b)";
                        break;
                }

                if (!string.IsNullOrWhiteSpace(VariableA))
                {
                    functionString = functionString.Replace("a", VariableA);
                }
                if (!string.IsNullOrWhiteSpace(VariableB))
                {
                    functionString = functionString.Replace("b", VariableB);
                }

                return functionString;
            }
        }

        public string VariableA
        {
            get => variableA;
            set
            {
                // TODO add validation of number
                if (variableA != value)
                {
                    variableA = value;
                    OnPropertyChanged(nameof(VariableA));
                    OnPropertyChanged(nameof(ResultingFunction));
                }
            }
        }

        public string VariableB
        {
            get => variableB;
            set
            {
                // TODO add validation of number
                if (variableB != value)
                {
                    variableB = value;
                    OnPropertyChanged(nameof(VariableB));
                    OnPropertyChanged(nameof(ResultingFunction));
                }
            }
        }

        private IFileLoader FileLoader { get; }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void LoadFileCallback()
        {
            this.DataPoints = this.FileLoader.DisplayDialogueAndLoadDataPoints();
            this.IsValidPlotLoaded = this.DataPoints.Any();
            OnPropertyChanged(nameof(DataPoints));
            OnPropertyChanged(nameof(IsValidPlotLoaded));
        }
    }
}