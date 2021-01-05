using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GBFDesktopTools.Model.abstractModel;

namespace GBFDesktopTools.Model
{
    public class WeaponSkill : Model.abstractModel.abstractModel
    {
        #region 枚举类型

        /// <summary>
        /// 技能类型 Enum
        /// </summary>
        public enum SkillTypeEnum
        {   
            /// <summary>
            /// 全部技能，用于筛选器的筛选条件
            /// </summary>
            allSkill,
            /// <summary>
            /// 错误技能
            /// </summary>
            errorSkill,
            /// <summary>
            /// 普通攻刃
            /// </summary>
            atk,
            /// <summary>
            /// EX攻刃
            /// </summary>
            atk_a,
            /// <summary>
            /// 方阵攻刃
            /// </summary>
            atk_m,
            /// <summary>
            /// 背水
            /// </summary>
            backwater,
            /// <summary>
            /// EX背水
            /// </summary>
            backwater_a,
            /// <summary>
            /// 方阵背水
            /// </summary>
            backwater_m,
            /// <summary>
            /// 屏障
            /// </summary>
            barrier,
            /// <summary>
            /// 克己
            /// </summary>
            coki,
            /// <summary>
            /// 方阵克己
            /// </summary>
            coki_m,
            /// <summary>
            /// 见切（反击回避）
            /// </summary>
            counter,
            /// <summary>
            /// 方阵见切（反击回避）
            /// </summary>
            counter_m,
            /// <summary>
            /// 二手
            /// </summary>
            da,
            /// <summary>
            /// EX二手
            /// </summary>
            da_a,
            /// <summary>
            /// 必杀
            /// </summary>
            deadly,
            /// <summary>
            /// 方阵必杀
            /// </summary>
            deadly_m,
            /// <summary>
            /// 括目
            /// </summary>
            eye,
            /// <summary>
            /// 方阵括目
            /// </summary>
            eye_m,
            /// <summary>
            /// 神威
            /// </summary>
            god,
            /// <summary>
            /// 方阵神威
            /// </summary>
            god_m,
            /// <summary>
            /// 六道神威
            /// </summary>
            xeno,
            /// <summary>
            /// 恩宠
            /// </summary>
            grace,
            /// <summary>
            /// 坚守
            /// </summary>
            guard,
            /// <summary>
            /// 治愈
            /// </summary>
            heal_limit,
            /// <summary>
            /// 方阵治愈
            /// </summary>
            heal_limit_m,
            /// <summary>
            /// 英杰
            /// </summary>
            hero,
            /// <summary>
            /// 守护
            /// </summary>
            hp,
            /// <summary>
            /// EX守护
            /// </summary>
            hp_a,
            /// <summary>
            /// 方阵守护
            /// </summary>
            hp_m,
            /// <summary>
            /// 转世守护
            /// </summary>
            hp_arcarum,
            /// <summary>
            /// 刃界
            /// </summary>
            jinkai,
            /// <summary>
            /// 刹那
            /// </summary>
            moment,
            /// <summary>
            /// 方阵刹那
            /// </summary>
            moment_m,
            /// <summary>
            /// 无双
            /// </summary>
            musou,
            /// <summary>
            /// 方阵无双
            /// </summary>
            musou_m,
            /// <summary>
            /// 罗刹
            /// </summary>
            nirrti,
            /// <summary>
            /// 方阵罗刹
            /// </summary>
            nirrti_m,
            /// <summary>
            /// 星晶兽
            /// </summary>
            primal,
            /// <summary>
            /// 方阵星晶兽
            /// </summary>
            primal_m,
            /// <summary>
            /// 乱舞
            /// </summary>
            ranbu,
            /// <summary>
            /// 方阵乱舞
            /// </summary>
            ranbu_m,
            /// <summary>
            /// 庇护
            /// </summary>
            resist,
            /// <summary>
            /// 秘奥
            /// </summary>
            sp_atk,
            /// <summary>
            /// 奥义上限
            /// </summary>
            sp_limit,
            /// <summary>
            /// 三手
            /// </summary>
            ta,
            /// <summary>
            /// 方阵三手
            /// </summary>
            ta_m,
            /// <summary>
            /// 技巧
            /// </summary>
            tec,
            /// <summary>
            /// 方阵技巧
            /// </summary>
            tec_m,
            /// <summary>
            /// 转世技巧
            /// </summary>
            tec_arcarum,
            /// <summary>
            /// 暴君
            /// </summary>
            tyrant,
            /// <summary>
            /// 方阵暴君
            /// </summary>
            tyrant_m,
            /// <summary>
            /// EX暴君
            /// </summary>
            tyrant_a,
            /// <summary>
            /// 浑身
            /// </summary>
            whole,
            /// <summary>
            /// 方阵浑身
            /// </summary>
            whole_m,
            /// <summary>
            /// 方阵军神
            /// </summary>
            wargod_m,
            /// <summary>
            /// 方阵意志
            /// </summary>
            will_m,
            /// <summary>
            /// 属性伤害减少
            /// </summary>
            damage_red,
            /// <summary>
            /// 进境
            /// </summary>
            jinjin,
            /// <summary>
            /// 方阵进境
            /// </summary>
            jinjin_m,
            /// <summary>
            /// 素材
            /// </summary>
            material,
            /// <summary>
            /// 职武技能
            /// </summary>
            job_weapon,
            /// <summary>
            /// 空插槽
            /// </summary>
            blank,
            /// <summary>
            /// 巴武
            /// </summary>
            baha_1,
            baha_3,
            baha_4,
            baha_5,
            baha_6,
            baha_7,
            race_atk_1,
            race_atk_2,
            race_atk_3,
            race_atk_4,
            race_hp_1,
            race_hp_2,
            race_hp_3,
            race_hp_4,
            race_da_1,
            race_da_2,
            race_da_3,
            race_da_4,
            /// <summary>
            /// 天司
            /// </summary>
            seraphic,
            /// <summary>
            /// U武
            /// </summary>
            weapon_atk_1,
            weapon_atk_2,
            weapon_atk_3,
            weapon_atk_4,
            weapon_atk_5,
            weapon_atk_6,
            weapon_atk_7,
            weapon_atk_8,
            weapon_atk_9,
            weapon_atk_10,
            /// <summary>
            /// 特殊技能
            /// </summary>
            self_1,
            self_2,
            self_3,
            /// <summary>
            /// 石油武治愈
            /// </summary>
            heal,
            /// <summary>
            /// 特典 攻击
            /// </summary>
            atk_all,
            /// <summary>
            /// 特典 生命值
            /// </summary>
            hp_all,
            /// <summary>
            /// 特典 连续攻击
            /// </summary>
            da_all,
            /// <summary>
            /// 特典 自我主义
            /// </summary>
            rosetta_atk_def,
            /// <summary>
            /// 特典 小巫女的法杖
            /// </summary>
            io_atk_def,
            /// <summary>
            /// 特典技能（德雷泽）
            /// </summary>
            eugen_atk_def,
            /// <summary>
            /// 特典技能（共鸣棒）
            /// </summary>
            diantha_atk_def,
            /// <summary>
            /// 特典技能（胡萝卜剑)
            /// </summary>
            weapon_carrot,
            /// <summary>
            /// 特典技能（幽灵树）
            /// </summary>
            dodge_6,
            /// <summary>
            /// D·碧
            /// </summary>
            tresure_1,
            //四象技能
            numbers_4_1,
            numbers_3_3,
            numbers_2_4,
            numbers_1_2,
            /// <summary>
            /// 火大马杖技能
            /// </summary>
            noroi_1,
            /// <summary>
            /// 大公武技能
            /// </summary>
            cosmos_1,
            cosmos_2,
            cosmos_3,
            cosmos_4,
            cosmos_5,
            cosmos_6,
            cosmos_7,
            cosmos_8,
            cosmos_9,
            cosmos_10
        }
        /// <summary>
        /// 计算方式
        /// </summary>
        public enum FormulaModeEnum
        {   
            /// <summary>
            /// 提升
            /// </summary>
            Up,
            /// <summary>
            /// 降低
            /// </summary>
            Down,
            /// <summary>
            /// 固定提升
            /// </summary>
            fixedUp,
            /// <summary>
            /// 固定降低
            /// </summary>
            fixedDown
        }
        /// <summary>
        /// 持续时间类型
        /// </summary>
        public enum DurationEnum
        {   
            /// <summary>
            /// 无
            /// </summary>
            NoHave,
            /// <summary>
            /// 回合
            /// </summary>
            Turn,
            /// <summary>
            /// 分钟
            /// </summary>
            Minute,
            /// <summary>
            /// 持续到消耗完
            /// </summary>
            RunOut
        }
        /// <summary>
        /// 加护类型
        /// </summary>
        public enum SummonEnum
        {   
            /// <summary>
            /// 普通
            /// </summary>
            Normal,
            /// <summary>
            /// 方阵
            /// </summary>
            FZ,
            /// <summary>
            /// EX
            /// </summary>
            EX
        }
        /// <summary>
        /// 生效条件类型
        /// </summary>
        public enum Condition
        {   
            /// <summary>
            /// 无
            /// </summary>
            NoHave,
            /// <summary>
            /// 战斗开始
            /// </summary>
            FightBegin,
            /// <summary>
            /// 收到伤害
            /// </summary>
            Damaged,
            /// <summary>
            /// 浑身
            /// </summary>
            Whole,
            /// <summary>
            /// 背水
            /// </summary>
            BackWater,
            /// <summary>
            /// 坚守
            /// </summary>
            Guard,
            /// <summary>
            /// 转世
            /// </summary>
            Arcarum,
            /// <summary>
            /// 星晶兽
            /// </summary>
            Primal,
            /// <summary>
            /// 进境
            /// </summary>
            JinJin,
            /// <summary>
            /// Counter
            /// </summary>
            Counter
        }
        /// <summary>
        /// skillTarget
        /// </summary>
        public enum SkillTargetEnum
        {
            /// <summary>
            /// 生命值
            /// </summary>
            HP,
            /// <summary>
            /// 攻击
            /// </summary>
            Attack,
            /// <summary>
            ///  连续攻击X2  
            /// </summary>
            DA,
            /// <summary>
            /// 连续攻击X3  
            /// </summary>
            TA,
            /// <summary>
            /// 防御力
            /// </summary>
            Defense,
            /// <summary>
            ///  伤害减免  
            /// </summary>
            Cut,
            /// <summary>
            /// 屏障
            /// </summary>
            Barrier,
            /// <summary>
            /// 弱体耐性
            /// </summary>
            Resist,
            /// <summary>
            /// 反击伤害（回避）百分比
            /// </summary>
            Counter,
            /// <summary>
            /// 技能伤害
            /// </summary>
            SkillDamage,
            /// <summary>
            /// 奥义连锁伤害
            /// </summary>
            ChainBurstDamage,
            /// <summary>
            /// 奥义伤害
            /// </summary>
            UltimateSkillDamage,
            /// <summary>
            /// 技伤上限  
            /// </summary>
            UpperLimit_Skill,
            /// <summary>
            /// 奥义上限
            /// </summary>
            UpperLimit_UltimateSkill,
            /// <summary>
            /// 生命回复上限
            /// </summary>
            Heal_limit,
            /// <summary>
            /// 奥义连锁伤害上限
            /// </summary>
            UpperLimit_ChainBurst,
            /// <summary>
            /// 普攻上限  
            /// </summary>
            UpperLimit_NormalAttack,
            /// <summary>
            ///  普攻追伤  
            /// </summary>
            AdditionalDamage_NormalAttack,
            /// <summary>
            /// 暴击几率  
            /// </summary>
            CritProbability,
            /// <summary>
            /// 暴击伤害
            /// </summary>
            CritDamage
        }

