using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace GBFDesktopTools.Model.ToolAndHelper
{   
    using Model.abstractModel;

    /// <summary>
    /// 编程工具
    /// </summary>
    public static class ToolsAndHelper
    {   
        /// <summary>
        /// 获取特殊技能列表
        /// </summary>
        /// 维护Resource/Txt文件夹下的GetSpecialSkillList.txt即可
        /// <returns></returns>
        public static List<string> GetSpecialSkillList()
        {
            var asm = System.Reflection.Assembly.GetCallingAssembly();
            string resoureeName = asm.GetName().Name + ".Resources.Txt.SpecialSkillList.txt";
          
            string TempStr = null;

            using (Stream stream = asm.GetManifestResourceStream(resoureeName))
            {
                StreamReader sr = new StreamReader(stream);
                TempStr = sr.ReadToEnd();
                sr.Close();
            }
            var list = SplitString(TempStr.Trim(), 2, IsHaveEmpty: StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < list.Count; i++ )
            {
                list[i] = list[i].Replace("\n", "").Replace("\r", "");
            }
            return list;
        }

        /// <summary>
        /// 按预设的符号分割字符串 -->  ; ；
        /// </summary>
        /// <param name="Target">目标字符串</param>
        /// <param name="splitType">预设分割符号类型 1(；;) 2:(,) 3:(_) </param>
        /// <param name="CustomStr">自定义单分隔符 如果同时使用CustomArray参数 以CustomStr为优先</param>
        /// <param name="CustomArray">自定义分隔符列表 如果同时使用CustomStr参数 以CustomStr为优先</param>
        /// <param name="IsHaveEmpty">返回的数组是否包含空字符串</param>
        /// <returns></returns>
        public static List<string> SplitString(string Target, short splitType = 1,string CustomStr = null,string[] CustomArray = null,StringSplitOptions IsHaveEmpty = StringSplitOptions.None)
        {
            string[] Delimiter = null;
            //自定义单个分隔符
            if (CustomStr != null)
            {
                Delimiter = new string[] { CustomStr };
            }
            //自定义分隔符数组
            if (CustomArray != null && CustomStr == null)
            {
                Delimiter = CustomArray;
            }
            //预设分隔符
            if (CustomStr == null && CustomArray == null)
            {   
                switch (splitType)
                {
                    case 1: Delimiter = new string[2] { ";", "；" };
                        break;
                    case 2: Delimiter = new string[1] { "," };
                        break;
                    case 3: Delimiter = new string[1] { "_" };
                        break;
                }
            }
            return Target.Split(Delimiter, IsHaveEmpty).ToList(); ;
        }

        /// <summary>
        /// 获取技能副名称
        /// </summary>
        /// <param name="MainName">技能主名称</param>
        /// <param name="ExtraName">技能副名称</param>
        /// <param name="Dic">技能字典集</param>
        /// <returns>如果找到副名称则返回 主名称+副名称 否则返回空字符串</returns>
        public static string FoundSkillExtraName(string MainName, string ExtraName, Dictionary<string, List<WeaponSkill>> Dic)
        {
            var DicKey = Dic.Keys.Where(x => StringContentType(x, CustomReg: MainName));
            if (DicKey != null)
            {
                ExtraName = DicKey.FirstOrDefault(f => f.Replace(MainName + "_","") == ExtraName);
            }
            else
            {
                ExtraName = null;
            }
            return (ExtraName == null || ExtraName == "") ? "" : ExtraName;
        }

        /// <summary>
        /// 验证技能后缀是否存在
        /// </summary>
        /// <param name="MainName">技能名称</param>
        /// <param name="Suffix">技能后缀</param>
        /// <param name="Dic">技能字典集</param>
        /// <returns>如果不存在则返回空字符串，存在则返回技能后缀</returns>
        public static string FoundSkillSuffix(string MainName, string Suffix, Dictionary<string, List<WeaponSkill>> Dic)
        {
            var DicValue = Dic.Where(x => x.Key == MainName).FirstOrDefault().Value;
            if (DicValue != null)
            {   
                var skill = DicValue.FirstOrDefault(x => x.Extra_Name == Suffix);
                Suffix = skill == null ? "" : skill.Extra_Name;
            }
            else
            {
                Suffix = "";
            }
            return Suffix;
        }

        /// <summary>
        /// 判断目标字符串类型
        /// </summary>
        /// <param name="TargetStr">目标字符串</param>
        /// <param name="RegType">要判断的类型 1： 数字, 2： 整数, 3:电话 </param>
        /// <param name="CustomReg">自定义正则式 </param>
        /// <returns>是否为要判断的类型</returns>
        public static bool StringContentType(string TargetStr ,short RegType = 1,string CustomReg = null)
        {
            string RegStr = "";
            if (CustomReg != null)
            {
                RegStr = CustomReg;
            }
            else
            {
                switch (RegType)
                {
                    //IsNumeric
                    case 1: RegStr = @"^[+-]?\d*[.]?\d*$";
                        break;
                    //isInt
                    case 2: RegStr = @"^[+-]?\d*$";
                        break;
                    //isTel
                    case 3: RegStr = @"\d{3}-\d{8}|\d{4}-\d{7}";
                        break;
                    default: RegStr = "";
                        break;
                }
            }
            return Regex.IsMatch(TargetStr, RegStr);
        }


    }

    /// <summary>
    /// 具有 （武器类型，属性，武器系列，稀有度，武器分类，武器上限解放程度）筛选词缀 的筛选器
    /// </summary>
    public class FilterCondition : abstractModel
    {
        /// <summary>
        /// 获取一个筛选器
        /// </summary>
        public FilterCondition()
        {
            Construction();
        }

        public List<Weapon> TargetList = new List<Weapon>();
        public List<string> SearchTip = new List<string>();

        #region Property

        private int _NowPage;
        private int _PageCount;
        private int _pageContentCount;
        private string _SearchName;


        /// <summary>
        /// 搜索名称
        /// </summary>
        public string SearchName
        {
            get { return _SearchName; }
            set { _SearchName = value; this.RaisePropertyChanged(x => x.SearchName); }
        }
        /// <summary>
        /// 每页有多少内容
        /// </summary>
        public int pageContentCount
        {
            get { return _pageContentCount; }
            set { _pageContentCount = value; this.RaisePropertyChanged(x => x.pageContentCount); }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return _PageCount; }
            set { _PageCount = value; this.RaisePropertyChanged(x => x.PageCount); }
        }
        /// <summary>
        /// 当前是第几页
        /// </summary>
        public int NowPage
        {
            get { return _NowPage; }
            set { _NowPage = value; this.RaisePropertyChanged(x => x.NowPage); }
        }
        

        /// <summary>
        /// 武器类型列表
        /// </summary>
        public List<Weapon.WeaponKind> WeanponKindList = new List<Weapon.WeaponKind>();
        /// <summary>
        /// 武器属性列表
        /// </summary>
        public List<Weapon.GBFElementCHSEnum> WeaponElementList = new List<GBFMessageAbstractModel.GBFElementCHSEnum>();
        /// <summary>
        /// 稀有度列表
        /// </summary>
        public List<Weapon.GBFRarityEnum> WeaponRarityList = new List<GBFMessageAbstractModel.GBFRarityEnum>();
        /// <summary>
        /// 武器系列日文对照
        /// </summary>
        public Dictionary<string, Weapon.GBFSeriesNameEnum> WeaponJPSeriesNameDic = new Dictionary<string, Weapon.GBFSeriesNameEnum>();
        /// <summary>
        /// 武器系列列表
        /// </summary>
        public List<Weapon.GBFSeriesNameEnum> WeaponSeriesNameList = new List<Weapon.GBFSeriesNameEnum>();
        /// <summary>
        /// 武器卡池列表
        /// </summary>
        public List<Weapon.GBFCategoryEnum> WeaponCategoryList = new List<GBFMessageAbstractModel.GBFCategoryEnum>();
        /// <summary>
        /// 武器终突次数筛选列表
        /// </summary>
        public List<Weapon.GFBSearchEvoCounEnum> WeaponSearchEvoCounList = new List<Weapon.GFBSearchEvoCounEnum>();
        /// <summary>
        /// 武器排序类型列表
        /// </summary>
        public List<Weapon.SearchSortTypeEnum> SearchSortTypeList = new List<GBFMessageAbstractModel.SearchSortTypeEnum>();


        private short _FnGBF_MaxEvo;
        private int _WeaponSkillID;
        private Weapon.GBFCategoryEnum _WeaponCategory;
        private Weapon.GBFSeriesNameEnum _Fc_SeriesName;
        private Weapon.GBFRarityEnum _Fc_WeaponRarity;
        private Weapon.GBFElementCHSEnum _Fc_WeaponElement;
        private Weapon.WeaponKind _Fc_WeanponKind;
        private Weapon.GFBSearchEvoCounEnum _SearchEvoCount;
        private Weapon.SearchSortTypeEnum _SearchSortType;


        /// <summary>
        /// 武器排序类型
        /// </summary>
        public Weapon.SearchSortTypeEnum SearchSortType
        {
            get { return _SearchSortType; }
            set { _SearchSortType = value; this.RaisePropertyChanged(x => x.SearchSortType); }
        }
        /// <summary>
        /// 武器终突次数
        /// </summary>
        public Weapon.GFBSearchEvoCounEnum SearchEvoCount
        {
            get { return _SearchEvoCount; }
            set { _SearchEvoCount = value; this.RaisePropertyChanged(x => x.SearchEvoCount); }
        }
        /// <summary>
        /// 武器类型
        /// </summary>
        public Weapon.WeaponKind Fc_WeanponKind
        {
            get { return _Fc_WeanponKind; }
            set { _Fc_WeanponKind = value; this.RaisePropertyChanged(x => x.Fc_WeanponKind); }
        }
        /// <summary>
        /// 武器属性
        /// </summary>
        public Weapon.GBFElementCHSEnum Fc_WeaponElement
        {
            get { return _Fc_WeaponElement; }
            set { _Fc_WeaponElement = value; this.RaisePropertyChanged(x => x.Fc_WeaponElement); }
        }
        /// <summary>
        /// 稀有度
        /// </summary>
        public Weapon.GBFRarityEnum Fc_WeaponRarity
        {
            get { return _Fc_WeaponRarity; }
            set { _Fc_WeaponRarity = value; this.RaisePropertyChanged(x => x.Fc_WeaponRarity); }
        }
        /// <summary>
        /// 武器系列
        /// </summary>
        public Weapon.GBFSeriesNameEnum Fc_SeriesName
        {
            get { return _Fc_SeriesName; }
            set { _Fc_SeriesName = value; this.RaisePropertyChanged(x => x.Fc_SeriesName); }
        }
        /// <summary>
        /// 武器所属卡池
        /// </summary>
        public Weapon.GBFCategoryEnum WeaponCategory
        {
            get { return _WeaponCategory; }
            set { _WeaponCategory = value; this.RaisePropertyChanged(x => x.WeaponCategory); }
        }
        /// <summary>
        /// 技能ID
        /// </summary>
        public int WeaponSkillID
        {
            get { return _WeaponSkillID; }
            set { _WeaponSkillID = value; this.RaisePropertyChanged(x => x.WeaponSkillID); }
        }
        /// <summary>
        /// 武器突破次数
        /// </summary>
        public short FnGBF_MaxEvo
        {
            get { return _FnGBF_MaxEvo; }
            set { _FnGBF_MaxEvo = value; this.RaisePropertyChanged(x => x.FnGBF_MaxEvo); }
        }

        #endregion

        #region Method

        /// <summary>
        /// 初始化构造器
        /// </summary>
        public void Construction()
        {
            #region 加载列表

            //加载武器类型列表
            foreach (var item in Enum.GetNames(typeof(Weapon.WeaponKind)))
            {
                WeanponKindList.Add((Weapon.WeaponKind)Enum.Parse(typeof(Weapon.WeaponKind), item));
            }
            //加载属性列表
            foreach (var item in Enum.GetNames(typeof(Weapon.GBFElementCHSEnum)))
            {
                WeaponElementList.Add((Weapon.GBFElementCHSEnum)Enum.Parse(typeof(Weapon.GBFElementCHSEnum), item));
            }
            //加载稀有度列表
            foreach (var item in Enum.GetNames(typeof(Weapon.GBFRarityEnum)))
            {
                WeaponRarityList.Add((Weapon.GBFRarityEnum)Enum.Parse(typeof(Weapon.GBFRarityEnum), item));
            }
            //加载武器系列列表
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var a = asm.GetManifestResourceNames();
            using (System.IO.Stream stream = asm.GetManifestResourceStream(asm.GetName().Name + ".Resources.Txt.FsSeries_Name.txt"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(stream);
                var Str = sr.ReadToEnd();
                var StrList = GBFDesktopTools.Model.ToolAndHelper.ToolsAndHelper.SplitString(Str, 2);
                foreach (var item in StrList)
                {
                    var s = item.Replace("\n", "").Replace("\r", "");
                    var SeriesNameList = GBFDesktopTools.Model.ToolAndHelper.ToolsAndHelper.SplitString(s, CustomStr: "=");
                    WeaponJPSeriesNameDic.Add(SeriesNameList[0], (Weapon.GBFSeriesNameEnum)Enum.Parse(typeof(Weapon.GBFSeriesNameEnum), SeriesNameList[1]));
                    WeaponSeriesNameList.Add((Weapon.GBFSeriesNameEnum)Enum.Parse(typeof(Weapon.GBFSeriesNameEnum), SeriesNameList[1]));
                }
                WeaponSeriesNameList.Add(Weapon.GBFSeriesNameEnum.六限武器_月底);
                WeaponSeriesNameList.Add(Weapon.GBFSeriesNameEnum.六限武器_月中);
                WeaponSeriesNameList.Sort();
                sr.Close();
            }
            //加载武器卡池列表
            foreach (var item in Enum.GetNames(typeof(Weapon.GBFCategoryEnum)))
            {
                WeaponCategoryList.Add((Weapon.GBFCategoryEnum)Enum.Parse(typeof(Weapon.GBFCategoryEnum), item));
            }
            //加载武器突破次数列表
            foreach (var item in Enum.GetNames(typeof(Weapon.GFBSearchEvoCounEnum)))
            {
                WeaponSearchEvoCounList.Add((Weapon.GFBSearchEvoCounEnum)Enum.Parse(typeof(Weapon.GFBSearchEvoCounEnum), item));
            }
            //加载武器排序类型列表
            foreach (var item in Enum.GetNames(typeof(Weapon.SearchSortTypeEnum)))
            {
                SearchSortTypeList.Add((Weapon.SearchSortTypeEnum)Enum.Parse(typeof(Weapon.SearchSortTypeEnum), item));
            }

            #endregion

            //查询条件初始化
            Fc_WeaponElement = GBFMessageAbstractModel.GBFElementCHSEnum.全部;
            Fc_WeaponRarity = GBFMessageAbstractModel.GBFRarityEnum.SSR;
            SearchSortType = GBFMessageAbstractModel.SearchSortTypeEnum.最新登场;
            Fc_SeriesName = Weapon.GBFSeriesNameEnum.全部系列;
            Fc_WeanponKind = Weapon.WeaponKind.ALL;
            FnGBF_MaxEvo = 1;
            WeaponSkillID = -1;

            //分页属性初始化
            NowPage = 0;
            pageContentCount = 50;
            PageCount = 0;
        }

        /// <summary>
        /// 获取一个搜索提示列表
        /// </summary>
        /// <param name="_WeaponList"></param>
        public void GetSearchTipList(List<Weapon> _WeaponList)
        {
            TargetList = _WeaponList;
            if (TargetList == null || TargetList.Count < 1)
            {
                return;
            }
            foreach (var wp in TargetList)
            {
                SearchTip.Add(wp.FsName_EN);
                SearchTip.Add(wp.FsName_JP);
                SearchTip.Add(wp.FsName_CHS);
                foreach (var nikeName in wp.FsGBF_Nickname)
                {
                    SearchTip.Add(nikeName);
                }
                foreach (var SnikeName in wp.FsSearch_Nickname)
                {
                    SearchTip.Add(SnikeName);
                }
            }
           SearchTip = SearchTip.GroupBy(p => p).Select(s => s.Key).ToList();
        }

        /// <summary>
        /// 搜索提示列表排序
        /// </summary>
        public class SearchTipListReverserClass : IComparer<string>
        {
            public SearchTipListReverserClass(String _RegexStr)
            {
                RegexStr = _RegexStr;
            }

            public string RegexStr { get; set; }

            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer<string>.Compare(string x, string y)
            {
                if (x == null|| x == string.Empty || y == null || y == string.Empty)
                {
                    return 0;
                }
                int xIndex = 0;
                int yIndex = 0;

                xIndex = x.IndexOf(RegexStr, StringComparison.OrdinalIgnoreCase);
                yIndex = y.IndexOf(RegexStr, StringComparison.OrdinalIgnoreCase);
                
                return xIndex < yIndex ? 1 : xIndex > yIndex ? -1 :0 ;
            }
        }

        /// <summary>
        /// 武器列表排序模式
        /// </summary>
        public class WeaponListReverserClass : IComparer<Weapon>
        {
            public WeaponListReverserClass(GBFMessageAbstractModel.SearchSortTypeEnum _SortType)
            {
                SortType = _SortType;
            }

            public GBFMessageAbstractModel.SearchSortTypeEnum SortType { get; set; }

            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer<Weapon>.Compare(Weapon x, Weapon y)
            {
                if (x == null || y == null)
                {
                    return 0;
                }
                DateTime xDate = new DateTime();
                DateTime yDate = new DateTime();
                List<DateTime> DateTimeList = new List<DateTime>();
                switch (SortType)
                {
                    case GBFMessageAbstractModel.SearchSortTypeEnum.最新登场:
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare(y.FdGBF_ReleaseDate, x.FdGBF_ReleaseDate));
                    case GBFMessageAbstractModel.SearchSortTypeEnum.最早登场:
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare(y.FdGBF_ReleaseDate, x.FdGBF_ReleaseDate));
                    case GBFMessageAbstractModel.SearchSortTypeEnum.最近更新:
                        DateTimeList.AddRange(new List<DateTime>() { x.FdGBF_ReleaseDate, x.FdGBF_Star4, x.FdGBF_Star5, x.FdGBF_LastDate });
                        xDate = DateTimeList.Max();
                        DateTimeList.AddRange(new List<DateTime>() { y.FdGBF_ReleaseDate, y.FdGBF_Star4, y.FdGBF_Star5, y.FdGBF_LastDate });
                        yDate = DateTimeList.Max();
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare(xDate, yDate));
                    case GBFMessageAbstractModel.SearchSortTypeEnum.远古更新:
                        DateTimeList.AddRange(new List<DateTime>() { x.FdGBF_ReleaseDate, x.FdGBF_Star4, x.FdGBF_Star5, x.FdGBF_LastDate });
                        xDate = DateTimeList.Max();
                        DateTimeList.AddRange(new List<DateTime>() { y.FdGBF_ReleaseDate, y.FdGBF_Star4, y.FdGBF_Star5, y.FdGBF_LastDate });
                        yDate = DateTimeList.Max();
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare(xDate, yDate));
                    case GBFMessageAbstractModel.SearchSortTypeEnum.属性:
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare((int)y.FeGBF_Element, (int)x.FeGBF_Element));
                    case GBFMessageAbstractModel.SearchSortTypeEnum.类型:
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare((int)y.FeWeapon_Kind, (int)x.FeWeapon_Kind));
                    default:
                        break;
                }
                return 0;
            }
        }

        public void ClearPageCount()
        {
            NowPage = 0;
            PageCount = 0;
        }

        /// <summary>
        /// 筛选器查询方法
        /// </summary>
        /// <param name="Page">查询的页数</param>
        /// <param name="SearchName">搜索名称</param>
        /// <returns></returns>
        public ObjectResult<Weapon> FilterRun()
        {   
            var ResultObj = new ObjectResult<Weapon>();
            
            if (NowPage <= 0)
            {
                ClearPageCount();
                return ResultObj.SetError("页码不正确");
            }
            if (TargetList.Count < 1 || TargetList == null)
            {
                return ResultObj.SetError("武器数据库中无数据");
            }
            List<Weapon> ResultList = TargetList;

            //搜索
            if (SearchName != "" && SearchName != null)
            {
                ResultList = ResultList.Where(x =>
                                          Regex.IsMatch(x.FsName_CHS,SearchName) ||
                                          Regex.IsMatch(x.FsName_EN,SearchName) ||
                                          Regex.IsMatch(x.FsName_JP, SearchName) ||
                                          x.FsSearch_Nickname.Exists(n => Regex.IsMatch(n, SearchName)) ||
                                          x.FsGBF_Nickname.Exists(n => Regex.IsMatch(n, SearchName))).ToList();
            }

            try
            {
                //筛选器筛选操作
                if (Fc_WeaponElement != GBFMessageAbstractModel.GBFElementCHSEnum.全部)
                {
                    ResultList = ResultList.Where(x => x.FeGBF_Element == Fc_WeaponElement).ToList();
                }
                if (Fc_WeaponRarity != GBFMessageAbstractModel.GBFRarityEnum.all)
                {
                    ResultList = ResultList.Where(x => x.FeGBF_Rarity == Fc_WeaponRarity).ToList();
                }
                if (Fc_WeanponKind != Weapon.WeaponKind.ALL)
                {
                    ResultList = ResultList.Where(x => x.FeWeapon_Kind == Fc_WeanponKind).ToList();
                }
                if (Fc_SeriesName != Weapon.GBFSeriesNameEnum.全部系列)
                {
                    ResultList = ResultList.Where(x => x.FsSeries_Name == Fc_SeriesName).ToList();
                }
                if (WeaponSkillID != -1)
                {
                    ResultList = ResultList.Where(x => x.WeaponSkill.Exists(s => s.Skill_ID == WeaponSkillID)).ToList();
                }
                if (FnGBF_MaxEvo != 1)
                {
                    ResultList = ResultList.Where(x => x.FnGBF_MaxEvo == FnGBF_MaxEvo).ToList();
                }
            }
            catch (Exception ex)
            {
                return ResultObj.SetError("筛选器错误：" + ex.Message);
            }
            
            //排序前的判空
            if (ResultList.Count < 1 || ResultList == null)
            {
                ClearPageCount();
                return ResultObj;
            }
            //传入排序类型 排序
            ResultList.Sort(new WeaponListReverserClass(SearchSortType));
            //升序降序的反转
            if (SearchSortType == GBFMessageAbstractModel.SearchSortTypeEnum.最近更新 ||
                SearchSortType == GBFMessageAbstractModel.SearchSortTypeEnum.属性 ||
                SearchSortType == GBFMessageAbstractModel.SearchSortTypeEnum.最早登场)
            {
                ResultList.Reverse();
            }

            //分页设置
            if (ResultList.Count > 0)
            {
                PageCount = (ResultList.Count % pageContentCount > 0) ? ResultList.Count / pageContentCount + 1 : ResultList.Count / pageContentCount;
                if (NowPage > PageCount)
                {
                    ClearPageCount();
                    return ResultObj.SetError("页码超出范围");
                }
                else
                {
                    if (NowPage == PageCount)
                    {
                        int LastPageCount = ResultList.Count - (PageCount - 1) * pageContentCount;
                        ResultObj.ObjList = ResultList.GetRange(ResultList.Count - LastPageCount, LastPageCount);
                        return ResultObj;
                    }
                    ResultList = ResultList.GetRange(NowPage * pageContentCount - pageContentCount, pageContentCount);
                }
                
                ResultObj.ObjList = ResultList;
            }
            else
            {
                ClearPageCount();
                return ResultObj;
            }

            return ResultObj;
        }

        #endregion
    }
}
