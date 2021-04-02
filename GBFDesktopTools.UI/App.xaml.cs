using System.Windows;

namespace GBFDesktopTools.View
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var Win = new MainWindow();
            Win.Show();
        }
    }
}