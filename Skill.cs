using System;

namespace PyGame
{
    internal class Skill
    {
        public required string Name;
        public required SkillType Type;
        public Buff[]? Effect;
        public float? Value;
        public required int CoolDown;
        public Skill(string Name,SkillType Type,Buff[]? Effect,float? Value, int CoolDown)
        {
            this.Name = Name;
            this.Type = Type;
            this.Effect = Effect;
            this.Value = Value;
            this.CoolDown = CoolDown;
        }
        public Skill(string Name,SkillType Type, Buff[] Effect, int CoolDown) :this(Name,Type,Effect,null,CoolDown)
        {

        }
        public Skill(string Name,SkillType Type,float Value,int CoolDown) : this(Name, Type, null, Value, CoolDown)
        {

        }
    }
}
