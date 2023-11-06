using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RoguelikeGame;

namespace RoguelikeGame.Class
{
    internal class Skill
    {
        public required string Name;
        public required SkillType Type;
        public required TargetType Target;
        public Buff[] Effect;
        public float Value;
        public required int CoolDown;//CD轮数
        public Skill(string Name, SkillType Type, TargetType Target,Buff[] Effect, float Value, int CoolDown)
        {
            this.Name = Name;
            this.Type = Type;
            this.Target = Target;
            this.Effect = Effect;
            this.Value = Value;
            this.CoolDown = CoolDown;
        }
        public Skill(string Name, SkillType Type, TargetType Target,Buff[] Effect, int CoolDown) : this(Name, Type,Target, Effect, 0, CoolDown)
        {

        }
        public Skill(string Name, SkillType Type, TargetType Target, float Value, int CoolDown) : this(Name, Type, Target,new Buff[] { }, Value, CoolDown)
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
        public Skill[] this[SkillType Type]
        {
            get
            {
                return Skills.Where(skill =>
                {
                    if (skill.Type == Type)
                        return true;
                    return false;
                }).ToArray();
            }
        }
        public void Add(Skill newSkill)
        {
            Skills.Add(newSkill);
        }
        public bool inCoolDown(Skill skill)
        {
            return CoolDownList.ContainsKey(skill);
        }
        public void ToCoolDown(Skill skill)
        {
            CoolDownList.Add(skill, skill.CoolDown);
        }
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
