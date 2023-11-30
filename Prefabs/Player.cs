using System;
using System.Collections.Generic;
using System.Text.Json;
using RoguelikeGame.Class;
using static RoguelikeGame.Game;

namespace RoguelikeGame.Prefabs
{
    internal class Player : Prefab
    {
        /// <summary>
        /// Player默认一级数据为:
        /// MaxHealth = 20
        /// Damage = 5
        /// Armor = 2
        /// ExpMaxLimit = 25
        /// </summary>
        public required long Experience;//目前经验值
        public required long ExpMaxLimit;//下一级

        /// <summary>
        /// 返回Player持有的Coin数
        /// </summary>
        public long CoinCount => this.Items["通用货币"].Length > 0 ? this.Items["通用货币"][0].Count : 0;

        public ItemCollection Items = new();
        public Player()
        {
            Type = PrefabType.Player;
        }
        public override bool Upgrade(long newLevel)
        {
            var oldLevel = Level;
            MaxHealth = (long)(MaxHealth * Math.Pow(1.055, newLevel - oldLevel));
            Damage = (long)(Damage * Math.Pow(1.06, newLevel - oldLevel));
            Armor = (long)(Armor * Math.Pow(1.06, newLevel - oldLevel));
            return true;
        }
        public bool Upgrade()
        {
            if (Experience >= ExpMaxLimit)
            {
                Upgrade(++Level);
                Experience -= ExpMaxLimit;
                ExpMaxLimit += 5 * (Level - 1);
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
        public Item? Dress(Item wear)
        {
            switch (wear.Type)
            {
                case ItemType.Weapon:
                    if (Hand is not null )
                    {
                        var _Hand = Hand;
                        Hand = wear as Weapon;
                        if(!Items.Add(_Hand))
                            return _Hand;
                    }
                    else
                        Hand = wear as Weapon;
                    break;
                case ItemType.Armor:
                    if (Body is not null)
                    {
                        var _Body = Body;
                        Body = wear as Armor;
                        if (!Items.Add(_Body))
                            return _Body;
                    }
                    else
                        Body = wear as Armor;
                    break;
            }
            return null;
        }
        /// <summary>
        /// 卸下装备
        /// </summary>
        /// <param name="wearType"></param>
        /// <returns></returns>
        public Item? UnDress(ItemType wearType)
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
        public static string Serialize(Player player)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            ///Player主要属性序列化
            string serializeStr = $"{GetBase64Str(player.Name)};"; 
            serializeStr += $"{GetBase64Str(player.Level)};";
            serializeStr += $"{GetBase64Str(player._maxHealth)};";
            serializeStr += $"{GetBase64Str(player._health)};";
            serializeStr += $"{GetBase64Str(player._armor)};";
            serializeStr += $"{GetBase64Str(player._damage)};";
            serializeStr += $"{GetBase64Str(player._dodge)};";
            serializeStr += $"{GetBase64Str(player.Experience)};";
            serializeStr += $"{GetBase64Str(player.ExpMaxLimit)};";
            ///穿戴部分序列化
            serializeStr += $"{GetBase64Str(Item.Serialize(player.Hand))};";
            serializeStr += $"{GetBase64Str(Item.Serialize(player.Body))};";
            ///ItemCollection序列化
            serializeStr += $"{GetBase64Str(player.Items.Serialize())};";
            ///SkillCollection序列化
            serializeStr += $"{GetBase64Str(player.Skills.Serialize())}";

            return GetBase64Str(serializeStr);
        }
        public static Player Deserialize(string serializeStr)
        {
            var deserializeArray = Base64ToStr(serializeStr).Split(";");
            ItemCollection items = new();
            SkillCollection skills = new();

            string name = Base64ToStr(deserializeArray[0]);
            long level = long.Parse(Base64ToStr(deserializeArray[1]));
            long maxHealth = long.Parse(Base64ToStr(deserializeArray[2]));
            long health = long.Parse(Base64ToStr(deserializeArray[3]));
            long armor = long.Parse(Base64ToStr(deserializeArray[4]));
            long damage = long.Parse(Base64ToStr(deserializeArray[5]));
            float dodge = float.Parse(Base64ToStr(deserializeArray[6]));
            long experience = long.Parse(Base64ToStr(deserializeArray[7]));
            long expMaxLimit = long.Parse(Base64ToStr(deserializeArray[8]));
            Weapon? hand = (Weapon?)Item.Deserialize(Base64ToStr(deserializeArray[9]));
            Armor? body = (Armor?)Item.Deserialize(Base64ToStr(deserializeArray[10]));

            items.Deserialize(Base64ToStr(deserializeArray[11]));
            skills.Deserialize(Base64ToStr(deserializeArray[12]));
            return new()
            {
                Type = PrefabType.Player,
                Name = name,
                MaxHealth = maxHealth,
                Health = health,
                Armor = armor,
                Dodge = dodge,
                Damage = damage,
                Level = level,
                Experience = experience,
                ExpMaxLimit = expMaxLimit,
                Items = items,
                Skills = skills,
                Hand = hand,
                Body = body
            };
        }
    }
}