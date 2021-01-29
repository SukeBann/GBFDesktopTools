using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GBFDesktopTools.Model.abstractModel;
using GBFDesktopTools.Model.ToolAndHelper;

// ReSharper disable once CheckNamespace
namespace GBFDesktopTools.Model
{
    public class Weapon : GBFMessageAbstractModel
    {
        #region 构造方法
        public Weapon(bool _isEmpty = false)
        {
            IsEmpty = _isEmpty;
            FsSearch_Nickname = new List<string>();
            FsGBF_Nickname = new List<string>();
        }

        #endregion

        #region Method

        /// <summary>
        /// 设置武器系列
        /// </summary>
        /// <param name="FC">获取包含武器系列的中日文对照词典</param>
        /// <param name="TargetStr">武器系列日文字符串</param>
        public void SetGBFSeriesName(FilterCondition FC, string TargetStr)
        {
            FsSeries_Name = string.IsNullOrEmpty(TargetStr) ? GBFSeriesNameEnum.未知 : FC.WeaponJpSeriesNameDic[TargetStr];
        }

        public Weapon CopySelf()
        {
            var TempWeapon = this.Clone() as Weapon;
            var TempSkillList = this.WeaponSkill.Select(x => x.Clone() as WeaponSkill).ToList();
            TempWeapon.WeaponSkill = TempSkillList;

            return TempWeapon;
        }

        #endregion

        #region Enum

        /// <summary>
        ///     武器类型枚举
        /// </summary>
        public enum WeaponKind
        {
            /// <summary>
            ///     无效
            /// </summary>
            无效类型 = -1,
            /// <summary>
            /// 全部类型，筛选器词缀
            /// </summary>
            全部 = 0,
            /// <summary>
            ///     Sword
            /// </summary>
            剑 = 1,
            /// <summary>
            ///     Dagger
            /// </summary>
            匕首 = 2,
            /// <summary>
            ///     Spear
            /// </summary>
            长枪 = 3,
            /// <summary>
            ///     Axe
            /// </summary>
            斧 = 4,
            /// <summary>
            ///     MagicWand
            /// </summary>
            杖 = 5,
            /// <summary>
            ///     Pistol
            /// </summary>
            铳 = 6,
            /// <summary>
            ///     Fighting
            /// </summary>
            格斗 = 7,
            /// <summary>
            ///     Bow
            /// </summary>
            弓 = 8,
            /// <summary>
            ///     MusicalInstruments
            /// </summary>
            乐器 = 9,
            /// <summary>
            ///     Blade
            /// </summary>
            刀 = 10,
            /// <summary>
            ///     Material
            /// </summary>
            素材 = 99
        }

        /// <summary>
        ///     武器系列
        /// </summary>
        public enum GBFSeriesNameEnum
        {
            未知 = -1,
            全部系列 = 0,
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
        ///     筛选器武器突破次数
        /// </summary>
        public enum GFBSearchEvoCountEnum
        {
            全部 = 0,
            三星 = 3,
            四星 = 4,
            四星以上 = 1,
            五星 = 5
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
        ///     是否过时
        /// </summary>
        public bool FbGBF_IsArchaic
        {
            get => _FbGBF_IsArchaic;
            set
            {
                _FbGBF_IsArchaic = value;
                this.RaisePropertyChanged(x => x.FbGBF_IsArchaic);
            }
        }

        /// <summary>
        ///     解锁的角色ID
        /// </summary>
        public long FnGBF_UnlockChar
        {
            get => _FnGBF_UnlockChar;
            set
            {
                _FnGBF_UnlockChar = value;
                this.RaisePropertyChanged(x => x.FnGBF_UnlockChar);
            }
        }

        /// <summary>
        ///     武器ID
        /// </summary>
        public long FnWeapon_ID
        {
            get => _FnWeapon_ID;
            set
            {
                _FnWeapon_ID = value;
                this.RaisePropertyChanged(x => x.FnWeapon_ID);
            }
        }

        /// <summary>
        ///     武器图片 无框斜图
        /// </summary>
        public string WeaponImgUrl_b
        {
            get => _WeaponImgUrl_b;
            set
            {
                _WeaponImgUrl_b = value;
                this.RaisePropertyChanged(x => x.WeaponImgUrl_b);
            }
        }

        /// <summary>
        ///     武器图片 游戏内造型
        /// </summary>
        public string WeaponImgUrl_cjs
        {
            get => _WeaponImgUrl_cjs;
            set
            {
                _WeaponImgUrl_cjs = value;
                this.RaisePropertyChanged(x => x.WeaponImgUrl_cjs);
            }
        }

