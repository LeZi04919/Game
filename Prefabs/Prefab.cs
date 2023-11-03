using System;


namespace PyGame.Prefabs
{
    internal class Prefab
    {
        public float MaxHealth;//最大血量
        public float Health;//目前血量
        public float Armor;//防御力
        public int Level;//等级
        public float Dodge;//闪避
        public Buff[] Buffs = { };
        
    }
}
