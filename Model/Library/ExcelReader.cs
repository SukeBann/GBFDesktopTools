using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable All

namespace GBFDesktopTools.Library
{
    using Model;
    using Model.abstractModel;
    using Model.ToolAndHelper;
    using System.Data;
    using System.IO;

    /// <summary>
    /// 从Excel读取武器和武器技能
    /// </summary>
    public class WeaponAndSkillExcelReader
    {
        public WeaponAndSkillExcelReader()
        {
            Fc = new FilterCondition();
        }

        public bool HasError;
        public string ErrorMsg { get; set; }

        //技能列表
        public ObjectResult<WeaponSkill> SkillList;

        //武器列表
        public ObjectResult<Weapon> WeaponList;

        //筛选器
        public FilterCondition Fc;

        private delegate List<string> SplitStr(string Target, short splitType = 1, string CustomStr = null, string[] CustomArray = null, StringSplitOptions IsHaveEmpty = StringSplitOptions.None);

        private readonly SplitStr SplitString = ToolsAndHelper.SplitString;

        /// <summary>
        /// 武器盘模拟器加载方法
        /// </summary>
        public WeaponAndSkillExcelReader LoadWeaponPanelSimulator()
        {
            var skillObj = LoadFromLocalSkill();
            if (skillObj.hasError)
            {
                HasError = true;
                SkillList = skillObj;
                this.ErrorMsg += SkillList.ErrorMsg;
                WeaponList = new ObjectResult<Weapon>();
                return this;
            }
            SkillList = skillObj;
            var weaponObj = LoadFromLocalWeapon();
            if (weaponObj.hasError)
            {
                HasError = true;
                WeaponList = weaponObj;
                this.ErrorMsg += string.IsNullOrEmpty(this.ErrorMsg) ? WeaponList.ErrorMsg : "And" + WeaponList.ErrorMsg;
            }
            WeaponList = weaponObj;
            return this;
        }