        #endregion

        #region 构造方法


        #endregion

        #region MainProperty
        /// <summary>
        /// 技能ID
        /// </summary>
        public int Skill_ID { get; set; }
        /// <summary>
        /// 技能前缀
        /// </summary>
        public SkillTypeEnum Main_Name { get; set; }
        /// <summary>
        /// 技能后缀
        /// </summary>
        public string Extra_Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Main_Description { get; set; }
        /// <summary>
        /// 特殊技能描述
        /// </summary>
        public string Extra_Description { get; set; }
        /// <summary>
        /// 主标签
        /// </summary>
        public string Main_Tag { get; set; }
        /// <summary>
        /// 特殊技能标签
        /// </summary>
        public string Extra_Tag { get; set; }
        /// <summary>
        /// 是否参与武器盘计算
        /// </summary>
        public bool IsCalculation { get; set; }
        /// <summary>
        /// 是否特殊计算
        /// </summary>
        public bool IsSpecialCalculation { get; set; }
        /// <summary>
        /// 是否有生效条件
        /// </summary>
        public bool IsSpecial { get; set; }
        /// <summary>
        /// 不参与计算的原因，或特殊计算的方式
        /// </summary>
        public string TheReason { get; set; }
        #endregion

        #region ExtraProperty
        
        /// <summary>
        /// 技能影响的属性
        /// </summary>
        public List<SkillTargetEnum> SkillTargetType = new List<SkillTargetEnum>();
        /// <summary>
        /// 技能计算方式
        /// </summary>
        public List<FormulaModeEnum> FormulamodeType = new List<FormulaModeEnum>();
        /// <summary>
        /// 技能大小标识
        /// </summary>
        public string Extra_Comment { get; set; }
        /// <summary>
        /// 技能持续类型
        /// </summary>
        public DurationEnum DurationType { get; set; }
        /// <summary>
        /// 持续时间 数值
        /// </summary>
        public short DurationValue { get; set; }
        /// <summary>
        /// 加护类型
        /// </summary>
        public SummonEnum SummonType { get; set; }
        /// <summary>
        /// 生效条件
        /// </summary>
        public List<Condition> ConditionType = new List<Condition>();
        /// <summary>
        /// 技能允许的最大值 默认0
        /// </summary>
        public double MaxValue { get; set; }
        /// <summary>
        /// 基础上限或特殊值 默认-1
        /// </summary>
        public double BaseLimit { get; set; }
        /// <summary>
        /// 技能值
        /// </summary>
        public Dictionary<int, List<double>> SkillValue = new Dictionary<int, List<double>>
        {   
            {0,new List<double>()},
            {1,new List<double>()},
            {2,new List<double>()},
            {3,new List<double>()},
            {4,new List<double>()},
            {5,new List<double>()},
            {6,new List<double>()},
            {7,new List<double>()},
            {8,new List<double>()},
            {9,new List<double>()},
            {10,new List<double>()},
            {11,new List<double>()},
            {12,new List<double>()},
            {13,new List<double>()},
            {14,new List<double>()},
            {15,new List<double>()},
            {16,new List<double>()},
            {17,new List<double>()},
            {18,new List<double>()},
            {19,new List<double>()},
            {20,new List<double>()}
        };

