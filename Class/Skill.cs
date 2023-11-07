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
        public required ReleaseType ReleaseType { get; set; }
        public required TargetType Target { get; set; }
        public required Buff[] Effect { get; set; }
        public required float Value { get; set; }
        public required int CoolDown;//CD轮数
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
                    if (skill.ReleaseType == ReleaseType)
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
