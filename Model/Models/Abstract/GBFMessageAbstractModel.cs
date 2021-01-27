using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace GBFDesktopTools.Model.abstractModel
{   
    /// <summary>
    /// 继承了abstractModel（属性变更通知，提交或回滚数据更改，以及克隆对象的功能），属性泛用性较高如（稀有度，属性，姓名等） 武器角色等实体必须继承此类！
    /// </summary>
    public abstract class GBFMessageAbstractModel:abstractModel
    {
        #region Enum

        /// <summary>
        /// 稀有度枚举
        /// </summary>
        public enum GBFRarityEnum
        {   
            /// <summary>
            /// 筛选器词条
            /// </summary>
            all,
            Unknown = -1,
            N = 1,
            R = 2,
            SR = 3,
            SSR = 4
        }
        /// <summary>
        /// 中文元素枚举
        /// </summary>
        public enum GBFElementCHSEnum
        {
            全部 = 0,
            无属性 = 98,
            未知 = 99,
            火 = 1,
            水 = 2,
            土 = 3,
            风 = 4,
            光 = 5,
            暗 = 6,
            可变化 = 7
        }
        /// <summary>
        /// 英文元素枚举
        /// </summary>
        public enum GBFElementEnEnum
        {   
            all,
            unknow = -1,
            noHave = 0,
            fire = 1,
            water = 2,
            wind = 3,
            earth = 4,
            light = 5,
            dark = 6,
            any = 7
        }
        /// <summary>
        /// 卡池分类
        /// </summary>
        public enum GBFCategoryEnum
        {   
            全部,
            未知,
            普通,
            金币,
            月中,
            月底,
            活动,
            特典,
            泳装浴衣,
            万圣节,
            圣诞节,
            神将,
            情人节,
            称号
        }
        /// <summary>
        /// 排序类型
        /// </summary>
        public enum SearchSortTypeEnum
        {
            最新登场,
            最早登场,
            最近更新,
            远古更新,
            属性,
            类型
        }

        #endregion

        #region Property

        private string _FsTitle_JP;
        private Weapon.GBFSeriesNameEnum _FsSeries_Name;
        private GBFCategoryEnum _FsGBF_Category;
        private string _FsGBF_Tag;

        private List<string> _FsGBF_Nickname;
        private List<string> _FsSearch_Nickname;

        private string _FsName_JP;
        private string _FsName_EN;
        private string _FsName_CHS;

        private DateTime _FdGBF_ReleaseDate;
        private DateTime _FdGBF_Star4;
        private DateTime _FdGBF_Star5;
        private DateTime _FdGBF_LastDate;

        private string _FsGBF_LinkGamewith;
        private string _FsGBF_LinkEnwiki;

        private short _FnGBF_UserLevel;
        private GBFRarityEnum _FeGBF_Rarity;
        private GBFElementCHSEnum _FeGBF_Element;
        private short _FnGBF_BaseEvo;
        private short _FnGBF_MaxEvo;

        /// <summary>
        /// 标签
        /// </summary>
        public string FsGBF_Tag
        {
            get => _FsGBF_Tag;
            set { _FsGBF_Tag = value; this.RaisePropertyChanged(x => x.FsGBF_Tag); }
        }
        /// <summary>
        /// 类别
        /// </summary>
        public GBFCategoryEnum FsGBF_Category
        {
            get => _FsGBF_Category;
            set { _FsGBF_Category = value; this.RaisePropertyChanged(x => x.FsGBF_Category); }
        }
        /// <summary>
        /// 最大星级(突破次数)
        /// </summary>
        public short FnGBF_MaxEvo
        {
            get => _FnGBF_MaxEvo;
            set { _FnGBF_MaxEvo = value; this.RaisePropertyChanged(x => x.FnGBF_MaxEvo); }
        }
        /// <summary>
        /// 基础星级(突破次数)
        /// </summary>
        public short FnGBF_BaseEvo
        {
            get => _FnGBF_BaseEvo;
            set { _FnGBF_BaseEvo = value; this.RaisePropertyChanged(x => x.FnGBF_BaseEvo); }
        }
        /// <summary>
        /// 属性
        /// </summary>
        public GBFElementCHSEnum FeGBF_Element
        {
            get => _FeGBF_Element;
            set { _FeGBF_Element = value; this.RaisePropertyChanged(x => x.FeGBF_Element); }
        }
        /// <summary>
        /// 稀有度
        /// </summary>
        public GBFRarityEnum FeGBF_Rarity
        {
            get => _FeGBF_Rarity;
            set { _FeGBF_Rarity = value; this.RaisePropertyChanged(x => x.FeGBF_Rarity); }
        }
        /// <summary>
        /// 适合Rank等级
        /// </summary>
        public short FnGBF_UserLevel
        {
            get => _FnGBF_UserLevel;
            set { _FnGBF_UserLevel = value; this.RaisePropertyChanged(x => x.FnGBF_UserLevel); }
        }
        /// <summary>
        /// 英文Wiki链接
        /// </summary>
        public string FsGBF_LinkEnwiki
        {
            get => _FsGBF_LinkEnwiki;
            set { _FsGBF_LinkEnwiki = value; this.RaisePropertyChanged(x => x.FsGBF_LinkEnwiki); }
        }
        /// <summary>
        /// GameWith链接
        /// </summary>
        public string FsGBF_LinkGamewith
        {
            get => _FsGBF_LinkGamewith;
            set { _FsGBF_LinkGamewith = value; this.RaisePropertyChanged(x => x.FsGBF_LinkGamewith); }
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime FdGBF_LastDate
        {
            get => _FdGBF_LastDate;
            set { _FdGBF_LastDate = value; this.RaisePropertyChanged(x => x.FdGBF_LastDate); }
        }
        /// <summary>
        /// 四突时间
        /// </summary>
        public DateTime FdGBF_Star4
        {
            get => _FdGBF_Star4;
            set { _FdGBF_Star4 = value; this.RaisePropertyChanged(x => x.FdGBF_Star5); }
        }
        /// <summary>
        /// 五突时间
        /// </summary>
        public DateTime FdGBF_Star5
        {
            get => _FdGBF_Star5;
            set { _FdGBF_Star5 = value; this.RaisePropertyChanged(x => x.FdGBF_Star5); }
        }
        /// <summary>
        /// 登场时间
        /// </summary>
        public DateTime FdGBF_ReleaseDate
        {
            get => _FdGBF_ReleaseDate;
            set { _FdGBF_ReleaseDate = value; this.RaisePropertyChanged(x => x.FdGBF_ReleaseDate); }
        }
        /// <summary>
        /// 搜索关键词(黑话,外号)
        /// </summary>
        public List<string> FsSearch_Nickname
        {
            get => _FsSearch_Nickname;
            set { _FsSearch_Nickname = value; this.RaisePropertyChanged(x => x.FsSearch_Nickname); }
        }
        /// <summary>
        /// 黑话(外号)
        /// </summary>
        public List<string> FsGBF_Nickname
        {
            get => _FsGBF_Nickname;
            set { _FsGBF_Nickname = value; this.RaisePropertyChanged(x => x.FsGBF_Nickname); }
        }
        /// <summary>
        /// 中文名
        /// </summary>
        public string FsName_CHS
        {
            get => _FsName_CHS;
            set { _FsName_CHS = value; this.RaisePropertyChanged(x => x.FsName_CHS); }
        }
        /// <summary>
        /// 英文名
        /// </summary>
        public string FsName_EN
        {
            get => _FsName_EN;
            set { _FsName_EN = value; this.RaisePropertyChanged(x => x.FsName_EN); }
        }
        /// <summary>
        /// 系列
        /// </summary>
        public Weapon.GBFSeriesNameEnum FsSeries_Name
        {
            get => _FsSeries_Name;
            set { _FsSeries_Name = value; this.RaisePropertyChanged(x => x.FsSeries_Name); }
        }
        /// <summary>
        /// 旧版称号
        /// </summary>
        public string FsTitle_JP
        {
            get => _FsTitle_JP;
            set { _FsTitle_JP = value; this.RaisePropertyChanged(x => x.FsTitle_JP); }
        }
        /// <summary>
        /// 日文名
        /// </summary>
        public string FsName_JP
        {
            get => _FsName_JP;
            set { _FsName_JP = value; this.RaisePropertyChanged(x => x.FsName_JP); }
        }

        #endregion
    }
}
