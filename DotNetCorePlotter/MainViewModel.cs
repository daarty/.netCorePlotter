using System.Windows.Input;
using DotNetCorePlotter.Mvvm;
using DotNetCorePlotter.Utils;
using OxyPlot;

namespace DotNetCorePlotter
{
    public class MainViewModel
    {
        public MainViewModel(IFileLoader fileLoader)
        {
            //  TODO maybe remove
            this.FileLoader = fileLoader;
            LoadFileCommand = new DelegateCommand(() => dataPoints = fileLoader.DisplayDialogueAndLoadDataPoints());
        }

        // TODO set private
        public DataPoint[] dataPoints { get; private set; } = null;

        public ICommand LoadFileCommand { get; }

        //  TODO maybe remove
        private IFileLoader FileLoader { get; }
    }
}