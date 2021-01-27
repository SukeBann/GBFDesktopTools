using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;

namespace GBFDesktopTools.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 主窗口 显示各种信息以及打开要使用的功能
    /// </summary>
    public partial class MainWindow : Panuon.UI.Silver.WindowX
    {
        public Navigationer<MainWindow, pgLoading> NG = null;
        //private Model.DownLoadMessage DM = new Model.DownLoadMessage("Welcome GBFDesktopTools","click these button to use the program");
        
        public MainWindow()
        {
            InitializeComponent();
            this.Icon = Application.Current.Resources["MainIcon"] as BitmapImage;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //this.DownLoadMessage.DataContext = DM.CustomMessage;
            this.Loaded -= MainWindow_Loaded;
        }

        

        /// <summary>
        /// 功能选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRuaRuaRua(object sender, RoutedEventArgs e)
        {
            try
            {
                Button RuaButton = sender as Button;
                string ButtonTag = RuaButton.Tag.ToString();

                switch (ButtonTag)
                {
                    case "WeaponPanelSimulator":
                        var ACT = new Action(() => 
                        {
                            var WPSWin = new WeaponPanelSimulator();
                            WPSWin.Show();
                        });
                        ACT();
                        break;

                    case "LocalWiki":
                        
                        break;

                    case "DamageTest":
                        break;

                    case "GameWalkthrough":
                        break;

                    case "MostUsedLink":
                        break;

                    case "tokenCalculator":
                        break;

                    case "Setting":
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开窗口时遇到错误:" + ex.Message);
            }     
        }
    }
}