        /// <summary>
        ///     武器图片 有框竖图
        /// </summary>
        public string WeaponImgUrl_ls
        {
            get
            {
                if (_WeaponImgUrl_ls != "") return _WeaponImgUrl_ls;
                var path = Assembly.GetExecutingAssembly().CodeBase;
                path = path.Remove(path.IndexOf(@"GBFDesktopTools.Model.DLL", StringComparison.Ordinal));
                path += @"Resources\Image\Weapon\ErrorImage (1).png";
                return path;
            }
            set
            {
                _WeaponImgUrl_ls = value;
                this.RaisePropertyChanged(x => x.WeaponImgUrl_ls);
            }
        }

        /// <summary>
        ///     武器图片 有框横图
        /// </summary>
        public string WeaponImgUrl_m
        {
            get
            {
                if (_WeaponImgUrl_m != "") return _WeaponImgUrl_m;
                var path = Assembly.GetExecutingAssembly().CodeBase;
                path = path.Remove(path.IndexOf(@"GBFDesktopTools.Model.DLL", StringComparison.Ordinal));
                path += @"Resources\Image\Weapon\ErrorImage (2).png";
                return path;
            }
            set
            {
                _WeaponImgUrl_m = value;
                this.RaisePropertyChanged(x => x.WeaponImgUrl_m);
            }
        }

        /// <summary>
        ///     武器图片 有框正方形
        /// </summary>
        public string WeaponImgUrl_s
        {
            get => _WeaponImgUrl_s;
            set
            {
                _WeaponImgUrl_s = value;
                this.RaisePropertyChanged(x => x.WeaponImgUrl_s);
            }
        }

        #endregion

        #region SpecialInformation

        public bool IsEmpty { get;}

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
        /// 获取中文技能名称
        /// </summary>
        public string GetSkillName
        {
            get
            {
                var skillName = WeaponSkill.Aggregate("", (current, item) => current + (item.CHS_Name + ","));
                return skillName == "" ? "无" : skillName.Remove(skillName.Length - 1);
            }
        }

        /// <summary>
        ///     获取一个可用的名称
        /// </summary>
        public string GetCanUseName => FsName_CHS != "" ? FsName_CHS : FsName_JP != "" ? FsName_JP : FsName_EN;

        /// <summary>
        ///     技能警告 技能无数据 无法计算等
        /// </summary>
        public bool SkillWarning { get; set; }

        /// <summary>
        ///     武器种类
        /// </summary>
        public WeaponKind FeWeapon_Kind
        {
            get => _FeWeapon_Kind;
            set
            {
                _FeWeapon_Kind = value;
                this.RaisePropertyChanged(x => x.FeWeapon_Kind);
            }
        }

        /// <summary>
        ///     技能ID
        /// </summary>
        public long FnWeapon_SkillID
        {
            get => _FnWeapon_SkillID;
            set
            {
                _FnWeapon_SkillID = value;
                this.RaisePropertyChanged(x => x.FnWeapon_SkillID);
            }
        }

        /// <summary>
        ///     技能
        /// </summary>
        public string FsWeapon_SkillName
        {
            get => _FsWeapon_SkillName;
            set
            {
                _FsWeapon_SkillName = value;
                this.RaisePropertyChanged(x => x.FsWeapon_SkillName);
            }
        }

        /// <summary>
        ///     5突攻击力
        /// </summary>
        public int FnWeapon_EvoFiveAttack
        {
            get => _FnWeapon_EvoFiveAttack;
            set
            {
                _FnWeapon_EvoFiveAttack = value;
                this.RaisePropertyChanged(x => x.FnWeapon_EvoFiveAttack);
            }
        }

        /// <summary>
        ///     5突生命值
        /// </summary>
        public int FnWeapon_EvoFiveHp
        {
            get => _FnWeapon_EvoFiveHp;
            set
            {
                _FnWeapon_EvoFiveHp = value;
                this.RaisePropertyChanged(x => x.FnWeapon_EvoFiveHp);
            }
        }

        /// <summary>
        ///   4突攻击力
        /// </summary>
        public int FnWeapon_EvoFourAttack
        {
            get => _FnWeapon_EvoFourAttack;
            set
            {
                _FnWeapon_EvoFourAttack = value;
                this.RaisePropertyChanged(x => x.FnWeapon_EvoFourAttack);
            }
        }

        /// <summary>
        ///     4突生命值
        /// </summary>
        public int FnWeapon_EvoFourHp
        {
            get => _FnWeapon_EvoFourHp;
            set
            {
                _FnWeapon_EvoFourHp = value;
                this.RaisePropertyChanged(x => x.FnWeapon_EvoFourHp);
            }
        }

