using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace GBFDesktopTools.View
{   
    using System.Windows.Media.Imaging;

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
