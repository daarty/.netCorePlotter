using System.Windows;
using System.Windows.Input;

namespace DotNetCorePlotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="viewModel">Injected instance of the viewmodel.</param>
        public MainWindow(MainViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        /// <summary>
        /// Quick hack to allow using Enter to lose the focus from the TextBoxes.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void VariableTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                Keyboard.ClearFocus();
                this.DrawButton.Focus();
            }
        }
    }
}