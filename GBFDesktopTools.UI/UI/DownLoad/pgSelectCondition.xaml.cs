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

namespace GBFDesktopTools.View
{
    using GBFDesktopTools.Model;
    using GBFDesktopTools.Model.abstractModel;
    /// <summary>
    /// pgSelectCondition.xaml 的交互逻辑
    /// </summary>
    public partial class pgSelectCondition : Page
    {
        //下载的目标类型 Role,Weapon,SummonGem
        AsyncCollection<SpiderCondition> ConditionTargetTypeList = new AsyncCollection<SpiderCondition>();
        //稀有度类型 R,SR,SSR,LIMIT
        AsyncCollection<SpiderCondition> ConditionRarityList = new AsyncCollection<SpiderCondition>();
        //操作类型 Normal,DownLoadXML
        AsyncCollection<SpiderCondition> ConditionExcTypeList = new AsyncCollection<SpiderCondition>();
        //父窗口 用于赋值操作类型
        GBFSpider ParentWindow = null;

        public pgSelectCondition(GBFSpider _ParentWindow)
        {
            InitializeComponent();
            (this.Resources["CvsTargetTypeL"] as CollectionViewSource).Source = ConditionTargetTypeList;
            (this.Resources["CvsRarityL"] as CollectionViewSource).Source = ConditionRarityList;
            (this.Resources["CvsExcTypeL"] as CollectionViewSource).Source = ConditionExcTypeList;

            ParentWindow = _ParentWindow;
            ParentWindow.DownList.Clear(); 
            ParentWindow.IsSelectCondition = false;
            this.Loaded += new RoutedEventHandler(pgSelectCondition_Loaded);
        }

        #region Method

        public void LoadConditionList()
        {
            SpiderCondition GetEnumList = new SpiderCondition();

            ConditionTargetTypeList.Clear();
            ConditionRarityList.Clear();
            ConditionExcTypeList.Clear();

            var CTT = GetEnumList.GetList<SpiderCondition.SpiderConditionTargetType>();
            ConditionTargetTypeList.AddRange(CTT);
            var CR = GetEnumList.GetList<SpiderCondition.SpiderConditionRarity>();
            ConditionRarityList.AddRange(CR);
            var CET = GetEnumList.GetList<SpiderCondition.SpiderConditionExcType>();
            ConditionExcTypeList.AddRange(CET);

            GetEnumList = null;
        }

        #endregion

        #region Event

        void pgSelectCondition_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConditionList();
            this.Loaded -= pgSelectCondition_Loaded;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button ButtonClick = sender as Button;
            string Tag = ButtonClick.Tag.ToString();
            switch (Tag)
            {
                case "Continue":
                    var TargetTypeList = ConditionTargetTypeList.Where(x => x.IsChecked == true).Select(x => x.ConditionTargetType).ToList();
                    var RarityList = ConditionRarityList.Where(x => x.IsChecked == true).Select(x => x.ConditionRarity).ToList();
                    var ExcList = ConditionExcTypeList.Where(x => x.IsChecked == true && x.ConditionExcType != SpiderCondition.SpiderConditionExcType.AllMessage).Select(x => x.ConditionExcType).ToList();
                    
                    //判断是否选择了条件
                    if (TargetTypeList.Count == 0 || RarityList.Count == 0 || ExcList.Count == 0)
                    {
                        this.ParentWindow.IsSelectCondition = false;
                        this.ParentWindow.NG.NavigaionWin.Close();
                        return;
                    }

                    //根据选择的条件生成下载列表
                    List<SpiderCondition> ConditionList = new List<SpiderCondition>();
                    this.ParentWindow.DownList = ConditionList;
                    foreach (var Tar in TargetTypeList)
                    {                        
                        foreach (var Exc in ExcList)
                        {
                            foreach (var Rar in RarityList)
                            {
                                SpiderCondition sc = new SpiderCondition();
                                sc.ConditionTargetType = Tar;
                                sc.ConditionExcType = Exc;
                                sc.ConditionRarity = Rar;
                                //sc.SetAddress();
                                ConditionList.Add(sc);
                            }   
                        } 
                    }

                    this.ParentWindow.IsSelectCondition = true;
                    this.ParentWindow.NG.NavigaionWin.Close();
                    return;

                case "Back":
                    this.ParentWindow.IsSelectCondition = false;
                    this.ParentWindow.NG.NavigaionWin.Close();
                    return;
            }
        }

