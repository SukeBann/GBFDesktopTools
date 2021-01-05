using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GBFDesktopTools.View
{
    using GBFDesktopTools.Library;
    using GBFDesktopTools.Model;
    using System.Text.RegularExpressions;
    /// <summary>
    /// WeaponPanelSimulator.xaml 的交互逻辑
    /// </summary>
    public partial class WeaponPanelSimulator : Panuon.UI.Silver.WindowX
    {
        WeaponAndSkillExcelReader WeaponAndSkill = new WeaponAndSkillExcelReader();
        AsyncCollection<Weapon> WeaponList = new AsyncCollection<Weapon>();
        AsyncCollection<string> WeaponSearchTip = new AsyncCollection<string>();

        GBFDesktopTools.Model.ToolAndHelper.FilterCondition Fc = null;
        
        public WeaponPanelSimulator(WeaponAndSkillExcelReader WAS)
        {
            InitializeComponent();
            WeaponAndSkill = WAS;

            Fc = this.Resources["FcModel"] as GBFDesktopTools.Model.ToolAndHelper.FilterCondition;
            Icon = App.Current.Resources["MainIcon"] as BitmapImage;
            (this.Resources["cvsWeaponList"] as CollectionViewSource).Source = WeaponList;
            (this.Resources["cvsWeaponSearchTip"] as CollectionViewSource).Source = WeaponSearchTip;

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.Loaded += new RoutedEventHandler(WeaponPanelSimulator_Loaded);
        }

        void WeaponPanelSimulator_Loaded(object sender, RoutedEventArgs e)
        {
            Fc.GetSearchTipList(WeaponAndSkill.WeaponList.ObjList);
            this.Loaded -= WeaponPanelSimulator_Loaded;
        }

        private void SearchRun(object sender, RoutedEventArgs e)
        {
            var Bt = sender as Button;
            string Tags = Bt.Tag.ToString();
            int pageIndex = Fc.NowPage;
            var Obj = new Model.abstractModel.ObjectResult<Weapon>();
            WeaponList.Clear();

            switch (Tags)
            {
                case "Search":
                case "First":
                    Fc.NowPage = 1;
                    Obj = Fc.FilterRun();
                    break;
                case "Previous":
                    Fc.NowPage = Fc.NowPage - 1 < 0 ? 0 : Fc.NowPage - 1;
                    Obj = Fc.FilterRun();
                    break;
                case "Next":
                    Fc.NowPage = Fc.NowPage + 1 > Fc.PageCount ? Fc.PageCount : Fc.NowPage + 1;
                    Obj = Fc.FilterRun();
                    break;
                case "Last":
                    if (Fc.PageCount == 0)
                    {
                        MessageBox.Show("当前列表没有数据");
                        return;
                    }
                    Fc.NowPage = Fc.PageCount;
                    Obj = Fc.FilterRun();
                    break;
                case "Goto":
                    Fc.NowPage = Convert.ToInt32(this.tbGotoValue.Text == "" ? "0" : this.tbGotoValue.Text);
                    Obj = Fc.FilterRun();
                    break;
            }
            if (Obj.hasError)
            {
                MessageBox.Show("加载列表出错："+Obj.ErrorMsg);
                return;
            }
            WeaponList.AddRange(Obj.ObjList);
        }

        //显示搜索提示
        private void wtSearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            WeaponSearchTip.Clear();
            var waterText = sender as Xceed.Wpf.Toolkit.WatermarkTextBox;
            var Text = waterText.Text;
            if (Text == string.Empty || Text == null)
            {
                return;
            }
            List<string> TempList = new List<string>();

            //匹配含有目标值的字符串
            TempList = Fc.SearchTip.Where(x => Regex.IsMatch(x, Text,RegexOptions.IgnoreCase)).ToList();
            
            //按照目标值在字符串中的索引排序
            TempList.Sort(new GBFDesktopTools.Model.ToolAndHelper.FilterCondition.SearchTipListReverserClass(Text));
            //反转List
            TempList.Reverse();

            //如果有十个以上的值则取前十个
            if (TempList.Count >= 10)
            {
                TempList = TempList.GetRange(0, 10);
            }
            //否则取10一下的最大值
            else
            {
                TempList = TempList.GetRange(0, TempList.Count);
            }

            WeaponSearchTip.AddRange(TempList);
            puInputTip.IsOpen = true;
        }
        //选中搜索提示后填入WaterMarkTextBox
        private void SelectTipChange(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            var lbItem = lb.SelectedItem as string;
            if (lbItem == null || lbItem == string.Empty)
            {
                return;
            }

            wtSearchInput.TextChanged -= wtSearchInput_TextChanged;
            wtSearchInput.Text = lbItem;
            puInputTip.IsOpen = false;
            wtSearchInput.TextChanged += wtSearchInput_TextChanged;
        }



    }
}
