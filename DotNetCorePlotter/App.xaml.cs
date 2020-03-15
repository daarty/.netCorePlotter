using System.Windows;
using DotNetCorePlotter.Utils;

namespace DotNetCorePlotter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var fileLoader = new FileLoader();
            var mainViewModel = new MainViewModel(fileLoader);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}