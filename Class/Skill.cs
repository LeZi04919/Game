using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RoguelikeGame;
using RoguelikeGame.Interfaces;

namespace RoguelikeGame.Class
{
    internal class Skill : IReleasable
    {
        public required string Name;
        /// <summary>
        /// 表示该Skill的值基准，可为攻击力或生命上限
        /// </summary>
        public required ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 表示该Skill作用范围
        /// </summary>
        public required TargetType Target { get; set; }
        /// <summary>
        /// 表示该Skill附带的Buff
        /// </summary>
        public required Buff[] Effect { get; set; }
        /// <summary>
        /// 倍数，造成相当于自身攻击力Value倍的伤害；为0时，仅生效Buff；不为0时，造成伤害的同时给目标附加Buff；
        /// Value为正数时，造成伤害；Value为负数时，造成回复效果
        /// </summary>
        public required float Value { get; set; }
        /// <summary>
        /// Skill冷却所需轮数
        /// </summary>
        public required int CoolDown;
        public Skill(string Name, ReleaseType ReleaseType, TargetType Target,Buff[] Effect, float Value, int CoolDown)
        {
            this.Name = Name;
            this.ReleaseType = ReleaseType;
            this.Target = Target;
            this.Effect = Effect;
            this.Value = Value;
            this.CoolDown = CoolDown;
        }
        public Skill(string Name, ReleaseType Type, TargetType Target,Buff[] Effect, int CoolDown) : this(Name, Type,Target, Effect, 0, CoolDown)
        {

        }
        public Skill(string Name, ReleaseType Type, TargetType Target, float Value, int CoolDown) : this(Name, Type, Target,new Buff[] { }, Value, CoolDown)
        {

        }
    }
    internal class SkillCollection : IEnumerable
    {
        List<Skill> Skills = new();
        Dictionary<Skill, int> CoolDownList = new();
        public Skill this[int index]
        {
            get { return Skills[index]; }
        }
        public Skill[] this[ReleaseType ReleaseType]
        {
            get
            {
                return Skills.Where(skill =>
                {
                    if (skill.ReleaseType == ReleaseType)
                        return true;
                    return false;
                }).ToArray();
            }
        }
        /// <summary>
        /// 添加新Skill
        /// </summary>
        /// <param name="newSkill"></param>
        public void Add(Skill newSkill)
        {
            Skills.Add(newSkill);
        }
        /// <summary>
        /// 获取该Skill是否处于CD状态
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool InCoolDown(Skill skill)
        {
            return CoolDownList.ContainsKey(skill);
        }
        /// <summary>
        /// 将目标Skill设置为CD状态
        /// </summary>
        /// <param name="skill"></param>
        public void ToCoolDown(Skill skill)
        {
            CoolDownList.Add(skill, skill.CoolDown);
        }
        /// <summary>
        /// 刷新Skill剩余CD轮数
        /// </summary>
        public void NextRound()
        {
            foreach (var skill in CoolDownList.Keys)
                if (--CoolDownList[skill] <= 0)
                    CoolDownList.Remove(skill);
        }
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)Skills).GetEnumerator();
        }
        public void ForEach(Action<Skill> action)
        {
            Skills.ForEach(action);
        }
    }
}
