using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using DotNetCorePlotter.Mvvm;
using DotNetCorePlotter.Utils;
using OxyPlot;

namespace DotNetCorePlotter
{
    /// <summary>
    /// ViewModel class for the MainWindow.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constant declaration string of an exponential function.
        /// </summary>
        public const string ExponentialFunction = "y = a * exp (b * x)";

        /// <summary>
        /// Default horizontal resolution for the generated function (above 4096 to be oversampled, even on 4K monitors).
        /// </summary>
        public const double FunctionResolution = 5000;

        /// <summary>
        /// Constant declaration string of a linear function.
        /// </summary>
        public const string LinearFunction = "y = (a * x) + b";

        /// <summary>
        /// Constant declaration string of a power function.
        /// </summary>
        public const string PowerFunction = "y = a * (x ^ b)";

        private double doubleValueA = 0d;
        private double doubleValueB = 0d;
        private FunctionType functionType = FunctionType.Linear;

        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="fileLoader">Injected instance of the fileLoader.</param>
        /// <param name="mathHelper">Injected instance of the mathHelper.</param>
        public MainViewModel(IFileLoader fileLoader, IMathHelper mathHelper)
        {
            this.FileLoader = fileLoader;
            this.MathHelper = mathHelper;
            LoadFileCommand = new DelegateCommand(LoadFileCallback);
            FindFunctionCommand = new DelegateCommand(FindFunction);
            DrawFunctionCommand = new DelegateCommand(DrawFunction);
        }

        /// <summary>
        /// Event handler for the 'property changed' event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the list of loaded data points.
        /// </summary>
        public List<DataPoint> DataPoints { get; private set; } = new List<DataPoint>();

        /// <summary>
        /// Gets the command that draws or redraws the generated function.
        /// </summary>
        public ICommand DrawFunctionCommand { get; }

        /// <summary>
        /// Gets the command that finds a fitting function for the data points.
        /// </summary>
        public ICommand FindFunctionCommand { get; }

        /// <summary>
        /// Gets or sets the function type of the generated function.
        /// </summary>
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
                    this.FindFunction();
                }
            }
        }

        /// <summary>
        /// Gets the list of the generated function's data points.
        /// </summary>
        public List<DataPoint> GeneratedDataPoints { get; private set; } = new List<DataPoint>();

        /// <summary>
        /// Gets a value determining whether valid plot data is loaded.
        /// </summary>
        public bool IsValidPlotLoaded { get; private set; }

        /// <summary>
        /// Gets the command that loads the file with the data points.
        /// </summary>
        public ICommand LoadFileCommand { get; }

        /// <summary>
        /// Gets the resulting generated function to display in the UI.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the A viariable of the generated function.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the B viariable of the generated function.
        /// </summary>
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

            switch (functionType)
            {
                case FunctionType.Linear:
                    for (int i = 0; i < FunctionResolution; i++)
                    {
                        var x = firstX + resolutionStep * i;
                        var y = this.doubleValueA * x + this.doubleValueB;
                        GeneratedDataPoints.Add(new DataPoint(x, y));
                    }
                    break;

                case FunctionType.Exponential:
                    for (int i = 0; i < FunctionResolution; i++)
                    {
                        var x = firstX + resolutionStep * i;
                        var y = this.doubleValueA * Math.Exp(this.doubleValueB * x);
                        GeneratedDataPoints.Add(new DataPoint(x, y));
                    }
                    break;

                case FunctionType.PowerFunction:
                    for (int i = 0; i < FunctionResolution; i++)
                    {
                        var x = firstX + resolutionStep * i;
                        var y = this.doubleValueA * Math.Pow(x, this.doubleValueB);
                        GeneratedDataPoints.Add(new DataPoint(x, y));
                    }
                    break;
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
                    tuple = this.MathHelper.FindExponentialFunction(xData, yData);
                    break;

                case FunctionType.PowerFunction:
                    tuple = this.MathHelper.FindPowerFunction(xData, yData);
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