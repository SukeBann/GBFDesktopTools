using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GBFDesktopTools.Model.abstractModel;

namespace GBFDesktopTools.Model
{
    public class Weapon:GBFMessageAbstractModel
    {
        #region Enum
        /// <summary>
        /// 武器类型枚举
        /// </summary>
        public enum WeaponKind
        {   
            /// <summary>
            /// 全部类型，筛选器词缀
            /// </summary>
            ALL,
            /// <summary>
            /// 无效
            /// </summary>
            Invalid = 0,
            /// <summary>
            /// 剑
            /// </summary>
            Sword = 1,
            /// <summary>
            /// 匕首
            /// </summary>
            Dagger = 2,
            /// <summary>
            /// 长枪
            /// </summary>
            Spear = 3,
            /// <summary>
            /// 斧头
            /// </summary>
            Axe = 4,
            /// <summary>
            /// 杖
            /// </summary>
            MagicWand = 5,
            /// <summary>
            /// 铳
            /// </summary>
            Pistol = 6,
            /// <summary>
            /// 格斗
            /// </summary>
            Fighting = 7,
            /// <summary>
            /// 弓
            /// </summary>
            Bow = 8,
            /// <summary>
            /// 乐器
            /// </summary>
            MusicalInstruments = 9,
            /// <summary>
            /// 刀
            /// </summary>
            Blade = 10,
            /// <summary>
            /// 素材
            /// </summary>
            Material = 99
        }

        /// <summary>
        /// 武器系列
        /// </summary>
        public enum GBFSeriesNameEnum
        {   
            全部系列,
            Unknown,
            强化素材,
            玛格纳方阵系列,
            王权方阵系列,
            腐朽武器,
            英雄武器,
            英雄武器复制品,
            依代武器,
            巴哈姆特武器,
            碧武,
            金月武器,
            石油武器,
            星晶兽系列,
            星晶兽系列_方阵,
            欧米茄系列,
            六道武器,
            六限武器_Limit,
            六限武器_月中,
            六限武器_月底,
            天司武器,
            蔷薇武器_bba,
            四象武器,
            史诗武器_大马,
            塔罗武器_阿卡姆转世,
            虚空神器,
            终末神器,
            宇宙系列_大公,
            星幽武器,
            六龙武器,
            先祖系列,
            天星器,
            十天光辉
        }

        /// <summary>
        /// 筛选器武器突破次数
        /// </summary>
        public enum GFBSearchEvoCounEnum
        {   
            全部,
            三星 = 3,
            四星 = 4,
            四星及以上 = 1,
            五星 = 5
        }

        #endregion

        #region 构造方法

        public Weapon()
        {
            FsSearch_Nickname = new List<string>();
            FsGBF_Nickname = new List<string>();
        }

        #endregion

        #region GeneralInformation

        private long _FnWeapon_ID;
        private long _FnGBF_UnlockChar;
        private bool _FbGBF_IsArchaic;

        private string _WeaponImgUrl_b;
        private string _WeaponImgUrl_cjs;
        private string _WeaponImgUrl_ls;
        private string _WeaponImgUrl_m;
        private string _WeaponImgUrl_s;

        /// <summary>
        /// 是否过时
        /// </summary>
        public bool FbGBF_IsArchaic
        {
            get { return _FbGBF_IsArchaic; }
            set { _FbGBF_IsArchaic = value; this.RaisePropertyChanged(x => x.FbGBF_IsArchaic); }
        }
        /// <summary>
        /// 解锁的角色ID
        /// </summary>
        public long FnGBF_UnlockChar
        {
            get { return _FnGBF_UnlockChar; }
            set { _FnGBF_UnlockChar = value; this.RaisePropertyChanged(x => x.FnGBF_UnlockChar); }
        }
        /// <summary>
        /// 武器ID
        /// </summary>
        public long FnWeapon_ID
        {
            get { return _FnWeapon_ID; }
            set { _FnWeapon_ID = value; this.RaisePropertyChanged(x => x.FnWeapon_ID); }
        }
        /// <summary>
        /// 武器图片 无框斜图
        /// </summary>
        public string WeaponImgUrl_b
        {
            get { return _WeaponImgUrl_b; }
            set { _WeaponImgUrl_b = value; this.RaisePropertyChanged(x => x.WeaponImgUrl_b); }
        }
        /// <summary>
        /// 武器图片 游戏内造型
        /// </summary>
        public string WeaponImgUrl_cjs
        {
            get { return _WeaponImgUrl_cjs; }
            set { _WeaponImgUrl_cjs = value; this.RaisePropertyChanged(x => x.WeaponImgUrl_cjs); }
        }
        /// <summary>
        /// 武器图片 有框竖图
        /// </summary>
        public string WeaponImgUrl_ls
        {
            get { return _WeaponImgUrl_ls; }
            set { _WeaponImgUrl_ls = value; this.RaisePropertyChanged(x => x.WeaponImgUrl_ls); }
        }
        /// <summary>
        /// 武器图片 有框横图
        /// </summary>
        public string WeaponImgUrl_m
        {
            get
            {
                if (_WeaponImgUrl_m == "")
                {
                    var path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                    path = path.Remove(path.IndexOf(@"GBFDesktopTools.Model.DLL"));
                    path += @"Resources\Image\Weapon\ErrorImage (2).png";
                    return path;
                } 
                return _WeaponImgUrl_m;
            }
            set { _WeaponImgUrl_m = value; this.RaisePropertyChanged(x => x.WeaponImgUrl_m); }
        }
        /// <summary>
        /// 武器图片 有框正方形
        /// </summary>
        public string WeaponImgUrl_s
        {
            get { return _WeaponImgUrl_s; }
            set { _WeaponImgUrl_s = value; this.RaisePropertyChanged(x => x.WeaponImgUrl_s); }
        }
        
