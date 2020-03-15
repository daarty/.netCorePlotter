using System.Globalization;
using System.Threading;
using System.Windows;
using DotNetCorePlotter.Utils;

namespace DotNetCorePlotter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly CultureInfo CultureInfo = new CultureInfo("en-US");

        /// <summary>
        /// Creates a new instance of <see cref="App"/>.
        /// </summary>
        public App()
        {
            // Sets the en-US culture to avoid number puntucation errors with 1.000,1 or 1,000.1.
            this.SetCulture();

            // Prepare dependency inhection for the MainWindow and its MainViewModel.
            var fileLoader = new FileLoader();
            var mathHelper = new MathHelper();

            var mainViewModel = new MainViewModel(fileLoader, mathHelper);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }

        private void SetCulture()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo;
            Thread.CurrentThread.CurrentCulture = CultureInfo;
            Thread.CurrentThread.CurrentUICulture = CultureInfo;
        }
    }
}