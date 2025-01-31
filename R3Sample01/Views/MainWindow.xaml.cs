using System.Windows;

namespace R3Sample01.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Unloaded += (o, e) =>
            {
                if (this.DataContext is IDisposable vm)
                {
                    vm.Dispose();
                }
            };
        }
    }
}
