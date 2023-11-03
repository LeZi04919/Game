using System;
using Game.Class;

namespace Game.Prefabs
{
    internal class Prefab
    {
        public float MaxHealth;//最大血量
        public float Health;//目前血量
        public float Armor;//防御力
        public int Level;//等级
        public float Dodge
        { get; set; }//闪避
        public BuffCollection Buffs = new();
        public SkillCollection Skills = new();
        
    }
}