        #endregion

        #region SpecialInformation

        private string _FsWeapon_SkillName;
        private long _FnWeapon_SkillID;

        private int _FnWeapon_MaxHp;
        private int _FnWeapon_MaxAttack;
        private int _FnWeapon_EvoFourHp;
        private int _FnWeapon_EvoFourAttack;
        private int _FnWeapon_EvoFiveHp;
        private int _FnWeapon_EvoFiveAttack;
        private WeaponKind _FeWeapon_Kind;
        public List<WeaponSkill> WeaponSkill = new List<WeaponSkill>();


        /// <summary>
        /// 获取技能名称
        /// </summary>
        public string GetSkillName
        {
            get 
            {
                string SkillName = "";
                foreach (var item in WeaponSkill)
                {
                    SkillName += item.Main_Description + item.Extra_Comment + ",";
                }
                return SkillName == "" ? "无" : SkillName.Remove(SkillName.Length - 1) ; 
            }
        }
        /// <summary>
        /// 获取一个可用的名称
        /// </summary>
        public String GetCanUseName
        {
            get 
            {
                return FsName_CHS != "" ? FsName_CHS : FsName_EN != "" ? FsName_EN : FsName_JP;
            }
        }
        /// <summary>
        /// 技能警告 技能无数据 无法计算等
        /// </summary>
        public bool SkillWarning { get; set; }
        /// <summary>
        /// 武器种类
        /// </summary>
        public WeaponKind FeWeapon_Kind
        {
            get { return _FeWeapon_Kind; }
            set { _FeWeapon_Kind = value; this.RaisePropertyChanged(x => x.FeWeapon_Kind); }
        }
        /// <summary>
        /// 技能ID
        /// </summary>
        public long FnWeapon_SkillID
        {
            get { return _FnWeapon_SkillID; }
            set { _FnWeapon_SkillID = value; this.RaisePropertyChanged(x => x.FnWeapon_SkillID); }
        }
        /// <summary>
        /// 技能
        /// </summary>
        public string FsWeapon_SkillName
        {
            get { return _FsWeapon_SkillName; }
            set { _FsWeapon_SkillName = value; this.RaisePropertyChanged(x => x.FsWeapon_SkillName); }
        }
        /// <summary>
        /// 5突攻击力
        /// </summary>
        public int FnWeapon_EvoFiveAttack
        {
            get { return _FnWeapon_EvoFiveAttack; }
            set { _FnWeapon_EvoFiveAttack = value; this.RaisePropertyChanged(x => x.FnWeapon_EvoFiveAttack); }
        }
        /// <summary>
        /// 5突生命值
        /// </summary>
        public int FnWeapon_EvoFiveHp
        {
            get { return _FnWeapon_EvoFiveHp; }
            set { _FnWeapon_EvoFiveHp = value; this.RaisePropertyChanged(x => x.FnWeapon_EvoFiveHp); }
        }
        /// <summary>
        /// 攻击力
        /// </summary>
        public int FnWeapon_EvoFourAttack
        {
            get { return _FnWeapon_EvoFourAttack; }
            set { _FnWeapon_EvoFourAttack = value; this.RaisePropertyChanged(x => x.FnWeapon_EvoFourAttack); }
        }
        /// <summary>
        /// 4突生命值
        /// </summary>
        public int FnWeapon_EvoFourHp
        {
            get { return _FnWeapon_EvoFourHp; }
            set { _FnWeapon_EvoFourHp = value; this.RaisePropertyChanged(x => x.FnWeapon_EvoFourHp); }
        }
        /// <summary>
        /// 100级攻击
        /// </summary>
        public int FnWeapon_MaxAttack
        {
            get { return _FnWeapon_MaxAttack; }
            set { _FnWeapon_MaxAttack = value; this.RaisePropertyChanged(x => x.FnWeapon_MaxAttack); }
        }
        /// <summary>
        /// 100级生命值
        /// </summary>
        public int FnWeapon_MaxHp
        {
            get { return _FnWeapon_MaxHp; }
            set { _FnWeapon_MaxHp = value; this.RaisePropertyChanged(x => x.FnWeapon_MaxHp); }
        }
        #endregion

        #region Method

        /// <summary>
        /// 设置武器系列
        /// </summary>
        /// <param name="FC">获取包含武器系列的中日文对照词典</param>
        /// <param name="TargetStr">武器系列日文字符串</param>
        public void SetGBFSeriesName(ToolAndHelper.FilterCondition FC,string TargetStr)
        {
            if (TargetStr == null || TargetStr == "")
            {
		        this.FsSeries_Name = GBFSeriesNameEnum.Unknown;
                return;
            }
            else
            {
                if (this.FsGBF_Tag == "月中" || this.FsGBF_Tag == "月底")
                {
                    this.FsSeries_Name = this.FsGBF_Tag == "月中" ? Weapon.GBFSeriesNameEnum.六限武器_月中 : GBFSeriesNameEnum.六限武器_月底;
                    return;
                }
                this.FsSeries_Name = FC.WeaponJPSeriesNameDic[TargetStr];
            }
        }

        #endregion
    }
}
