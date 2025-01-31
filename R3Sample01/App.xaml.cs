using R3;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace R3Sample01
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            WpfProviderInitializer.SetDefaultObservableSystem(ex => Debug.WriteLine($"R3 UnhandledException:{ex}"));
        }
    }
}