        private void SpiderConditionExcType_Checked(object sender, RoutedEventArgs e)
        {
            if (ConditionExcTypeList.Where(x => x.ConditionExcType.ToString() == "AllMessage").ToList()[0].IsChecked == true)
            {
                foreach (var item in ConditionExcTypeList)
                {   
                    if (item.ConditionExcType.ToString() != "AllMessage")
                    {
                        item.IsChecked = true;
                        item.IsEnabled = false;
                        if (item.ConditionExcType.ToString() == "LoadFromLocal")
                        {
                            item.IsChecked = false;
                        }
                    }  
                }
                foreach (var item in ConditionRarityList)
                {
                    item.IsChecked = true;
                    item.IsEnabled = false;
                }
                foreach (var item in ConditionTargetTypeList)
                {
                    item.IsChecked = true;
                    item.IsEnabled = false;
                }
            }
            if (ConditionExcTypeList.Where(x => x.ConditionExcType.ToString() == "AllMessage").ToList()[0].IsChecked == false && (sender as CheckBox).Content.ToString() == "AllMessage")
            {
                foreach (var item in ConditionExcTypeList)
                {
                    item.IsChecked = false;
                    item.IsEnabled = true;
                }
                foreach (var item in ConditionRarityList)
                {
                    item.IsChecked = false;
                    item.IsEnabled = true;
                }
                foreach (var item in ConditionTargetTypeList)
                {
                    item.IsChecked = false;
                    item.IsEnabled = true;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 条件选择器实体类
    /// </summary>
    public class SpiderCondition : abstractModel
    {
        #region SpiderConditionEnum

        public enum SpiderConditionTargetType
        {
            Character = 1,
            Weapon = 2,
            Summons = 3
        }

        public enum SpiderConditionRarity
        {   
            N = 1,
            R = 2,
            SR = 3,
            SSR = 4
        }

        public enum SpiderConditionExcType
        {
            AllMessage = 1,
            DownLoadXML = 2,
            DownLoadImage = 3,
            LoadFromLocal = 4
        }
        
        #endregion

        #region Constructor

        public SpiderCondition()
        {
            IsChecked = false;
            IsEnabled = true;
        }

        #endregion

        #region Property

        private string _Address;
        private string _DownLoadAddress;

        private SpiderConditionTargetType _ConditionTargetType;
        private SpiderConditionRarity _ConditionRarity;
        private SpiderConditionExcType _ConditionExcType;

        //绑定用的枚举类型列表(操作类型)
        public SpiderConditionExcType ConditionExcType
        {
            get { return _ConditionExcType; }
            set { _ConditionExcType = value; PropertyChangedBaseEx.RaisePropertyChanged(this, x => x._ConditionExcType); }
        }
        //绑定用的枚举类型列表(稀有度)
        public SpiderConditionRarity ConditionRarity
        {
            get { return _ConditionRarity; }
            set { _ConditionRarity = value; PropertyChangedBaseEx.RaisePropertyChanged(this, x => x._ConditionRarity); }
        }
        //绑定用的枚举类型列表(目标类型)
        public SpiderConditionTargetType ConditionTargetType
        {
            get { return _ConditionTargetType; }
            set { _ConditionTargetType = value; PropertyChangedBaseEx.RaisePropertyChanged(this, x => x._ConditionTargetType); }
        }
        
        //主程序路径
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        //下载路径
        public string DownLoadAddress
        {
            get { return _DownLoadAddress; }
            set { _DownLoadAddress = value; }
        }
        
        #endregion

        #region Indexer

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "SpiderConditionTargetType":
                        return ConditionTargetType as object;
                    case "SpiderConditionRarity":
                        return ConditionRarity as object;
                    case "SpiderConditionExcType":
                        return ConditionExcType as object;
                    default:
                        throw new ArgumentOutOfRangeException("index");//抛出异常
                }
            }

            set
            {
                switch (index)
                {
                    case "SpiderConditionTargetType":
                        ConditionTargetType = (SpiderConditionTargetType)value;
                        break;
                    case "SpiderConditionRarity":
                        ConditionRarity = (SpiderConditionRarity)value;
                        break;
                    case "SpiderConditionExcType":
                        ConditionExcType = (SpiderConditionExcType)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("index");//抛出异常
                }
            }
        }


        #endregion

        #region Method

        /// <summary>
        /// 获取枚举列表List (string)
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>包含枚举列表的条件选择器实体类</returns>
        public List<SpiderCondition> GetList<T>()
        {
            List<SpiderCondition> TargetList = new List<SpiderCondition>();
            foreach (var item in (T[])System.Enum.GetValues(typeof(T)))
            {
                SpiderCondition SC = new SpiderCondition();
                SC[item.GetType().Name] = item;
                TargetList.Add(SC);
            }
            return TargetList;
        }
        
        /// <summary>
        /// 设置下载地址
        /// </summary>
        //public void SetAddress()
        //{
        //    //Location = "E:\\WeaponPanelSimulator\\WeaponPanelSimulator\\bin\\Debug\\WeaponPanelSimulator.exe"
        //    var asm = System.Reflection.Assembly.GetExecutingAssembly();
        //    string resoureePath = asm.Location;

        //    resoureePath = resoureePath.Replace("WeaponPanelSimulator.exe", "Resources\\");
        //    //程序资源目录 + 操作类型 + 稀有度 + 目标类型
        //    DownLoadAddress = resoureePath + (ConditionExcType == SpiderConditionExcType.DownLoadImage ? "Image\\" : "XML\\");
        //}

        #endregion
    }

}
