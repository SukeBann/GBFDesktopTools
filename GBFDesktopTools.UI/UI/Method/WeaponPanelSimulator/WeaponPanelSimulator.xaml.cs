using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using GBFDesktopTools.Model.ToolAndHelper;
using Panuon.UI.Silver.Core;
// ReSharper disable DelegateSubtraction

namespace GBFDesktopTools.View
{
    using Model;
    using Library;
    using Panuon.UI.Silver;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Text.RegularExpressions;

    /// <summary>
    /// WeaponPanelSimulator.xaml 的交互逻辑
    /// </summary>
    public partial class WeaponPanelSimulator : WindowX
    {
        private readonly WeaponAndSkillExcelReader WeaponAndSkill = new WeaponAndSkillExcelReader();
        private readonly AsyncCollection<string> WeaponSkillNameList = new AsyncCollection<string>();
        private readonly AsyncCollection<WeaponSkill> WeaponSkillList = new AsyncCollection<WeaponSkill>();
        private readonly AsyncCollection<Weapon> WeaponList = new AsyncCollection<Weapon>();
        private readonly AsyncCollection<string> WeaponSearchTip = new AsyncCollection<string>();

        private readonly FilterCondition Fc;
        private readonly CalculatorCore CC;

        public WeaponPanelSimulator()
        {
            InitializeComponent();

            Fc = this.Resources["FcModel"] as FilterCondition;
            CC = this.Resources["CCModel"] as CalculatorCore;

            ((CollectionViewSource) this.Resources["cvsWeaponList"]).Source = WeaponList;
            ((CollectionViewSource) this.Resources["cvsWeaponSearchTip"]).Source = WeaponSearchTip;
            
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Loaded += WeaponPanelSimulator_Loaded;
        }

        private void WeaponPanelSimulator_Loaded(object sender, RoutedEventArgs e)
        {
            RunTaskMethod();
            // ReSharper disable once PossibleNullReferenceException
            
            //筛选器下拉菜单数据
            cbSortTypeList.ItemsSource = Fc.SearchSortTypeList;
            cbWeaponKindList.ItemsSource = Fc.WeaponKindList;
            cbWeaponSeriesNameList.ItemsSource = Fc.WeaponSeriesNameList;
            cbWeaponEvoCountList.ItemsSource = Fc.WeaponSearchEvoCountList;
            cbWeaponElementList.ItemsSource = Fc.WeaponElementList;
            cbWeaponCategoryList.ItemsSource = Fc.WeaponCategoryList;
            cbWeaponRarityList.ItemsSource = Fc.WeaponRarityList;

            tempC.ItemsSource = Fc.WeaponElementList;
            tempA.ItemsSource = WeaponSkillNameList;
            cbWeaponSkillNameList.ItemsSource = WeaponSkillNameList;
            cbWeaponSkillNameList.SelectedIndex = 0;

            Fc.loadWeaponListHandler += SearchRunEvent;
            this.Loaded -= WeaponPanelSimulator_Loaded;
        }

