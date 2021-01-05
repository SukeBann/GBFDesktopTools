using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GBFDesktopTools.Model.abstractModel;

namespace GBFDesktopTools.Model
{
    public class SkillTarget:GBFMessageAbstractModel
    {
        #region 构造方法和枚举类型

        /// <summary>
        /// 技能目标类型 Enum
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

        public SkillTarget(SkillTargetEnum SkillTargetType)
        {
            _FeST_STEnum = SkillTargetType;
        }

        #endregion
        
        #region Method

        private SkillTargetEnum _FeST_STEnum;
        /// <summary>
        /// 技能目标类型枚举
        /// </summary>
        public SkillTargetEnum FeST_STEnum
        {
            get { return _FeST_STEnum; }
        }
        /// <summary>
        /// 获取技能目标字典集
        /// </summary>
        /// <returns></returns>
        public Dictionary<SkillTargetEnum, SkillTarget> GetSkillTarget()
        {
            var dic = new Dictionary<SkillTargetEnum, SkillTarget>();
            foreach (var item in Enum.GetNames(typeof(SkillTargetEnum)))
            {
                var enumtype = (SkillTargetEnum)Enum.Parse(typeof(SkillTargetEnum),item);
                dic.Add(enumtype, new SkillTarget(enumtype));
            }
            return dic;
        }

        #endregion
    }
}
