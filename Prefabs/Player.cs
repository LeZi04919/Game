using System;
using RoguelikeGame.Class;

namespace RoguelikeGame.Prefabs
{
    internal class Player : Prefab
    {
        public required long Experience;//目前经验值
        public required long ExpMaxLimit;//下一级

        public ItemCollection Items = new();
        public Player()
        {
            Type = PrefabType.Player;
        }
        public Player(long MaxHealth,long Health, long Armor, long Damage, float Dodge, long Level, SkillCollection Skills):base(MaxHealth,Armor,Damage,Dodge,Level,PrefabType.Player,Skills)
        {
            this.Health = Health;
        }
        public Player(long MaxHealth, long Armor, long Damage, float Dodge, long Level,SkillCollection Skills) : this(MaxHealth,MaxHealth, Armor, Damage, Dodge, Level,Skills)
        {

        }
        public override bool Upgrade()
        {
            if (Experience >= ExpMaxLimit)
            {
                Level++;
                Experience -= ExpMaxLimit;
                ExpMaxLimit += 5 * (Level - 1);
                MaxHealth *= (long)Math.Pow(1.055, Level - 1);
                Damage *= (long)Math.Pow(1.06, Level - 1);
                if (Experience >= ExpMaxLimit)
                    Upgrade();
                return true;
            }
            return false;
        }
        public bool AddExp(long exp)
        {
            Experience += exp;
            return Upgrade();
        }
        public void DisposeItem(int index)//丢弃物品
        {
            Items.Remove(index);
        }
        /// <summary>
        /// 装备武器
        /// </summary>
        /// <param name="wear"></param>
        /// <returns></returns>
        public Wear? Dress(Wear wear)
        {
            switch (wear.Type)
            {
                case ItemType.Weapon:
                    if (Hand is null)
                        Hand = wear;
                    else
                    {
                        if (!Items.Add(Hand))
                            return Hand;
                        else
                            Hand = wear;
                    }
                    break;
                case ItemType.Armor:
                    if (Body is null)
                        Body = wear;
                    else
                    {
                        if (!Items.Add(Body))
                            return Body;
                        else
                            Body = wear;
                    }
                    break;
            }
            return null;
        }
        /// <summary>
        /// 卸下装备
        /// </summary>
        /// <param name="wearType"></param>
        /// <returns></returns>
        public Wear? UnDress(ItemType wearType)
        {
            switch(wearType) 
            {
                case ItemType.Weapon:
                    if (!Items.Add(Hand))
                        return Hand;
                    Hand = null;
                    break;
                case ItemType.Armor:
                    if (!Items.Add(Body))
                        return Body;
                    Body = null;
                    break;
            }
            return null;
        }
    }
}