        #endregion

        #region Method

        /// <summary>
        /// 设置技能目标
        /// </summary>
        /// <param name="str">要拆分的字符串</param>
        public void SetSkillTarget(string Str)
        {
            Str = Str.Remove(Str.Length - 1, 1).Remove(0, 1);
            var strArray = Str.Split(new char[1] { ',' });
            foreach (var item in strArray)
            {
                this.SkillTargetType.Add((SkillTargetEnum)Enum.Parse(typeof(SkillTargetEnum),item));
            }
        }

        /// <summary>
        /// 设置技能计算方式
        /// </summary>
        /// <param name="str">要拆分的字符串</param>
        public void SetFormulaModeEnum(string Str)
        {
            Str = Str.Remove(Str.Length - 1, 1).Remove(0, 1);
            var strArray = Str.Split(new char[1] { ',' });
            foreach (var item in strArray)
            {
                switch (item)
                {
                    case "+":
                        this.FormulamodeType.Add(FormulaModeEnum.Up);
                        break;
                    case "-":
                        this.FormulamodeType.Add(FormulaModeEnum.Down);
                        break;
                    case "++":
                        this.FormulamodeType.Add(FormulaModeEnum.fixedUp);
                        break;
                    case "--":
                        this.FormulamodeType.Add(FormulaModeEnum.fixedDown);
                        break;
                }
            }
        }

