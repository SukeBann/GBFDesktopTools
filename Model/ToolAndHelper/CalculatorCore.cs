using System;
using System.Reflection;
using GBFDesktopTools.Model.abstractModel;

namespace GBFDesktopTools.Model.ToolAndHelper
{
    /// <summary>
    /// 计算器核心
    /// </summary>
    public class CalculatorCore : abstractModel.abstractModel
    {   
        /// <summary>
        /// 空武器模型
        /// </summary>
        public static Weapon emptyWeapon = new Weapon();
        /// <summary>
        /// 拖拽武器时的临时对象
        /// </summary>
        public Weapon TempWeapon = new Weapon();

        public CalculatorCore()
        {
            GetEmptyWeapon();
            ResetPannel();
        }

        #region BaseMethod

        /// <summary>
        /// 生成一个空武器模型
        /// </summary>
        public void GetEmptyWeapon()
        {
            var path = Assembly.GetExecutingAssembly().CodeBase;
            path = path.Remove(path.IndexOf(@"bin/Debug/GBFDesktopTools.Model.DLL", StringComparison.Ordinal)) + @"Resources/Image/";

            emptyWeapon = new Weapon(true)
            {
                SkillWarning = true,
                WeaponImgUrl_ls = path + @"VerticalEmpty.jpg",
                WeaponImgUrl_m = path + @"HorizontalEmpty.jpg"
            };

        }

        /// <summary>
        /// 重置模拟器面板
        /// </summary>
        public void ResetPannel()
        {
            MainWeapon = emptyWeapon.Clone() as Weapon;
            OneAndOne = emptyWeapon.Clone() as Weapon;
            OneAndTwo = emptyWeapon.Clone() as Weapon;
            OneAndThree = emptyWeapon.Clone() as Weapon;
            TwoAndOne = emptyWeapon.Clone() as Weapon;
            TwoAndTwo = emptyWeapon.Clone() as Weapon;
            TwoAndThree = emptyWeapon.Clone() as Weapon;
            ThreeAndOne = emptyWeapon.Clone() as Weapon;
            ThreeAndTwo = emptyWeapon.Clone() as Weapon;
            ThreeAndThree = emptyWeapon.Clone() as Weapon;
        }

        #endregion

        #region RunningProperty

        private bool _CopyOrMove;

        /// <summary>
        /// 拖拽时的操作 true为复制 false为移动
        /// </summary>
        public bool CopyOrMove
        {
            get => _CopyOrMove;
            set { _CopyOrMove = value; this.RaisePropertyChanged(x => x.CopyOrMove); }
        }

        #endregion

        #region SourceWeapon

        private Weapon _MainWeapon;

        private Weapon _OneAndOne;
        private Weapon _OneAndTwo;
        private Weapon _OneAndThree;
        private Weapon _TwoAndOne;
        private Weapon _TwoAndTwo;
        private Weapon _TwoAndThree;
        private Weapon _ThreeAndOne;
        private Weapon _ThreeAndTwo;
        private Weapon _ThreeAndThree;

        /// <summary>
        /// 主武器
        /// </summary>
        public Weapon MainWeapon
        {
            get => _MainWeapon;
            set { _MainWeapon = value; this.RaisePropertyChanged(x => x.MainWeapon); }
        }

        /// <summary>
        /// 一行第一列
        /// </summary>
        public Weapon OneAndOne
        {
            get => _OneAndOne;
            set { _OneAndOne = value; this.RaisePropertyChanged(x => x.OneAndOne); }
        }
        /// <summary>
        /// 一行第二列
        /// </summary>
        public Weapon OneAndTwo
        {
            get => _OneAndTwo;
            set { _OneAndTwo = value; this.RaisePropertyChanged(x => x.OneAndTwo); }
        }
        /// <summary>
        /// 一行第三列
        /// </summary>
        public Weapon OneAndThree
        {
            get => _OneAndThree;
            set { _OneAndThree = value; this.RaisePropertyChanged(x => x.OneAndThree); }
        }

        /// <summary>
        /// 二行第一列
        /// </summary>
        public Weapon TwoAndOne
        {
            get => _TwoAndOne;
            set { _TwoAndOne = value; this.RaisePropertyChanged(x => x.TwoAndOne); }
        }
        /// <summary>
        /// 二行第二列
        /// </summary>
        public Weapon TwoAndTwo
        {
            get => _TwoAndTwo;
            set { _TwoAndTwo = value; this.RaisePropertyChanged(x => x.TwoAndTwo); }
        }
        /// <summary>
        /// 二行第三列
        /// </summary>
        public Weapon TwoAndThree
        {
            get => _TwoAndThree;
            set { _TwoAndThree = value; this.RaisePropertyChanged(x => x.TwoAndThree); }
        }


        /// <summary>
        /// 三行第一列
        /// </summary>
        public Weapon ThreeAndOne
        {
            get => _ThreeAndOne;
            set { _ThreeAndOne = value; this.RaisePropertyChanged(x => x.ThreeAndOne); }
        }
        /// <summary>
        /// 三行第二列
        /// </summary>
        public Weapon ThreeAndTwo
        {
            get => _ThreeAndTwo;
            set { _ThreeAndTwo = value; this.RaisePropertyChanged(x => x.ThreeAndTwo); }
        }
        /// <summary>
        /// 三行第三列
        /// </summary>
        public Weapon ThreeAndThree
        {
            get => _ThreeAndThree;
            set { _ThreeAndThree = value; this.RaisePropertyChanged(x => x.ThreeAndThree); }
        }

        #endregion

    }
}