        /// <summary>
        /// 异步载入窗口数据时显示加载窗口
        /// </summary>
        private void RunTaskMethod()
        {
            try
            {
                var PendingBox = PendingBoxX.Show("正在加载武器数据，请稍等。", "Loading~",this);
                var task = Task.Factory.StartNew(() => WeaponAndSkill.LoadWeaponPanelSimulator());
                var act = new Action(() =>
                {
                    try
                    {
                        while (true)
                        {
                            if (!task.IsCompleted) continue;
                            
                            var excelReader = task.Result;
                            if (excelReader.HasError)
                            {
                                throw new Exception(excelReader.ErrorMsg);
                            }
                            Fc.GetSearchTipList(excelReader.WeaponList.ObjList);
                            WeaponSkillList.AddRange(excelReader.SkillList.ObjList);
                            WeaponSkillNameList.AddRange(excelReader.SkillList.ObjList.GroupBy(x => x.Extra_Description + x.Main_Description).Select(x => x.Key).ToList());
                            PendingBox.Close();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxX.Show("子线程错误：" + ex.Message, "ErrorMessage", MessageBoxButton.OK, MessageBoxIcon.Error);
                        Application.Current.Dispatcher.Invoke(new Action(this.Close));
                    }
                });
                var thread = new Thread(new ThreadStart(act));
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBoxX.Show("打开窗口时发生错误：" + ex.Message, "ErrorMessage", MessageBoxButton.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        #region 放置源

        private void FilterRunMethod()
        {
            this.IsMaskVisible = false;
            var Obj = Fc.FilterRun();
            WeaponList.Clear();
            if (Obj.hasError || Obj.ObjList.Count == 0)
            {
                NoticeX.Show("加载列表出错：" + (string.IsNullOrEmpty(Obj.ErrorMsg) ? "没有查询到数据" : Obj.ErrorMsg), MessageBoxIcon.Error, 900);
                return;
            }
            WeaponList.AddRange(Obj.ObjList);

            this.IsMaskVisible = false;
        }

        private void SearchRunEvent(object sender, EventArgs e)
        {
            FilterRunMethod();
        }

        private void SearchRunClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }
            Fc.loadWeaponListHandler -= SearchRunEvent;
            var tags = button.Tag;
            switch (tags)
            {
                case "Search":
                    Fc.NowPage = 1;
                    break;
                case "Goto":
                    var tempInt = string.IsNullOrEmpty(tbGotoValue.Text) ? 1 : Convert.ToInt32(tbGotoValue.Text);
                    if (tempInt > Fc.PageCount || tempInt < 1)
                    {
                        NoticeX.Show("页码超出范围", "error", MessageBoxIcon.Error, 900);
                        return;
                    }
                    Fc.NowPage = tempInt;
                    break;
            }
            FilterRunMethod();
            Fc.loadWeaponListHandler += SearchRunEvent;
        }

        //显示搜索提示
        private void WtSearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            WeaponSearchTip.Clear();
            if (!(sender is TextBox txBox)) return;

            var Text = txBox.Text.Replace(@"\", "");
            if (Text.IsNullOrEmpty()) return;

            //匹配含有目标值的字符串
            var TempList = Fc.SearchTip.Where(x => Regex.IsMatch(x, Text, RegexOptions.IgnoreCase)).ToList();
            if (TempList.Count == 0) return;

            //按照目标值在字符串中的索引排序
            TempList.Sort(new Model.ToolAndHelper.FilterCondition.SearchTipListReverserClass(Text));

            //反转List
            TempList.Reverse();

            //如果有十个以上的值则取前十个
            TempList = TempList.GetRange(0, TempList.Count >= 10 ? 10 : TempList.Count);

            WeaponSearchTip.AddRange(TempList);
            puInputTip.IsOpen = true;
        }

        //搜索输入框支持回车以及Key.down切换至搜索提示
        private void SearchKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (puInputTip.IsOpen)
                {
                    var text = TipListbox.SelectedItem?.ToString();
                    if (!string.IsNullOrEmpty(text))
                    {
                        tbSearchInput.Text = text;
                    }
                }

                Fc.loadWeaponListHandler -= SearchRunEvent;
                FilterRunMethod();
                Fc.loadWeaponListHandler += SearchRunEvent;
                puInputTip.IsOpen = false;
                return;
            }

            if (e.Key != Key.Down)
            {
                if (e.Key != Key.Up)
                {
                    return;
                }
            }

            if (TipListbox.SelectedItem != null) return;
            TipListbox.Focus();
            if (TipListbox.SelectedItem == null)
            {
                TipListbox.SelectedIndex = 0;
            }
        }

        #endregion

        #region 实现拖放


        private DependencyObject TempControl = null;

        private void DragDidBegin(object sender, MouseButtonEventArgs e)
        {
            switch (sender)
            {
                case ListBox lb:
                {
                    var pos = e.GetPosition(lb);
                    var result = VisualTreeHelper.HitTest(lb, pos);
                    if (result == null)
                    {
                        return;
                    }
                    CC.TempWeapon = Utils.FindVisualParent<ListBoxItem>(result.VisualHit).Content as Weapon;
                    break;
                }
                case Border bd:
                {
                    var pos = e.GetPosition(bd);
                    var result = VisualTreeHelper.HitTest(bd, pos);
                    if (result == null)
                    {
                        return;
                    }
                    CC.TempWeapon = Utils.FindVisualParent<Border>(result.VisualHit).DataContext as Weapon;
                    if (!CC.CopyOrMove)
                    {
                        TempControl = bd;
                    }
                    break;
                }
                default:
                    CC.TempWeapon = null;
                    break;
            }

            if (CC.TempWeapon == null) {return;}
            var dataObject = new DataObject(CC.TempWeapon);
            DragDrop.DoDragDrop((DependencyObject) sender, dataObject, DragDropEffects.Copy);
        }

        private void WeaponDrop(object sender, DragEventArgs e)
        {
            if (TempControl == null)
            {
               var Grid = Utils.FindVisualParent<Grid>(sender as Border);
               Grid.DataContext = (e.Data.GetData(typeof(Weapon)) as Weapon).CopySelf();
            }
            else
            {
                var Grid = Utils.FindVisualParent<Grid>(sender as Border);
                var SourceGrid = Utils.FindVisualParent<Grid>(TempControl);
                SourceGrid.DataContext = Grid.DataContext;
                Grid.DataContext = e.Data.GetData(typeof(Weapon)) as Weapon;
            }

            TempControl = null;
        }

        #endregion

    }

    /// <summary>
    /// 拖拽事件辅助类
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// 根据子元素查找父元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindVisualParent<T>(DependencyObject obj) where T : class
        {
            while (obj != null)
            {
                if (obj is T obj1)
                {
                    return obj1;
                }
                    
                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }
    }
}
