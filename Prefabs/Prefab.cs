using System;
using RoguelikeGame.Class;

namespace RoguelikeGame.Prefabs
{
    internal class Prefab
    {
        public long MaxHealth;//最大血量
        public long Health;//目前血量
        public long Armor;//防御力
        public long Attack;//攻击力
        public long Level;//等级
        public long Experience;//目前经验值
        public long ExpMaxLimit;//下一级
        public PrefabType Type;
        public float Dodge
        { get; set; }//闪避
        public BuffCollection Buffs = new();
        public SkillCollection Skills = new();

        public void ReleaseSkill(Prefab target,Skill skill)
        {
            switch(skill.Type)
            {
                case SkillType.Normal:
                    //选择目标
                    break;
                case SkillType.Buff:
                    target.Buffs.AddRange(skill.Effect);
                    break;
            }
            Skills.ToCoolDown(skill);
        }        
        public bool Upgrade()
        {
            if (Experience >= ExpMaxLimit)
            {
                Level++;
                Experience -= ExpMaxLimit;
                ExpMaxLimit += 5 * (Level - 1);
                MaxHealth *= (long)Math.Pow(1.057, Level - 1);
                Attack *= (long)Math.Pow(1.062,Level - 1);
            }
            return false;
        }
    }
    internal class Player : Prefab
    {

    }
    internal class Monster : Prefab
    {

    }
}
