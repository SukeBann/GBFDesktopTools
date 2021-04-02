using System.Windows;
using System.Windows.Controls;

namespace GBFDesktopTools.View
{
    /// <summary>
    /// pgLoading.xaml 的交互逻辑
    /// </summary>
    public partial class pgLoading : Page
    {
        public pgLoading(string LoadingMsg)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(pgLoading_Loaded);
            this.tbLoadingTxt.Text = LoadingMsg;
        }

        private void pgLoading_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= pgLoading_Loaded;
            this.PictureOfGif.Image = System.Drawing.Image.FromFile(@"E:\WeaponPanelSimulator\WeaponPanelSimulator\Resources\Gif\Loading2.gif");
        }
    }
}