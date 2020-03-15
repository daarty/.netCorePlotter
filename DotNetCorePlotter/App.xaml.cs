using System.Windows;
using DotNetCorePlotter.Utils;

namespace DotNetCorePlotter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Creates a new instance of <see cref="App"/>.
        /// </summary>
        public App()
        {
            // Prepare dependency inhection for the MainWindow and its MainViewModel.
            var fileLoader = new FileLoader();
            var mathHelper = new MathHelper();

            var mainViewModel = new MainViewModel(fileLoader, mathHelper);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}