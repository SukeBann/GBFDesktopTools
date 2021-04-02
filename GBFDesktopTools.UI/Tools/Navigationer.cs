using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace GBFDesktopTools.View
{
    public class Navigationer<P, C> where P : class where C : class
    {
        private System.ComponentModel.CancelEventHandler CancelHandler = null;
        private System.ComponentModel.CancelEventArgs CancelArgs = new System.ComponentModel.CancelEventArgs();

        //导航器实体
        public NavigationWindow NavigaionWin = null;

        #region Constructor

        /// <summary>
        /// 构造一个普通UI导航器
        /// </summary>
        /// <param name="_PWin"></param>
        /// <param name="_CWin"></param>
        public Navigationer(P _PWin, C _CWin)
        {
            //要打开的窗口的父窗口
            ParentWindow = _PWin;
            //要打卡的窗口
            ChildWindow = _CWin;
            //导航窗口
            NavigaionWin = new NavigationWindow();

            #region 默认设置

            //设置用户是否能关闭窗口 默认为不能
            CanClose = true;
            SetCancelEventHandler();
            NavigaionWin.Closing += CancelHandler;
            //锁定父窗口
            IsLockedParentWin = true;
            //不显示导航栏
            ShowsNavigationUI = false;
            //窗口默认打开位置为屏幕中央
            StartupLocation = WindowStartupLocation.CenterScreen;
            //窗口禁止缩放
            WinResizeMode = ResizeMode.NoResize;

            Icon = new BitmapImage(new Uri(@"Resources\Icon\SpiderProgramA.ico", UriKind.Relative));

            #endregion 默认设置
        }

        /// <summary>
        /// 重写onClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="CancelArgs">为可取消的事件提供数据</param>
        public void CreateCancelEventHandler(object sender, System.ComponentModel.CancelEventArgs CancelArgs)
        {
            CancelArgs.Cancel = !CanClose;
        }

        /// <summary>
        /// 赋值CancelEventHandler
        /// </summary>
        private void SetCancelEventHandler()
        {
            CancelHandler = new System.ComponentModel.CancelEventHandler(CreateCancelEventHandler);
        }

        #endregion Constructor

        #region Window

        private P _ParentWindow;
        private C _ChildWindow;

        //子窗口
        public C ChildWindow
        {
            get { return _ChildWindow; }
            set { _ChildWindow = value; }
        }

        //父窗口
        public P ParentWindow
        {
            get { return _ParentWindow; }
            set { _ParentWindow = value; }
        }

        #endregion Window

        #region WidthAndHeight

        private double _Height;
        private double _MaxHeight;
        private double _MinHeight;
        private double _Width;
        private double _MaxWidth;
        private double _MinWidth;

        public double MinWidth
        {
            get { return _MinWidth; }
            set
            {
                NavigaionWin.MinWidth = value;
                _MinWidth = value;
            }
        }

        public double MaxWidth
        {
            get { return _MaxWidth; }
            set
            {
                NavigaionWin.MaxWidth = value;
                _MaxWidth = value;
            }
        }

        public double Width
        {
            get { return _Width; }
            set
            {
                NavigaionWin.Width = value;
                _Width = value;
            }
        }

        public double MinHeight
        {
            get { return _MinHeight; }
            set
            {
                NavigaionWin.MinHeight = value;
                _MinHeight = value;
            }
        }

        public double MaxHeight
        {
            get { return _MaxHeight; }
            set
            {
                NavigaionWin.MaxHeight = value;
                _MaxHeight = value;
            }
        }

        public double Height
        {
            get { return _Height; }
            set
            {
                NavigaionWin.Height = value;
                _Height = value;
            }
        }

        #endregion WidthAndHeight

        #region Title,Icon And Settings

        private string _Title;
        private ImageSource _Icon;
        private System.Windows.ResizeMode _WinResizeMode;
        private System.Windows.WindowStartupLocation _StartupLocation;
        private bool _ShowsNavigationUI;
        private bool _IsLockedParentWin;
        private bool _CanClose;

        /// <summary>
        /// 用户是否能关闭窗口 true 为 是
        /// </summary>
        public bool CanClose
        {
            get { return _CanClose; }
            set { _CanClose = value; }
        }

        /// <summary>
        /// 是否锁定父窗口
        /// </summary>
        public bool IsLockedParentWin
        {
            get { return _IsLockedParentWin; }
            set { _IsLockedParentWin = value; }
        }

        /// <summary>
        /// 是否显示导航栏
        /// </summary>
        public bool ShowsNavigationUI
        {
            get { return _ShowsNavigationUI; }
            set
            {
                NavigaionWin.ShowsNavigationUI = value;
                _ShowsNavigationUI = value;
            }
        }

        /// <summary>
        /// 窗口打开的位置
        /// </summary>
        public System.Windows.WindowStartupLocation StartupLocation
        {
            get { return _StartupLocation; }
            set
            {
                NavigaionWin.WindowStartupLocation = value;
                _StartupLocation = value;
            }
        }

        /// <summary>
        /// 窗口缩放模式
        /// </summary>
        public System.Windows.ResizeMode WinResizeMode
        {
            get { return _WinResizeMode; }
            set
            {
                NavigaionWin.ResizeMode = value;
                _WinResizeMode = value;
            }
        }

        /// <summary>
        /// 窗口图标
        /// </summary>
        public ImageSource Icon
        {
            get { return _Icon; }
            set
            {
                NavigaionWin.Icon = value;
                _Icon = value;
            }
        }

        /// <summary>
        /// 窗口标题
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set
            {
                NavigaionWin.Title = value;
                _Title = value;
            }
        }

        #endregion Title,Icon And Settings

        #region Method

        /// <summary>
        /// 打开页面
        /// </summary>
        public void ShowPg()
        {
            NavigaionWin.Content = ChildWindow;
            if (IsLockedParentWin == true)
            {
                NavigaionWin.ShowDialog();
            }
            else
            {
                NavigaionWin.Show();
            }
        }

        /// <summary>
        /// 设置窗口大小
        /// </summary>
        /// <param name="IsNeedParameter">是否需要高宽参数，不需要的话从父窗口取高宽乘以缩放率</param>
        /// <param name="_Height">高度,可选参数，默认400</param>
        /// <param name="_Width">宽度，可选参数，默认400</param>
        /// <param name="HZoomRate">高度缩放率，默认0.75</param>
        /// <param name="WZoomRate">宽度缩放率，默认0.75</param>
        public void SetWinSize(bool IsNeedParameter, double _Height = 400.0, double _Width = 400.0, double HZoomRate = 1.5, double WZoomRate = 0.75)
        {
            if (IsNeedParameter == true)
            {
                if (WinResizeMode == ResizeMode.NoResize)
                {
                    Height = _Height;
                    Width = _Width;
                    MaxHeight = _Height;
                    MaxWidth = _Width;
                }
                if (WinResizeMode == ResizeMode.CanMinimize)
                {
                    Height = _Height;
                    Width = _Width;
                    MinHeight = _Height;
                    MinWidth = _Width;
                }
                if (WinResizeMode == ResizeMode.CanResize || WinResizeMode == ResizeMode.CanResizeWithGrip)
                {
                    Height = _Height;
                    Width = _Width;
                }
            }
            else
            {
                double PHeight = (ParentWindow as Window).Height;
                double PWidth = (ParentWindow as Window).Width;

                if (WinResizeMode == ResizeMode.NoResize)
                {
                    Height = PHeight * HZoomRate;
                    Width = PWidth * WZoomRate;
                    MaxHeight = PHeight * HZoomRate;
                    MaxWidth = PWidth * WZoomRate;
                }
                if (WinResizeMode == ResizeMode.CanMinimize)
                {
                    Height = PHeight * HZoomRate;
                    Width = PWidth * WZoomRate;
                    MinHeight = PHeight * HZoomRate;
                    MinWidth = PWidth * WZoomRate;
                }
                if (WinResizeMode == ResizeMode.CanResize || WinResizeMode == ResizeMode.CanResizeWithGrip)
                {
                    Height = PHeight * HZoomRate;
                    Width = PWidth * WZoomRate;
                }
            }
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        public void Close()
        {
            CanClose = true;
            NavigaionWin.Dispatcher.Invoke(new Action(() => { NavigaionWin.Close(); }));
        }

        #endregion Method
    }
}