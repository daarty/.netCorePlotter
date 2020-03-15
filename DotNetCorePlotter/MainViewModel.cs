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
        public const string ExponentialFunction = "y = a * exp (b * x)";
        public const double FunctionResolution = 5000;
        public const string LinearFunction = "y = (a * x) + b";
        public const string PowerFunction = "y = a * (x ^ b)";

        private double doubleValueA = 0d;
        private double doubleValueB = 0d;
        private FunctionType functionType = FunctionType.Linear;

        public MainViewModel(IFileLoader fileLoader, IMathHelper mathHelper)
        {
            this.FileLoader = fileLoader;
            this.MathHelper = mathHelper;
            LoadFileCommand = new DelegateCommand(LoadFileCallback);
            FindFunctionCommand = new DelegateCommand(FindFunction);
            DrawFunctionCommand = new DelegateCommand(DrawFunction);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<DataPoint> DataPoints { get; private set; } = new List<DataPoint>();
        public ICommand DrawFunctionCommand { get; }
        public ICommand FindFunctionCommand { get; }

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

        public List<DataPoint> GeneratedDataPoints { get; private set; } = new List<DataPoint>();
        public bool IsValidPlotLoaded { get; private set; }
        public ICommand LoadFileCommand { get; }

        public string ResultingFunction
        {
            get
            {
                string functionString = null;

                switch (functionType)
                {
                    case FunctionType.Linear:
                        functionString = LinearFunction;
                        break;

                    case FunctionType.Exponential:
                        functionString = ExponentialFunction;
                        break;

                    case FunctionType.PowerFunction:
                        functionString = PowerFunction;
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

                DrawFunction();

                return functionString;
            }
        }

        public string VariableA
        {
            get => doubleValueA.ToString();
            set
            {
                if (VariableA != value)
                {
                    if (!double.TryParse(value, out var validValue))
                    {
                        return;
                    }

                    doubleValueA = validValue;
                    OnPropertyChanged(nameof(VariableA));
                    OnPropertyChanged(nameof(ResultingFunction));
                }
            }
        }

        public string VariableB
        {
            get => doubleValueB.ToString();
            set
            {
                if (VariableB != value)
                {
                    if (!double.TryParse(value, out var validValue))
                    {
                        return;
                    }

                    doubleValueB = validValue;
                    OnPropertyChanged(nameof(VariableB));
                    OnPropertyChanged(nameof(ResultingFunction));
                }
            }
        }

        private IFileLoader FileLoader { get; }
        private IMathHelper MathHelper { get; }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void DrawFunction()
        {
            // Overwriting instead of Clear() is needed to refresh the plot.
            GeneratedDataPoints = new List<DataPoint>();
            if (!this.DataPoints.Any())
            {
                return;
            }

            var firstX = DataPoints[0].X;
            var lastX = DataPoints[DataPoints.Count - 1].X;

            var resolutionStep = (lastX - firstX) / FunctionResolution;

            for (int i = 0; i < FunctionResolution; i++)
            {
                var x = firstX + resolutionStep * i;
                var y = this.doubleValueA * x + this.doubleValueB;
                GeneratedDataPoints.Add(new DataPoint(x, y));
            }

            OnPropertyChanged(nameof(GeneratedDataPoints));
        }

        private void FindFunction()
        {
            // Overwriting instead of Clear() is needed to refresh the plot.
            GeneratedDataPoints = new List<DataPoint>();
            if (!this.DataPoints.Any())
            {
                return;
            }

            (double a, double b) tuple = (0d, 0d);

            var xData = this.DataPoints.Select(p => p.X).ToArray();
            var yData = this.DataPoints.Select(p => p.Y).ToArray();

            switch (functionType)
            {
                case FunctionType.Linear:
                    tuple = this.MathHelper.FindLinearFunction(xData, yData);
                    break;

                case FunctionType.Exponential:
                    tuple = this.MathHelper.FindLinearFunction(xData, yData);
                    break;

                case FunctionType.PowerFunction:
                    tuple = this.MathHelper.FindLinearFunction(xData, yData);
                    break;
            }

            this.VariableA = tuple.a.ToString();
            this.VariableB = tuple.b.ToString();

            this.DrawFunction();
        }

        private void LoadFileCallback()
        {
            this.DataPoints = this.FileLoader.DisplayDialogueAndLoadDataPoints();
            this.IsValidPlotLoaded = this.DataPoints.Any();
            OnPropertyChanged(nameof(DataPoints));
            OnPropertyChanged(nameof(IsValidPlotLoaded));

            this.FindFunction();
        }
    }
}