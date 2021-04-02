using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using static GBFDesktopTools.Model.Weapon;

namespace GBFDesktopTools.Model.ToolAndHelper
{
    using abstractModel;

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
            var resourcesName = asm.GetName().Name + ".Resources.Txt.SpecialSkillList.txt";

            string tempStr;

            using (var stream = asm.GetManifestResourceStream(resourcesName))
            {
                if (stream == null)
                {
                    throw new Exception("读取特殊技能列表错误");
                }
                var sr = new StreamReader(stream);
                tempStr = sr.ReadToEnd();
                sr.Close();
            }
            var list = SplitString(tempStr.Trim(), 2, isHaveEmpty: StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Replace("\n", "").Replace("\r", "");
            }
            return list;
        }

        /// <summary>
        /// 按预设的符号分割字符串 -->  ; ；
        /// </summary>
        /// <param name="target">目标字符串</param>
        /// <param name="splitType">预设分割符号类型 1(；;) 2:(,) 3:(_) </param>
        /// <param name="customStr">自定义单分隔符 如果同时使用CustomArray参数 以CustomStr为优先</param>
        /// <param name="customArray">自定义分隔符列表 如果同时使用CustomStr参数 以CustomStr为优先</param>
        /// <param name="isHaveEmpty">返回的数组是否包含空字符串</param>
        /// <returns></returns>
        public static List<string> SplitString(string target, short splitType = 1, string customStr = null, string[] customArray = null, StringSplitOptions isHaveEmpty = StringSplitOptions.None)
        {
            string[] delimiter = null;
            //自定义单个分隔符
            if (customStr != null)
            {
                delimiter = new[] { customStr };
            }
            //自定义分隔符数组
            if (customArray != null && customStr == null)
            {
                delimiter = customArray;
            }
            //预设分隔符
            if (customStr != null || customArray != null) return target.Split(delimiter, isHaveEmpty).ToList();
            switch (splitType)
            {
                case 1:
                    delimiter = new[] { ";", "；" };
                    break;

                case 2:
                    delimiter = new[] { "," };
                    break;

                case 3:
                    delimiter = new[] { "_" };
                    break;
            }
            return target.Split(delimiter, isHaveEmpty).ToList();
        }

        /// <summary>
        /// 获取技能副名称
        /// </summary>
        /// <param name="mainName">技能主名称</param>
        /// <param name="extraName">技能副名称</param>
        /// <param name="dic">技能字典集</param>
        /// <returns>如果找到副名称则返回 主名称+副名称 否则返回空字符串</returns>
        public static string FoundSkillExtraName(string mainName, string extraName, Dictionary<string, List<WeaponSkill>> dic)
        {
            var dicKey = dic.Keys.Where(x => StringContentType(x, customReg: mainName)).ToList();
            extraName = dicKey.Any() ? dicKey.FirstOrDefault(f => f.Replace(mainName + "_", "") == extraName) : null;
            return string.IsNullOrEmpty(extraName) ? "" : extraName;
        }

        /// <summary>
        /// 验证技能后缀是否存在
        /// </summary>
        /// <param name="mainName">技能名称</param>
        /// <param name="suffix">技能后缀</param>
        /// <param name="dic">技能字典集</param>
        /// <returns>如果不存在则返回空字符串，存在则返回技能后缀</returns>
        public static string FoundSkillSuffix(string mainName, string suffix, Dictionary<string, List<WeaponSkill>> dic)
        {
            var dicValue = dic.FirstOrDefault(x => x.Key == mainName).Value;
            var skill = dicValue.FirstOrDefault(x => x.Extra_Name == suffix);
            suffix = skill == null ? "" : skill.Extra_Name;
            return suffix;
        }

        /// <summary>
        /// 判断目标字符串类型 或正则匹配
        /// </summary>
        /// <param name="targetStr">目标字符串</param>
        /// <param name="regType">要判断的类型 1： 数字, 2： 整数, 3:电话 </param>
        /// <param name="customReg">自定义正则式 </param>
        /// <returns>是否为要判断的类型</returns>
        public static bool StringContentType(string targetStr, short regType = 1, string customReg = null)
        {
            string regStr;
            if (customReg != null)
            {
                regStr = customReg;
            }
            else
            {
                switch (regType)
                {
                    //IsNumeric
                    case 1:
                        regStr = @"^[+-]?\d*[.]?\d*$";
                        break;
                    //isInt
                    case 2:
                        regStr = @"^[+-]?\d*$";
                        break;
                    //isTel
                    case 3:
                        regStr = @"\d{3}-\d{8}|\d{4}-\d{7}";
                        break;

                    default:
                        regStr = "";
                        break;
                }
            }
            return Regex.IsMatch(targetStr, regStr);
        }

        /// <summary>
        /// 获取一个以技能目标枚举类型作为键，中文技能目标类型描述为值的字典集
        /// </summary>
        /// <returns></returns>
        public static Dictionary<WeaponSkill.SkillTargetEnum, string> GetSkillTargetChsNameDictionary()
        {
            return new Dictionary<WeaponSkill.SkillTargetEnum, string>()
            {
                {WeaponSkill.SkillTargetEnum.Attack,"攻击力"},

                {WeaponSkill.SkillTargetEnum.HP,"生命值"},
                {WeaponSkill.SkillTargetEnum.Cut,"伤害减免"},
                {WeaponSkill.SkillTargetEnum.Defense,"防御力"},

                {WeaponSkill.SkillTargetEnum.CritDamage,"暴击伤害"},
                {WeaponSkill.SkillTargetEnum.CritProbability,"暴击几率"},

                {WeaponSkill.SkillTargetEnum.DA,"DA几率(连续攻击两次)"},
                {WeaponSkill.SkillTargetEnum.TA,"TA几率(连续攻击三次)"},
                {WeaponSkill.SkillTargetEnum.Counter,"反击伤害(回避)几率"},
                {WeaponSkill.SkillTargetEnum.AdditionalDamage_NormalAttack,"普攻追伤"},
                {WeaponSkill.SkillTargetEnum.UpperLimit_NormalAttack,"普攻伤害上限"},

                {WeaponSkill.SkillTargetEnum.SkillDamage,"技能伤害"},
                {WeaponSkill.SkillTargetEnum.UpperLimit_Skill,"技能伤害上限"},

                {WeaponSkill.SkillTargetEnum.ChainBurstDamage,"连锁奥义伤害(cb)"},
                {WeaponSkill.SkillTargetEnum.UltimateSkillDamage,"奥义伤害"},
                {WeaponSkill.SkillTargetEnum.UpperLimit_UltimateSkill,"奥义伤害上限"},
                {WeaponSkill.SkillTargetEnum.UpperLimit_ChainBurst,"连锁奥义伤害(cb)上限"},

                {WeaponSkill.SkillTargetEnum.Barrier,"屏障"},
                {WeaponSkill.SkillTargetEnum.Resist,"弱体耐性"},
                {WeaponSkill.SkillTargetEnum.Heal_limit,"生命回复上限"}
            };
        }

        /// <summary>
        /// 获取一个特殊技能枚举类型为键，特殊技能的描述信息为值的字典集
        /// </summary>
        /// <returns></returns>
        public static Dictionary<WeaponSkill.Condition, string> GetSpecialSkillDictionary()
        {
            return new Dictionary<WeaponSkill.Condition, string>()
            {
                {WeaponSkill.Condition.Guard,"生命值越少,防御力越高"},
                {WeaponSkill.Condition.BackWater,"生命值越少,攻击力越高"},
                {WeaponSkill.Condition.Whole,"生命值越高，攻击力越高"},

                {WeaponSkill.Condition.barrier,"获得屏障，持续到消耗完"},

                {WeaponSkill.Condition.FightBegin,"在战斗开始时"},
                {WeaponSkill.Condition.JinJin,"经过的回合越多,攻击力越高(13回合达到最大值)"},

                {WeaponSkill.Condition.Arcarum,"在阿卡姆转世中,"},

                {WeaponSkill.Condition.Damaged,"收到伤害时"},

                {WeaponSkill.Condition.Primal,"星晶兽角色的"},
            };
        }
    }

    /// <summary>
    /// 具有 （武器类型，属性，武器系列，稀有度，武器分类，武器上限解放程度）筛选词缀 的筛选器，
    /// 提供一系列筛选的操作方法，以及筛选词缀列表。
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

        /// <summary>
        /// 当前页码变更时在property中触发
        /// </summary>
        public EventHandler loadWeaponListHandler;

        #region Property

        private int _nowPage;
        private int _pageCount;
        private int _pageContentCount;
        private string _searchName;

        /// <summary>
        /// 搜索名称
        /// </summary>
        public string SearchName
        {
            get => _searchName;
            set { _searchName = value; this.RaisePropertyChanged(x => x.SearchName); }
        }

        /// <summary>
        /// 每页有多少内容
        /// </summary>
        public int PageContentCount
        {
            get => _pageContentCount;
            set { _pageContentCount = value; this.RaisePropertyChanged(x => x.PageContentCount); }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get => _pageCount;
            set { _pageCount = value; this.RaisePropertyChanged(x => x.PageCount); }
        }

        /// <summary>
        /// 当前是第几页
        /// </summary>
        public int NowPage
        {
            get => _nowPage;
            set
            {
                _nowPage = value; this.RaisePropertyChanged(x => x.NowPage);
                if (value == 0)
                {
                    return;
                }
                loadWeaponListHandler?.Invoke(new object(), new EventArgs());
            }
        }

        #region List

        /// <summary>
        /// 武器源列表
        /// </summary>
        public List<Weapon> TargetList = new List<Weapon>();

        /// <summary>
        /// 搜索提示列表
        /// </summary>
        public List<string> SearchTip = new List<string>();

        /// <summary>
        /// 武器类型列表
        /// </summary>
        public List<WeaponKind> WeaponKindList = new List<WeaponKind>();

        /// <summary>
        /// 武器属性列表
        /// </summary>
        public List<GBFMessageAbstractModel.GBFElementCHSEnum> WeaponElementList = new List<GBFMessageAbstractModel.GBFElementCHSEnum>();

        /// <summary>
        /// 稀有度列表
        /// </summary>
        public List<GBFMessageAbstractModel.GBFRarityEnum> WeaponRarityList = new List<GBFMessageAbstractModel.GBFRarityEnum>();

        /// <summary>
        /// 武器系列日文对照
        /// </summary>
        public Dictionary<string, GBFSeriesNameEnum> WeaponJpSeriesNameDic = new Dictionary<string, GBFSeriesNameEnum>();

        /// <summary>
        /// 武器系列列表
        /// </summary>
        public List<GBFSeriesNameEnum> WeaponSeriesNameList = new List<GBFSeriesNameEnum>();

        /// <summary>
        /// 武器卡池列表
        /// </summary>
        public List<GBFMessageAbstractModel.GBFCategoryEnum> WeaponCategoryList = new List<GBFMessageAbstractModel.GBFCategoryEnum>();

        /// <summary>
        /// 武器终突次数筛选列表
        /// </summary>
        public List<GFBSearchEvoCountEnum> WeaponSearchEvoCountList = new List<GFBSearchEvoCountEnum>();

        /// <summary>
        /// 武器排序类型列表
        /// </summary>
        public List<GBFMessageAbstractModel.SearchSortTypeEnum> SearchSortTypeList = new List<GBFMessageAbstractModel.SearchSortTypeEnum>();

        #endregion List

        private short _fnGbfMaxEvo;
        private string _weaponSkillName;
        private GBFMessageAbstractModel.GBFCategoryEnum _weaponCategory;
        private GBFSeriesNameEnum _fcSeriesName;
        private GBFMessageAbstractModel.GBFRarityEnum _fcWeaponRarity;
        private GBFMessageAbstractModel.GBFElementCHSEnum _fcWeaponElement;
        private WeaponKind _fcWeaponKind;
        private GFBSearchEvoCountEnum _searchEvoCount;
        private GBFMessageAbstractModel.SearchSortTypeEnum _searchSortType;

        /// <summary>
        /// 武器排序类型
        /// </summary>
        public GBFMessageAbstractModel.SearchSortTypeEnum SearchSortType
        {
            get => _searchSortType;
            set { _searchSortType = value; this.RaisePropertyChanged(x => x.SearchSortType); }
        }

        /// <summary>
        /// 武器终突次数
        /// </summary>
        public GFBSearchEvoCountEnum SearchEvoCount
        {
            get => _searchEvoCount;
            set { _searchEvoCount = value; this.RaisePropertyChanged(x => x.SearchEvoCount); }
        }

        /// <summary>
        /// 武器类型
        /// </summary>
        public WeaponKind FcWeaponKind
        {
            get => _fcWeaponKind;
            set { _fcWeaponKind = value; this.RaisePropertyChanged(x => x.FcWeaponKind); }
        }

        /// <summary>
        /// 武器属性
        /// </summary>
        public GBFMessageAbstractModel.GBFElementCHSEnum FcWeaponElement
        {
            get => _fcWeaponElement;
            set { _fcWeaponElement = value; this.RaisePropertyChanged(x => x.FcWeaponElement); }
        }

        /// <summary>
        /// 稀有度
        /// </summary>
        public GBFMessageAbstractModel.GBFRarityEnum FcWeaponRarity
        {
            get => _fcWeaponRarity;
            set { _fcWeaponRarity = value; this.RaisePropertyChanged(x => x.FcWeaponRarity); }
        }

        /// <summary>
        /// 武器系列
        /// </summary>
        public GBFSeriesNameEnum FcSeriesName
        {
            get => _fcSeriesName;
            set { _fcSeriesName = value; this.RaisePropertyChanged(x => x.FcSeriesName); }
        }

        /// <summary>
        /// 武器所属卡池
        /// </summary>
        public GBFMessageAbstractModel.GBFCategoryEnum WeaponCategory
        {
            get => _weaponCategory;
            set { _weaponCategory = value; this.RaisePropertyChanged(x => x.WeaponCategory); }
        }

        /// <summary>
        /// 技能ID
        /// </summary>
        public string WeaponSkillName
        {
            get => _weaponSkillName;
            set { _weaponSkillName = value; this.RaisePropertyChanged(x => x.WeaponSkillName); }
        }

        /// <summary>
        /// 武器突破次数
        /// </summary>
        public short FnGbfMaxEvo
        {
            get => _fnGbfMaxEvo;
            set { _fnGbfMaxEvo = value; this.RaisePropertyChanged(x => x.FnGbfMaxEvo); }
        }

        #endregion Property

        #region Method

        /// <summary>
        /// 初始化构造器
        /// </summary>
        public void Construction()
        {
            #region 加载列表

            //加载武器类型列表
            foreach (var item in Enum.GetNames(typeof(WeaponKind)))
            {
                WeaponKindList.Add((WeaponKind)Enum.Parse(typeof(WeaponKind), item));
            }
            //加载属性列表
            foreach (var item in Enum.GetNames(typeof(GBFMessageAbstractModel.GBFElementCHSEnum)))
            {
                WeaponElementList.Add((GBFMessageAbstractModel.GBFElementCHSEnum)Enum.Parse(typeof(GBFMessageAbstractModel.GBFElementCHSEnum), item));
            }
            //加载稀有度列表
            foreach (var item in Enum.GetNames(typeof(GBFMessageAbstractModel.GBFRarityEnum)))
            {
                WeaponRarityList.Add((GBFMessageAbstractModel.GBFRarityEnum)Enum.Parse(typeof(GBFMessageAbstractModel.GBFRarityEnum), item));
            }
            //加载武器系列列表
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream(asm.GetName().Name + ".Resources.Txt.FsSeries_Name.txt"))
            {
                if (stream == null)
                {
                    return;
                }
                var sr = new StreamReader(stream);
                var str = sr.ReadToEnd();
                var strList = ToolsAndHelper.SplitString(str, 2);
                foreach (var seriesNameList in strList.Select(item => item.Replace("\n", "").Replace("\r", "")).Select(s => ToolsAndHelper.SplitString(s, customStr: "=")))
                {
                    WeaponJpSeriesNameDic.Add(seriesNameList[0], (GBFSeriesNameEnum)Enum.Parse(typeof(GBFSeriesNameEnum), seriesNameList[1]));
                    WeaponSeriesNameList.Add((GBFSeriesNameEnum)Enum.Parse(typeof(GBFSeriesNameEnum), seriesNameList[1]));
                }
                WeaponSeriesNameList.Sort();
                WeaponSeriesNameList.Insert(0, GBFSeriesNameEnum.全部系列);
                sr.Close();
            }
            //加载武器卡池列表
            foreach (var item in Enum.GetNames(typeof(GBFMessageAbstractModel.GBFCategoryEnum)))
            {
                WeaponCategoryList.Add((GBFMessageAbstractModel.GBFCategoryEnum)Enum.Parse(typeof(GBFMessageAbstractModel.GBFCategoryEnum), item));
            }
            //加载武器突破次数列表
            foreach (var item in Enum.GetNames(typeof(GFBSearchEvoCountEnum)))
            {
                WeaponSearchEvoCountList.Add((GFBSearchEvoCountEnum)Enum.Parse(typeof(GFBSearchEvoCountEnum), item));
            }
            //加载武器排序类型列表
            foreach (var item in Enum.GetNames(typeof(GBFMessageAbstractModel.SearchSortTypeEnum)))
            {
                SearchSortTypeList.Add((GBFMessageAbstractModel.SearchSortTypeEnum)Enum.Parse(typeof(GBFMessageAbstractModel.SearchSortTypeEnum), item));
            }

            #endregion 加载列表

            //查询条件初始化
            FcWeaponElement = GBFMessageAbstractModel.GBFElementCHSEnum.全部;
            FcWeaponRarity = GBFMessageAbstractModel.GBFRarityEnum.SSR;
            SearchSortType = GBFMessageAbstractModel.SearchSortTypeEnum.最新登场;
            FcWeaponRarity = GBFMessageAbstractModel.GBFRarityEnum.all;
            FcSeriesName = GBFSeriesNameEnum.全部系列;
            FcWeaponKind = WeaponKind.全部;
            WeaponSkillName = "全部技能";
            WeaponCategory = GBFMessageAbstractModel.GBFCategoryEnum.全部;
            SearchEvoCount = GFBSearchEvoCountEnum.全部;
            WeaponSkillName = "";

            //分页属性初始化
            NowPage = 1;
            PageContentCount = 36;
            PageCount = 0;
        }

        /// <summary>
        /// 获取一个搜索提示列表
        /// </summary>
        /// <param name="weaponList"></param>
        public void GetSearchTipList(List<Weapon> weaponList)
        {
            TargetList = weaponList;
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
                foreach (var nikeName in wp.FsSearch_Nickname)
                {
                    SearchTip.Add(nikeName);
                }
            }
            SearchTip = SearchTip.GroupBy(p => p).Select(s => s.Key).ToList();
        }

        /// <summary>
        /// 搜索提示列表排序
        /// </summary>
        public class SearchTipListReverserClass : IComparer<string>
        {
            public SearchTipListReverserClass(string regexStr)
            {
                RegexStr = regexStr;
            }

            public string RegexStr { get; set; }

            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer<string>.Compare(string x, string y)
            {
                if (string.IsNullOrEmpty(x) || y == null || y == string.Empty)
                {
                    return 0;
                }

                var xIndex = x.IndexOf(RegexStr, StringComparison.OrdinalIgnoreCase);
                var yIndex = y.IndexOf(RegexStr, StringComparison.OrdinalIgnoreCase);
                return xIndex < yIndex ? 1 : xIndex > yIndex ? -1 : 0;
            }
        }

        /// <summary>
        /// 武器列表排序模式
        /// </summary>
        public class WeaponListReverserClass : IComparer<Weapon>
        {
            //根据排序类型排序
            public WeaponListReverserClass(GBFMessageAbstractModel.SearchSortTypeEnum sortType)
            {
                SortType = sortType;
            }

            public bool IsNameSort;

            public GBFMessageAbstractModel.SearchSortTypeEnum SortType { get; set; }

            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer<Weapon>.Compare(Weapon x, Weapon y)
            {
                if (x == null || y == null)
                {
                    return 0;
                }
                if (IsNameSort)
                {
                    return ((new System.Collections.CaseInsensitiveComparer()).Compare(y.GetCanUseName, x.GetCanUseName));
                }
                DateTime xDate;
                DateTime yDate;
                var dateTimeList = new List<DateTime>();
                switch (SortType)
                {
                    case GBFMessageAbstractModel.SearchSortTypeEnum.最新登场:
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare(y.FdGBF_ReleaseDate, x.FdGBF_ReleaseDate));

                    case GBFMessageAbstractModel.SearchSortTypeEnum.最早登场:
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare(y.FdGBF_ReleaseDate, x.FdGBF_ReleaseDate));

                    case GBFMessageAbstractModel.SearchSortTypeEnum.最近更新:
                        dateTimeList.AddRange(new List<DateTime>() { x.FdGBF_ReleaseDate, x.FdGBF_Star4, x.FdGBF_Star5, x.FdGBF_LastDate });
                        xDate = dateTimeList.Max();
                        dateTimeList.AddRange(new List<DateTime>() { y.FdGBF_ReleaseDate, y.FdGBF_Star4, y.FdGBF_Star5, y.FdGBF_LastDate });
                        yDate = dateTimeList.Max();
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare(xDate, yDate));

                    case GBFMessageAbstractModel.SearchSortTypeEnum.远古更新:
                        dateTimeList.AddRange(new List<DateTime>() { x.FdGBF_ReleaseDate, x.FdGBF_Star4, x.FdGBF_Star5, x.FdGBF_LastDate });
                        xDate = dateTimeList.Max();
                        dateTimeList.AddRange(new List<DateTime>() { y.FdGBF_ReleaseDate, y.FdGBF_Star4, y.FdGBF_Star5, y.FdGBF_LastDate });
                        yDate = dateTimeList.Max();
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare(xDate, yDate));

                    case GBFMessageAbstractModel.SearchSortTypeEnum.属性:
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare((int)y.FeGBF_Element, (int)x.FeGBF_Element));

                    case GBFMessageAbstractModel.SearchSortTypeEnum.类型:
                        return ((new System.Collections.CaseInsensitiveComparer()).Compare((int)y.FeWeapon_Kind, (int)x.FeWeapon_Kind));
                }
                return 0;
            }
        }

        /// <summary>
        /// 筛选器查询方法
        /// </summary>
        /// <returns></returns>
        public ObjectResult<Weapon> FilterRun()
        {
            var resultObj = new ObjectResult<Weapon>();

            if (NowPage <= 0)
            {
                NowPage = 1;
            }
            if (TargetList.Count < 1 || TargetList == null)
            {
                return resultObj.SetError("武器数据库中无数据");
            }
            var resultList = TargetList;

            //搜索
            if (!string.IsNullOrEmpty(SearchName))
            {
                resultList = resultList.Where(x =>
                                          Regex.IsMatch(x.FsName_CHS, SearchName) ||
                                          Regex.IsMatch(x.FsName_EN, SearchName) ||
                                          Regex.IsMatch(x.FsName_JP, SearchName) ||
                                          x.FsSearch_Nickname.Exists(n => Regex.IsMatch(n, SearchName)) ||
                                          x.FsGBF_Nickname.Exists(n => Regex.IsMatch(n, SearchName))).ToList();
            }

            try
            {
                //筛选器筛选操作
                if (FcWeaponRarity != GBFMessageAbstractModel.GBFRarityEnum.all)
                {
                    resultList = resultList.Where(x => x.FeGBF_Rarity == FcWeaponRarity).ToList();
                }
                if (FcWeaponElement != GBFMessageAbstractModel.GBFElementCHSEnum.全部)
                {
                    resultList = resultList.Where(x => x.FeGBF_Element == FcWeaponElement).ToList();
                }
                if (FcSeriesName != GBFSeriesNameEnum.全部系列)
                {
                    resultList = resultList.Where(x => x.FsSeries_Name == FcSeriesName).ToList();
                }
                if (WeaponCategory != GBFMessageAbstractModel.GBFCategoryEnum.全部)
                {
                    if (WeaponCategory == GBFMessageAbstractModel.GBFCategoryEnum.月中 || WeaponCategory == GBFMessageAbstractModel.GBFCategoryEnum.月底)
                    {
                        resultList = resultList.Where(x => x.FsGBF_Tag == WeaponCategory.ToString()).ToList();
                    }
                    else
                    {
                        resultList = resultList.Where(x => x.FsGBF_Category == WeaponCategory).ToList();
                    }
                }
                if (FcWeaponKind != WeaponKind.全部)
                {
                    resultList = resultList.Where(x => x.FeWeapon_Kind == FcWeaponKind).ToList();
                }
                if (WeaponSkillName != "全部技能")
                {
                    resultList = resultList.Where(x => x.WeaponSkill.Exists(s => s.Extra_Description + s.Main_Description == WeaponSkillName)).ToList();
                }
                if (SearchEvoCount != GFBSearchEvoCountEnum.全部)
                {
                    FnGbfMaxEvo = (short)SearchEvoCount;
                    switch (FnGbfMaxEvo)
                    {
                        case 1:
                            resultList = resultList.Where(x => x.FnGBF_MaxEvo >= 4).ToList();
                            break;

                        case 3:
                        case 4:
                        case 5:
                            resultList = resultList.Where(x => x.FnGBF_MaxEvo == FnGbfMaxEvo).ToList();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                return resultObj.SetError("筛选器错误：" + ex.Message);
            }

            //排序前的判空
            if (resultList.Count < 1 || resultList == null)
            {
                NowPage = 0;
                PageCount = 0;
                resultObj.ObjList = resultList;
                return resultObj;
            }
            //传入排序类型 先按名称排序 再按排序类型排序
            var weaponListReverser = new WeaponListReverserClass(SearchSortType) { IsNameSort = true };
            resultList.Sort(weaponListReverser);
            weaponListReverser.IsNameSort = false;
            resultList.Sort(weaponListReverser);

            //升序降序的反转
            if (SearchSortType == GBFMessageAbstractModel.SearchSortTypeEnum.最近更新 ||
                SearchSortType == GBFMessageAbstractModel.SearchSortTypeEnum.属性 ||
                SearchSortType == GBFMessageAbstractModel.SearchSortTypeEnum.最早登场)
            {
                resultList.Reverse();
            }

            //分页设置
            if (resultList.Count > 0)
            {
                PageCount = (resultList.Count % PageContentCount > 0) ? resultList.Count / PageContentCount + 1 : resultList.Count / PageContentCount;
                if (NowPage > PageCount)
                {
                    //ClearPageCount();
                    return resultObj.SetError("页码超出范围");
                }
                else
                {
                    if (NowPage == PageCount)
                    {
                        var lastPageCount = resultList.Count - (PageCount - 1) * PageContentCount;
                        resultObj.ObjList = resultList.GetRange(resultList.Count - lastPageCount, lastPageCount);
                        return resultObj;
                    }
                    resultList = resultList.GetRange(NowPage * PageContentCount - PageContentCount, PageContentCount);
                }

                resultObj.ObjList = resultList;
            }
            else
            {
                //ClearPageCount();
                return resultObj;
            }

            return resultObj;
        }

        #endregion Method
    }

    /// <summary>
    /// 利用表达式树实现深拷贝的类
    /// </summary>
    /// <typeparam name="TSource">源对象类型</typeparam>
    /// <typeparam name="TTarget">目标对象类型</typeparam>
    internal static class Copier<TSource, TTarget>
    {
        // 缓存委托
        private static Func<TSource, TTarget> _copyFunc;

        private static Action<TSource, TTarget> _copyAction;

        /// <summary>
        /// 新建目标类型实例，并将源对象的属性值拷贝至目标对象的对应属性
        /// </summary>
        /// <param name="source">源对象实例</param>
        /// <returns>深拷贝了源对象属性的目标对象实例</returns>
        public static TTarget Copy(TSource source)
        {
            if (source == null) return default(TTarget);

            // 因为对于泛型类型而言，每次传入不同的泛型参数都会调用静态构造函数，所以可以通过这种方式进行缓存
            if (_copyFunc != null)
            {
                // 如果之前缓存过，则直接调用缓存的委托
                return _copyFunc(source);
            }

            Type sourceType = typeof(TSource);
            Type targetType = typeof(TTarget);

            var paramExpr = Expression.Parameter(sourceType, nameof(source));

            Expression bodyExpr;

            // 如果对象可以遍历（目前只支持数组和ICollection<T>实现类）
            if (sourceType == targetType && Utils.IsIEnumerableExceptString(sourceType))
            {
                bodyExpr = Expression.Call(null, EnumerableCopier.GetMethondInfo(sourceType), paramExpr);
            }
            else
            {
                var memberBindings = new List<MemberBinding>();
                // 遍历目标对象的所有属性信息
                foreach (var targetPropInfo in targetType.GetProperties())
                {
                    // 从源对象获取同名的属性信息
                    var sourcePropInfo = sourceType.GetProperty(targetPropInfo.Name);

                    Type sourcePropType = sourcePropInfo?.PropertyType;
                    Type targetPropType = targetPropInfo.PropertyType;

                    // 只在满足以下三个条件的情况下进行拷贝
                    // 1.源属性类型和目标属性类型一致
                    // 2.源属性可读
                    // 3.目标属性可写
                    if (sourcePropType == targetPropType
                        && sourcePropInfo.CanRead
                        && targetPropInfo.CanWrite)
                    {
                        // 获取属性值的表达式
                        Expression expression = Expression.Property(paramExpr, sourcePropInfo);

                        // 如果目标属性是值类型或者字符串，则直接做赋值处理
                        // 暂不考虑目标值类型有非字符串的引用类型这种特殊情况
                        // 非字符串引用类型做递归处理
                        if (Utils.IsRefTypeExceptString(targetPropType))
                        {
                            // 进行递归
                            if (Utils.IsRefTypeExceptString(targetPropType))
                            {
                                expression = Expression.Call(null,
                                    GetCopyMethodInfo(sourcePropType, targetPropType), expression);
                            }
                        }
                        memberBindings.Add(Expression.Bind(targetPropInfo, expression));
                    }
                }

                bodyExpr = Expression.MemberInit(Expression.New(targetType), memberBindings);
            }

            var lambdaExpr
                = Expression.Lambda<Func<TSource, TTarget>>(bodyExpr, paramExpr);

            _copyFunc = lambdaExpr.Compile();
            return _copyFunc(source);
        }

        /// <summary>
        /// 新建目标类型实例，并将源对象的属性值拷贝至目标对象的对应属性
        /// </summary>
        /// <param name="source">源对象实例</param>
        /// <param name="target">目标对象实例</param>
        public static void Copy(TSource source, TTarget target)
        {
            if (source == null) return;

            // 因为对于泛型类型而言，每次传入不同的泛型参数都会调用静态构造函数，所以可以通过这种方式进行缓存
            // 如果之前缓存过，则直接调用缓存的委托
            if (_copyAction != null)
            {
                _copyAction(source, target);
                return;
            }

            Type sourceType = typeof(TSource);
            Type targetType = typeof(TTarget);

            // 如果双方都可以被遍历
            if (Utils.IsIEnumerableExceptString(sourceType) && Utils.IsIEnumerableExceptString(targetType))
            {
                // TODO
                // 向已存在的数组或者ICollection<T>拷贝的功能暂不支持
            }
            else
            {
                var paramSourceExpr = Expression.Parameter(sourceType, nameof(source));
                var paramTargetExpr = Expression.Parameter(targetType, nameof(target));

                var binaryExpressions = new List<Expression>();
                // 遍历目标对象的所有属性信息
                foreach (var targetPropInfo in targetType.GetProperties())
                {
                    // 从源对象获取同名的属性信息
                    var sourcePropInfo = sourceType.GetProperty(targetPropInfo.Name);

                    Type sourcePropType = sourcePropInfo?.PropertyType;
                    Type targetPropType = targetPropInfo.PropertyType;

                    // 只在满足以下三个条件的情况下进行拷贝
                    // 1.源属性类型和目标属性类型一致
                    // 2.源属性可读
                    // 3.目标属性可写
                    if (sourcePropType == targetPropType
                        && sourcePropInfo.CanRead
                        && targetPropInfo.CanWrite)
                    {
                        // 获取属性值的表达式
                        Expression expression = Expression.Property(paramSourceExpr, sourcePropInfo);
                        Expression targetPropExpr = Expression.Property(paramTargetExpr, targetPropInfo);

                        // 如果目标属性是值类型或者字符串，则直接做赋值处理
                        // 暂不考虑目标值类型有非字符串的引用类型这种特殊情况
                        if (Utils.IsRefTypeExceptString(targetPropType))
                        {
                            expression = Expression.Call(null,
                                GetCopyMethodInfo(sourcePropType, targetPropType), expression);
                        }
                        binaryExpressions.Add(Expression.Assign(targetPropExpr, expression));
                    }
                }

                Expression bodyExpr = Expression.Block(binaryExpressions);

                var lambdaExpr
                    = Expression.Lambda<Action<TSource, TTarget>>(bodyExpr, paramSourceExpr, paramTargetExpr);

                _copyAction = lambdaExpr.Compile();
                _copyAction(source, target);
            }
        }

        private static MethodInfo GetCopyMethodInfo(Type source, Type target)
            => typeof(Copier<,>).MakeGenericType(source, target).GetMethod(nameof(Copy), new[] { source });
    }

    /// <summary>
    /// 工具类
    /// </summary>
    internal static class Utils
    {
        private static readonly Type _typeString = typeof(string);

        private static readonly Type _typeIEnumerable = typeof(IEnumerable);

        private static readonly ConcurrentDictionary<Type, Func<object>> _ctors = new ConcurrentDictionary<Type, Func<object>>();

        /// <summary>
        /// 判断是否是string以外的引用类型
        /// </summary>
        /// <returns>True：是string以外的引用类型，False：不是string以外的引用类型</returns>
        public static bool IsRefTypeExceptString(Type type)
            => !type.IsValueType && type != _typeString;

        /// <summary>
        /// 判断是否是string以外的可遍历类型
        /// </summary>
        /// <returns>True：是string以外的可遍历类型，False：不是string以外的可遍历类型</returns>
        public static bool IsIEnumerableExceptString(Type type)
            => _typeIEnumerable.IsAssignableFrom(type) && type != _typeString;

        /// <summary>
        /// 创建指定类型实例
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <returns>指定类型的实例</returns>
        public static object CreateNewInstance(Type type) =>
            _ctors.GetOrAdd(type,
                t => Expression.Lambda<Func<object>>(Expression.New(t)).Compile())();
    }

    /// <summary>
    /// 对尚不支持的类型进行拷贝时抛出的异常
    /// </summary>
    public class UnsupportedTypeException : Exception
    {
        /// <summary>
        /// 用指定的类型初始化 DeepCopier.UnsupportedTypeException 类的新实例
        /// </summary>
        /// <param name="type">暂不支持的类型信息</param>
        public UnsupportedTypeException(Type type) : base($"Type[{type.Name}] has not been supported yet.")
        {
        }
    }

    /// <summary>
    /// 可遍历类型拷贝器
    /// </summary>
    internal class EnumerableCopier
    {
        private static readonly MethodInfo _copyArrayMethodInfo;

        private static readonly MethodInfo _copyICollectionMethodInfo;

        private static readonly Type _typeICollection = typeof(ICollection<>);

        static EnumerableCopier()
        {
            Type type = typeof(EnumerableCopier);
            _copyArrayMethodInfo = type.GetMethod(nameof(CopyArray));
            _copyICollectionMethodInfo = type.GetMethod(nameof(CopyICollection));
        }

        /// <summary>
        /// 根据IEnumerable的实现类类型选择合适的拷贝方法
        /// </summary>
        /// <param name="type">IEnumerable的实现类类型</param>
        /// <returns>拷贝方法信息</returns>
        public static MethodInfo GetMethondInfo(Type type)
        {
            if (type.IsArray)
            {
                return _copyArrayMethodInfo.MakeGenericMethod(type.GetElementType());
            }
            else if (type.GetGenericArguments().Length > 0)
            {
                Type elementType = type.GetGenericArguments()[0];
                if (_typeICollection.MakeGenericType(elementType).IsAssignableFrom(type))
                {
                    return _copyICollectionMethodInfo.MakeGenericMethod(type, elementType);
                }
            }
            throw new UnsupportedTypeException(type);
        }

        /// <summary>
        /// 拷贝List
        /// </summary>
        /// <typeparam name="T">源ICollection实现类类型</typeparam>
        /// <typeparam name="TElement">源ICollection元素类型</typeparam>
        /// <param name="source">源ICollection对象</param>
        /// <returns>深拷贝完成的ICollection对象</returns>
        public static T CopyICollection<T, TElement>(T source)
            where T : ICollection<TElement>
        {
            T result = (T)Utils.CreateNewInstance(source.GetType());

            if (Utils.IsRefTypeExceptString(typeof(TElement)))
            {
                foreach (TElement item in source)
                {
                    result.Add(Copier<TElement, TElement>.Copy(item));
                }
            }
            else
            {
                foreach (TElement item in source)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// 拷贝数组
        /// </summary>
        /// <typeparam name="TElement">源数组元素类型</typeparam>
        /// <param name="source">源List</param>
        /// <returns>深拷贝完成的数组</returns>
        public static TElement[] CopyArray<TElement>(TElement[] source)
        {
            TElement[] result = new TElement[source.Length];
            if (Utils.IsRefTypeExceptString(typeof(TElement)))
            {
                for (int i = 0; i < source.Length; i++)
                {
                    result[i] = Copier<TElement, TElement>.Copy(source[i]);
                }
            }
            else
            {
                for (int i = 0; i < source.Length; i++)
                {
                    result[i] = source[i];
                }
            }
            return result;
        }
    }
}