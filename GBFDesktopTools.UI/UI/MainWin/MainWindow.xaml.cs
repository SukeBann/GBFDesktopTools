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
    public partial class MainWindow : Window
    {
        public Navigationer<MainWindow, pgLoading> NG = null;
        private Model.DownLoadMessage DM = new Model.DownLoadMessage("Welcome GBFDesktopTools","click these button to use the program");
        
        public MainWindow()
        {
            InitializeComponent();
            this.Icon = Application.Current.Resources["MainIcon"] as BitmapImage;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DownLoadMessage.DataContext = DM.CustomMessage;
            this.Loaded -= MainWindow_Loaded;
        }

        /// <summary>
        /// 异步加载数据时显示加载窗口，加载完成后打开对应功能的窗体
        /// </summary>
        /// <typeparam name="T">对应窗体体构造参数的类型</typeparam>
        /// <param name="func">封装好返回对应窗体构造参数的方法</param>
        /// <param name="WinType">加载的窗体类型</param>
        /// <param name="LoadingMsg">加载窗口显示的信息</param>
        private void RunTaskMethod<T>(Func<T> func,Type WinType,string LoadingMsg)
        {
            try
            {
                var task = Task.Factory.StartNew<T>(func);
                NG = new Navigationer<MainWindow, pgLoading>(this, new pgLoading("加载" + LoadingMsg + "数据中,请稍等。")) {CanClose = false };
                NG.Title = "Loading...";
                NG.SetWinSize(true, 100, 300);
                NG.IsLockedParentWin = false;
                NG.ShowPg();

                Action act = new Action(() => {
                    try
                    {
                        while (true)
                        {
                            if (task.IsCompleted)
                            {
                                NG.Close();
                                this.Dispatcher.Invoke(new Action(() => { this.IsEnabled = true; }));

                                if (WinType == typeof(WeaponPanelSimulator))
                                {   
                                    var WAER = task.Result as GBFDesktopTools.Library.WeaponAndSkillExcelReader;
                                    if (WAER.HasError)
                                    {
                                        throw new Exception(WAER.ErrorMsg);
                                    }
                                    WeaponPanelSimulator wp = new WeaponPanelSimulator(task.Result as GBFDesktopTools.Library.WeaponAndSkillExcelReader);
                                    wp.ShowDialog();
                                }
                                if (true)
                                {
                                    
                                }
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("打开窗口时发生错误：" + ex.Message);
                    }
                }); 

                this.IsEnabled = false;
                Thread td = new Thread(new ThreadStart(act));
                td.TrySetApartmentState(ApartmentState.STA);
                td.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("异步加载数据时遇到错误："+ex.Message);
            }
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
                        var waer = new GBFDesktopTools.Library.WeaponAndSkillExcelReader();
                        var func = new Func<GBFDesktopTools.Library.WeaponAndSkillExcelReader>(() => waer.LoadWeaponPanelSimulator());
                        RunTaskMethod<GBFDesktopTools.Library.WeaponAndSkillExcelReader>(func, typeof(WeaponPanelSimulator), "武器");
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