        /// <summary>
        /// 设置技能生效条件
        /// </summary>
        /// <param name="Str">要拆分的字符串</param>
        public void SetConditionType(string Str)
        {
            if (Str == String.Empty)
            {
                this.ConditionType.Add(Condition.NoHave);
                return;
            }
            var strArray = Str.Split(new char[1] { ',' }).ToList();
            foreach (var item in strArray)
            {
                this.ConditionType.Add((Condition)Enum.Parse(typeof(Condition), item));
            }
        }

        /// <summary>
        /// 设置技能值
        /// </summary>
        /// <param name="StrList">要拆分的字符串数组</param>
        public void SetSkillValue(List<string> StrList)
        {
            for (int i = 1; i< 21; i++)
            {
                if (StrList[i] == String.Empty)
                {
                    continue;
                }
                var strArray = StrList[i].Split(new char[1] { '&' }).ToList();
                var DoubleList = new List<double>();
                foreach (var str in strArray)
                {
                    DoubleList.Add(Convert.ToDouble(str) / 100);
                }
                SkillValue[i] = DoubleList;
            }
        }

        /// <summary>
        /// 获取一个以技能类型分组的字典集
        /// </summary>
        /// <param name="str">封装后的对象，包含技能列表</param>
        /// <returns></returns>
        public ObjectResult<WeaponSkill> GetWeaponSkillList(ObjectResult<WeaponSkill> WeaponList)
        {   
            if (WeaponList.hasError) return WeaponList;

            var dic = new Dictionary<string,List<WeaponSkill>>();
            var WpList = WeaponList.ObjList;
            foreach (var skillType in Enum.GetNames(typeof(SkillTypeEnum)))
            {
                dic.Add(skillType,
                        WpList.Where(x => x.Main_Name == (SkillTypeEnum)Enum.Parse(typeof(SkillTypeEnum),skillType)).ToList()
                    );
            }
            WeaponList.ObjStrDic = dic;
            return WeaponList;
        }

        #endregion

    }
}
