using System;
using RoguelikeGame.Class;

namespace RoguelikeGame.Prefabs
{
    internal class Prefab
    {
        public float MaxHealth;//最大血量
        public float Health;//目前血量
        public float Armor;//防御力
        public int Level;//等级
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
    }
    internal class Player : Prefab
    {

    }
    internal class Monster : Prefab
    {

    }
}