        /// <summary>
        ///     100级攻击
        /// </summary>
        public int FnWeapon_MaxAttack
        {
            get => _FnWeapon_MaxAttack;
            set
            {
                _FnWeapon_MaxAttack = value;
                this.RaisePropertyChanged(x => x.FnWeapon_MaxAttack);
            }
        }

        /// <summary>
        ///     100级生命值
        /// </summary>
        public int FnWeapon_MaxHp
        {
            get => _FnWeapon_MaxHp;
            set
            {
                _FnWeapon_MaxHp = value;
                this.RaisePropertyChanged(x => x.FnWeapon_MaxHp);
            }
        }

        #endregion

        #region CalculatorMethod

        /// <summary>
        /// 设置默认等级数据
        /// </summary>
        public void SetDefaultsValue()
        {
            NowLevel = MaxLevel;
            NowSkillLevel = MaxLevel;
            ExtraLevel = 0;
        }

        #endregion

        #region CalculatorProperty

        /// <summary>
        /// 最大等级
        /// </summary>
        public int MaxLevel => FnWeapon_EvoFiveAttack != 0 ? 200 : FnWeapon_EvoFourAttack != 0 ? 150 :100;
        /// <summary>
        /// 最大技能等级
        /// </summary>
        public int MaxSkillLevel => FnWeapon_EvoFiveAttack != 0 ? 20 : FnWeapon_EvoFourAttack != 0 ? 15 : 10;

        private int _NowLevel;
        private int _NowSkillLevel;
        private int _ExtraLevel;

        /// <summary>
        /// 加蛋等级
        /// </summary>
        public int ExtraLevel
        {
            get => _ExtraLevel;
            set { _ExtraLevel = value; this.RaisePropertyChanged(x => x.ExtraLevel); }
        }

        /// <summary>
        /// 当前生命值
        /// </summary>
        public int NowHP
        {
            get
            {
                var ExtraHP = ExtraLevel * 1;
                var _HP = NowLevel == 200 ? FnWeapon_EvoFiveHp : NowLevel == 150 ? FnWeapon_EvoFourHp : FnWeapon_MaxHp;
                return _HP + ExtraHP;
            }
        }

        /// <summary>
        /// 当前攻击力
        /// </summary>
        public int NowAttack
        {
            get
            {
                var ExtraATT = ExtraLevel * 5;
                var _ATT = NowLevel == 200 ? FnWeapon_EvoFiveAttack : NowLevel == 150 ? FnWeapon_EvoFourAttack : FnWeapon_MaxAttack;
                return _ATT + ExtraATT;
            }
        }

        /// <summary>
        /// 当前技能等级
        /// </summary>
        public int NowSkillLevel
        {
            get => _NowSkillLevel;
            set { _NowSkillLevel = value; this.RaisePropertyChanged(x => x.NowSkillLevel); }
        }

        /// <summary>
        /// 当前等级
        /// </summary>
        public int NowLevel
        {
            get => _NowLevel;
            set { _NowLevel = value; this.RaisePropertyChanged(x => x.NowLevel); }
        }

        /// <summary>
        /// 技能一
        /// </summary>
        public string SkillOne
        {
            get
            {
                if (WeaponSkill != null && WeaponSkill.Count >= 1)
                {
                    return WeaponSkill[0].CHS_Name;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 技能一描述
        /// </summary>
        public string SkillOneDescriptionOne
        {
            get
            {
                if (WeaponSkill != null && WeaponSkill.Count >= 1)
                {
                    return FeGBF_Element + WeaponSkill[0].CHS_DetailedDescription;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 技能二
        /// </summary>
        public string SkillTwo
        {
            get
            {
                if (WeaponSkill != null && WeaponSkill.Count >= 2)
                {
                    return WeaponSkill[1].CHS_Name;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 技能二描述
        /// </summary>
        public string SkillOneDescriptionTwo
        {
            get
            {
                if (WeaponSkill != null && WeaponSkill.Count >= 2)
                {
                    return FeGBF_Element + WeaponSkill[1].CHS_DetailedDescription;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 技能三
        /// </summary>
        public string SkillThree
        {
            get
            {
                if (WeaponSkill != null && WeaponSkill.Count >= 3)
                {
                    return WeaponSkill[2].CHS_Name;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 技能三描述
        /// </summary>
        public string SkillOneDescriptionThree
        {
            get
            {
                if (WeaponSkill != null && WeaponSkill.Count >= 3)
                {
                    return FeGBF_Element + WeaponSkill[2].CHS_DetailedDescription;
                }
                else
                {
                    return "";
                }
            }
        }

        #endregion
    }
}