        /// <summary>
        /// 设置武器图片路径(更新图片后调用)
        /// </summary>
        /// <param name="ID">武器ID</param>
        /// <param name="RowIndex">武器在Excel中的行数索引</param>
        /// <param name="Rarity">稀有度</param>
        /// <param name="sheet">导入目标源Excel</param>
        private void FilePath(long ID, int RowIndex, string Rarity, Aspose.Cells.Worksheet sheet)
        {
            DirectoryInfo directory = new DirectoryInfo(@"E:\WeaponPanelSimulator\WeaponPanelSimulator\bin\Debug\Resources\Image\Weapon\" + Rarity + @"\");
            directory.Create();
            //输出稀有度目录下的所有文件与文件夹,如果目标文件夹为空 return
            var files = directory.GetFileSystemInfos().ToList();
            if (files.Count == 0) { return; }
            //如果没有找到目标的资源文件夹则return
            var f = files.FirstOrDefault(x => x.Name.Remove(x.Name.IndexOf("_", StringComparison.Ordinal)) == ID.ToString());
            if (f == null) { return; }

            var pathOfFile = f.FullName;
            var fileName = files.FirstOrDefault(x => x.Name.Remove(x.Name.IndexOf("_", StringComparison.Ordinal)) == ID.ToString())?.Name;
            if (string.IsNullOrEmpty(fileName)) { return; }

            var dirt = new DirectoryInfo(pathOfFile);
            var fileList = dirt.GetFiles().ToList();
            if (fileList.Exists(x => x.Name.Remove(x.Name.IndexOf("_", StringComparison.Ordinal)) == "b"))
            {
                sheet.Cells[RowIndex, 32].PutValue(@"Resources\Image\Weapon\" + Rarity + @"\" + fileName + @"\b_" + ID + ".jpg");
            }
            if (fileList.Exists(x => x.Name.Remove(x.Name.IndexOf("_", StringComparison.Ordinal)) == "cjs"))
            {
                sheet.Cells[RowIndex, 33].PutValue(@"Resources\Image\Weapon\" + Rarity + @"\" + fileName + @"\cjs_" + ID + ".jpg");
            }
            if (fileList.Exists(x => x.Name.Remove(x.Name.IndexOf("_", StringComparison.Ordinal)) == "ls"))
            {
                sheet.Cells[RowIndex, 34].PutValue(@"Resources\Image\Weapon\" + Rarity + @"\" + fileName + @"\ls_" + ID + ".jpg");
            }
            if (fileList.Exists(x => x.Name.Remove(x.Name.IndexOf("_", StringComparison.Ordinal)) == "m"))
            {
                sheet.Cells[RowIndex, 35].PutValue(@"Resources\Image\Weapon\" + Rarity + @"\" + fileName + @"\m_" + ID + ".jpg");
            }
            if (fileList.Exists(x => x.Name.Remove(x.Name.IndexOf("_", StringComparison.Ordinal)) == "s"))
            {
                sheet.Cells[RowIndex, 36].PutValue(@"Resources\Image\Weapon\" + Rarity + @"\" + fileName + @"\s_" + ID + ".jpg");
            }
        }

        /// <summary>
        /// 设置武器技能
        /// </summary>
        /// <param name="SkillStr">Excel中的技能短字符串</param>
        /// <param name="Weapon">被设置技能的武器</param>
        private void SetSkillNew(String SkillStr, Weapon Weapon)
        {
            if (SkillStr == "") return;
            //不确定的技能名单
            var ineptitude = ToolsAndHelper.GetSpecialSkillList();

            try
            {
                //先用英文逗号分割技能字符串 ，提取其中的多个技能
                var SkillArray = SplitString(SkillStr, 2, IsHaveEmpty: StringSplitOptions.RemoveEmptyEntries);
                //遍历技能列表获取技能名称
                foreach (var item in SkillArray)
                {
                    //技能实体
                    WeaponSkill Skill;
                    //技能后缀
                    var SkillSuffix = "";
                    //后缀索引
                    short SkillSuffixIndex;
                    var element = WeaponSkill.GBFElementCHSEnum.未知;

                    //判断是否为特殊技能 如果是则直接设置特殊技能
                    if (ineptitude.Exists(x => x == item))
                    {
                        Skill = SkillList.ObjStrDic[item].FirstOrDefault()?.Clone() as WeaponSkill;
                        //设置是否启用技能警告
                        Weapon.SkillWarning = true;
                        if (Skill == null)
                        {
                            throw new Exception("未知的特殊技能，请添加信息");
                        }
                        Weapon.WeaponSkill.Add(Skill);
                        continue;
                    }

                    var TempStr = SplitString(item, 3);
                    //取出技能主名称
                    var SkillMainName = TempStr.FirstOrDefault();

                    //当索引为1的字符串 不为数字（属性）时 ，寻找技能副名称，且索引为2的字符串为元素
                    if (!ToolsAndHelper.StringContentType(TempStr[1], 2))
                    {
                        var ExtraNameResult = ToolsAndHelper.FoundSkillExtraName(SkillMainName, TempStr[1], SkillList.ObjStrDic);

                        //如果返回值为空则说明字典集中不存在副名称为SkillExtraName的技能
                        if (ExtraNameResult == "")
                        {
                            Skill = SkillList.ObjStrDic["errorSkill"].FirstOrDefault();
                            Skill = Copier<WeaponSkill, WeaponSkill>.Copy(Skill);
                            Weapon.WeaponSkill.Add(Skill);
                            Weapon.SkillWarning = true;
                            continue;
                        }
                        //将返回值(主名称+副名称) 赋值给主名称
                        SkillMainName = ExtraNameResult;
                        //技能后缀的索引
                        SkillSuffixIndex = 3;
                    }
                    else
                    {
                        //否则索引为1的字符串为元素
                        //赋值元素，设置技能后缀索引
                        element = (WeaponSkill.GBFElementCHSEnum)Enum.Parse(
                            typeof(WeaponSkill.GBFElementCHSEnum), TempStr[1]);
                        SkillSuffixIndex = 2;
                    }
                    //赋值后缀
                    if (TempStr.Count >= 1 + SkillSuffixIndex)
                    {
                        SkillSuffix = TempStr[SkillSuffixIndex];
                        if (TempStr.Count > 1 + SkillSuffixIndex)
                        {
                            SkillSuffix += TempStr[1 + SkillSuffixIndex];
                        }
                        else
                        {
                            if (element == WeaponSkill.GBFElementCHSEnum.未知)
                            {
                                element = (WeaponSkill.GBFElementCHSEnum)Enum.Parse(
                                    typeof(WeaponSkill.GBFElementCHSEnum), TempStr[SkillSuffixIndex]);
                            }
                        }
                    }
                    //验证后缀
                    SkillSuffix = ToolsAndHelper.FoundSkillSuffix(SkillMainName, SkillSuffix, SkillList.ObjStrDic);

                    //如果技能字典集中有此技能
                    if (SkillList.ObjStrDic.Keys.ToList().Exists(x => x == SkillMainName))
                    {
                        //搜索并设置武器的技能
                        // ReSharper disable once AssignNullToNotNullAttribute
                        Skill = SkillList.ObjStrDic[SkillMainName].FirstOrDefault(x => x.Extra_Name == SkillSuffix);
                        Skill = Copier<WeaponSkill, WeaponSkill>.Copy(Skill);
                        if (Skill == null)
                        {
                            Skill = new WeaponSkill()
                            {
                                Main_Name = (WeaponSkill.SkillTypeEnum)Enum.Parse(typeof(WeaponSkill.SkillTypeEnum), SkillMainName),
                                Extra_Name = SkillSuffix,
                                Main_Description = "Excel中未读取到此后缀的技能，请添加",
                                IsCalculation = false,
                                TheReason = "Excel中未读取到此后缀的技能，请添加"
                            };
                            SkillList.ObjStrDic[SkillMainName].Add(Skill);
                            Weapon.SkillWarning = true;
                        }

                        Skill.SkillElement = element;
                        Weapon.WeaponSkill.Add(Skill);
                    }
                    else
                    {
                        throw new Exception("特殊技能 请添加信息");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("设置武器\"" + (string.IsNullOrEmpty(Weapon.FsName_CHS) ? Weapon.FnWeapon_ID.ToString() : Weapon.FsName_CHS) + "\"的武器技能时时发生错误:" + ex.Message);
            }
        }

        /// <summary>
        /// 读取Excel表格并向内存中写入技能数据
        /// </summary>
        /// RowCount，ColumnCount : 读取的行数和列数 维护时请删除Excel内的空白行 以提高性能
        /// <returns></returns>
        private ObjectResult<WeaponSkill> LoadFromLocalSkill()
        {
            var ObjResult = new ObjectResult<WeaponSkill>();
            List<WeaponSkill> skillList = new List<WeaponSkill>();
            try
            {
                var asm = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = asm.GetName().Name + ".Resources.Excel.WeaponSkill.xls";

                Aspose.Cells.Workbook wk;
                using (Stream stream = asm.GetManifestResourceStream(resourceName))
                {
                    wk = new Aspose.Cells.Workbook(stream);
                }
                var sheet = wk.Worksheets["WeaponSkill"];

                //读取的行数和列数 维护时请根据Excel内的行数列数设置  以提高性能
                int RowCount = sheet.Cells.Rows.Count;
                int ColumnCount = 50;

                DataTable dt = sheet.Cells.ExportDataTable(0, 0, RowCount, ColumnCount);
                for (var i = 2; i < dt.Rows.Count; i++)
                {
                    WeaponSkill skill = new WeaponSkill
                    {
                        Skill_ID = Convert.ToInt32(dt.Rows[i][0]),
                        Main_Name = (WeaponSkill.SkillTypeEnum)Enum.Parse(typeof(WeaponSkill.SkillTypeEnum),
                            dt.Rows[i][1].ToString()),
                        Extra_Name = dt.Rows[i][2].ToString() == string.Empty ? "" : dt.Rows[i][2].ToString(),
                        Main_Description = dt.Rows[i][3].ToString() == string.Empty ? "" : dt.Rows[i][3].ToString(),
                        Extra_Description = dt.Rows[i][4].ToString() == string.Empty ? "" : dt.Rows[i][4].ToString(),
                        Main_Tag = dt.Rows[i][5].ToString() == string.Empty ? "" : dt.Rows[i][5].ToString(),
                        Extra_Tag = dt.Rows[i][6].ToString() == string.Empty ? "" : dt.Rows[i][6].ToString(),
                        IsCalculation = dt.Rows[i][7].ToString() == "1",
                        IsSpecialCalculation = dt.Rows[i][8].ToString() == "1",
                        IsSpecial = dt.Rows[i][9].ToString() == "1",
                        TheReason = dt.Rows[i][10].ToString() == string.Empty ? "" : dt.Rows[i][10].ToString(),
                        Extra_Comment = dt.Rows[i][13].ToString() == string.Empty ? "" : dt.Rows[i][13].ToString(),
                        DurationType = dt.Rows[i][14].ToString() == string.Empty
                            ? WeaponSkill.DurationEnum.NoHave
                            : (WeaponSkill.DurationEnum)Enum.Parse(typeof(WeaponSkill.DurationEnum),
                                dt.Rows[i][14].ToString()),
                        DurationValue = dt.Rows[i][15].ToString() == string.Empty
                            ? (short)-1
                            : Convert.ToInt16(dt.Rows[i][15].ToString()),
                        SummonType = dt.Rows[i][16].ToString() == string.Empty
                            ? WeaponSkill.SummonEnum.Normal
                            : (WeaponSkill.SummonEnum)Enum.Parse(typeof(WeaponSkill.SummonEnum),
                                dt.Rows[i][16].ToString())
                    };
                    //设置持续时间类型
                    //设置技能生效条件
                    skill.SetConditionType(dt.Rows[i][17].ToString());
                    skill.MaxValue = dt.Rows[i][18].ToString() == string.Empty ? 0 : Convert.ToDouble(dt.Rows[i][18].ToString());
                    skill.BaseLimit = dt.Rows[i][19].ToString() == string.Empty ? -1.0 : Convert.ToDouble(dt.Rows[i][19].ToString());

                    //设置技能目标
                    if (dt.Rows[i][11].ToString() != string.Empty)
                    {
                        skill.SetSkillTarget(dt.Rows[i][11].ToString());
                    }
                    //设置计算方式
                    if (dt.Rows[i][12].ToString() != string.Empty)
                    {
                        skill.SetFormulaModeEnum(dt.Rows[i][12].ToString());
                    }

                    //设置技能数值
                    var StrList = new List<string>();
                    for (var index = 19; index < 40; index++)
                    {
                        StrList.Add(dt.Rows[i][index].ToString() == string.Empty ? "" : dt.Rows[i][index].ToString());
                    }
                    skill.SetSkillValue(StrList);

                    skillList.Add(skill);
                }
                skillList.Insert(0, new WeaponSkill()
                {
                    Main_Name = WeaponSkill.SkillTypeEnum.allSkill,
                    Main_Description = "全部技能",
                    Skill_ID = -1,
                    TheReason = "该技能仅作为一个筛选技能的词缀存在，无计算，无其他作用",
                    IsCalculation = false
                });
                skillList.Add(new WeaponSkill()
                {
                    Main_Name = WeaponSkill.SkillTypeEnum.errorSkill,
                    Main_Description = "错误技能",
                    Skill_ID = -999,
                    TheReason = "技能字典集中无此技能，请维护技能字典集",
                    IsCalculation = false
                });
                ObjResult.ObjList = skillList;
                ObjResult = new WeaponSkill().GetWeaponSkillList(ObjResult);
            }
            catch (Exception ex)
            {
                return ObjResult.SetError("从Excel读取技能数据失败:" + ex.Message);
            }
            return ObjResult;
        }

        /// <summary>
        /// 从本地Excel加载Weapon数据
        /// </summary>
        /// <returns></returns>
        private ObjectResult<Weapon> LoadFromLocalWeapon(bool IsUpdateImage = false)
        {
            ObjectResult<Weapon> resultObj = new ObjectResult<Weapon>();
            List<Weapon> weaponList = new List<Weapon>();
            try
            {
                var asm = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = asm.GetName().Name + ".Resources.Excel.武器列表.xls";
                Aspose.Cells.Workbook wk;
                using (var stream = asm.GetManifestResourceStream(resourceName))
                {
                    wk = new Aspose.Cells.Workbook(stream);
                }
                var sheet = wk.Worksheets["武器列表"];
                var rowsCount = sheet.Cells.Rows.Count;
                const int columnCount = 80;

                var dt = sheet.Cells.ExportDataTable(0, 0, rowsCount, columnCount);

                for (var i = 2; i < dt.Rows.Count; i++)
                {
                    var path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                    path = path.Remove(path.IndexOf(@"GBFDesktopTools.Model.DLL", StringComparison.Ordinal));
                    var wp = new Weapon
                    {
                        FnWeapon_ID = dt.Rows[i][0].ToString() == string.Empty ? -99 : Convert.ToInt64(dt.Rows[i][0]),
                        FsName_JP = dt.Rows[i][1].ToString() == string.Empty ? "" : dt.Rows[i][1].ToString(),
                        FsTitle_JP = dt.Rows[i][2].ToString() == string.Empty ? "" : dt.Rows[i][2].ToString(),
                        FsName_EN = dt.Rows[i][4].ToString() == string.Empty ? "" : dt.Rows[i][4].ToString(),
                        FsName_CHS = dt.Rows[i][5].ToString() == string.Empty ? "" : dt.Rows[i][5].ToString(),
                        FsGBF_Nickname = dt.Rows[i][7].ToString() == string.Empty
                            ? new List<string>()
                            : SplitString(dt.Rows[i][7].ToString()),
                        FsSearch_Nickname = dt.Rows[i][8].ToString() == string.Empty
                            ? new List<string>()
                            : SplitString(dt.Rows[i][8].ToString()),
                        FeGBF_Element = dt.Rows[i][9].ToString() == string.Empty
                            ? Weapon.GBFElementCHSEnum.无属性
                            : (Weapon.GBFElementCHSEnum)Enum.Parse(typeof(Weapon.GBFElementCHSEnum),
                                dt.Rows[i][9].ToString()),
                        FeWeapon_Kind = dt.Rows[i][10].ToString() == string.Empty
                            ? Weapon.WeaponKind.无效类型
                            : (Weapon.WeaponKind)Enum.Parse(typeof(Weapon.WeaponKind), dt.Rows[i][10].ToString()),
                        FeGBF_Rarity = dt.Rows[i][11].ToString() == string.Empty
                            ? Weapon.GBFRarityEnum.Unknown
                            : (Weapon.GBFRarityEnum)Enum.Parse(typeof(Weapon.GBFRarityEnum),
                                dt.Rows[i][11].ToString()),
                        FsGBF_Category = dt.Rows[i][12].ToString() == string.Empty
                            ? Weapon.GBFCategoryEnum.未知
                            : (Weapon.GBFCategoryEnum)Enum.Parse(typeof(Weapon.GBFCategoryEnum),
                                dt.Rows[i][12].ToString()),
                        FsGBF_Tag = dt.Rows[i][13].ToString() == string.Empty ? "" : dt.Rows[i][13].ToString(),
                        FdGBF_ReleaseDate = dt.Rows[i][14].ToString() == string.Empty
                            ? Convert.ToDateTime(null)
                            : Convert.ToDateTime(dt.Rows[i][14].ToString().Remove(0, 2)),
                        FdGBF_Star4 = dt.Rows[i][15].ToString() == string.Empty
                            ? Convert.ToDateTime(null)
                            : Convert.ToDateTime(dt.Rows[i][15].ToString().Remove(0, 2)),
                        FdGBF_Star5 = dt.Rows[i][16].ToString() == string.Empty
                            ? Convert.ToDateTime(null)
                            : Convert.ToDateTime(dt.Rows[i][16].ToString().Remove(0, 2)),
                        FdGBF_LastDate = dt.Rows[i][17].ToString() == string.Empty
                            ? Convert.ToDateTime(null)
                            : Convert.ToDateTime(dt.Rows[i][17].ToString().Remove(0, 2)),
                        FnGBF_UnlockChar = dt.Rows[i][18].ToString() == string.Empty
                            ? 0
                            : Convert.ToInt64(dt.Rows[i][18]),
                        FsGBF_LinkGamewith = dt.Rows[i][19].ToString() == string.Empty ? "" : dt.Rows[i][19].ToString(),
                        FsGBF_LinkEnwiki = dt.Rows[i][20].ToString() == string.Empty ? "" : dt.Rows[i][20].ToString(),
                        FnGBF_UserLevel = dt.Rows[i][21].ToString() == string.Empty
                            ? (short)0
                            : Convert.ToInt16(dt.Rows[i][21]),
                        FnWeapon_MaxHp =
                            dt.Rows[i][22].ToString() == string.Empty ? 0 : Convert.ToInt32(dt.Rows[i][22]),
                        FnWeapon_MaxAttack = dt.Rows[i][23].ToString() == string.Empty
                            ? 0
                            : Convert.ToInt32(dt.Rows[i][23]),
                        FnWeapon_EvoFourHp = dt.Rows[i][23].ToString() == string.Empty
                            ? 0
                            : Convert.ToInt32(dt.Rows[i][24]),
                        FnWeapon_EvoFourAttack = dt.Rows[i][25].ToString() == string.Empty
                            ? 0
                            : Convert.ToInt32(dt.Rows[i][25]),
                        FnWeapon_EvoFiveHp = dt.Rows[i][26].ToString() == string.Empty
                            ? 0
                            : Convert.ToInt32(dt.Rows[i][26]),
                        FnWeapon_EvoFiveAttack = dt.Rows[i][27].ToString() == string.Empty
                            ? 0
                            : Convert.ToInt32(dt.Rows[i][27]),
                        FnGBF_BaseEvo = dt.Rows[i][28].ToString() == string.Empty
                            ? (short)0
                            : Convert.ToInt16(dt.Rows[i][28]),
                        FnGBF_MaxEvo = dt.Rows[i][29].ToString() == string.Empty
                            ? (short)0
                            : Convert.ToInt16(dt.Rows[i][29]),
                        FbGBF_IsArchaic =
                            dt.Rows[i][30].ToString() != string.Empty && Convert.ToBoolean(dt.Rows[i][30]),
                        FsWeapon_SkillName = dt.Rows[i][31].ToString() == string.Empty ? "" : dt.Rows[i][31].ToString(),
                        WeaponImgUrl_b = dt.Rows[i][32].ToString() == string.Empty
                            ? ""
                            : path + dt.Rows[i][32],
                        WeaponImgUrl_cjs = dt.Rows[i][33].ToString() == string.Empty
                            ? ""
                            : path + dt.Rows[i][33],
                        WeaponImgUrl_ls = dt.Rows[i][34].ToString() == string.Empty
                            ? ""
                            : path + dt.Rows[i][34],
                        WeaponImgUrl_m = dt.Rows[i][35].ToString() == string.Empty
                            ? ""
                            : path + dt.Rows[i][35],
                        WeaponImgUrl_s = dt.Rows[i][36].ToString() == string.Empty
                            ? ""
                            : path + dt.Rows[i][36]
                    };
                    //更新图片路径
                    if (IsUpdateImage)
                    {
                        FilePath(wp.FnWeapon_ID, i, wp.FeGBF_Rarity.ToString(), sheet);
                    }
                    //设置武器系列
                    wp.SetGBFSeriesName(Fc, dt.Rows[i][3].ToString());
                    SetSkillNew(wp.FsWeapon_SkillName, wp);

                    weaponList.Add(wp);
                }
                resultObj.ObjList = weaponList;
            }
            catch (Exception ex)
            {
                return resultObj.SetError("从Excel读取武器数据失败:" + ex.Message);
            }
            return resultObj;
        }
    }
}