using RoguelikeGame.Class;
using RoguelikeGame.Prefabs;

namespace RoguelikeGame
{
    public enum BuffEffect
    {        
        /// <summary>
        /// 指示该Buff会使目标攻击力上升
        /// </summary>
        DamageUp,
        /// <summary>
        /// 指示该Buff会使目标攻击力下降
        /// </summary>
        DamageDown,
        /// <summary>
        /// 指示该Buff会使目标防御力上升
        /// </summary>
        ArmorUp,
        /// <summary>
        /// 指示该Buff会使目标防御力下降
        /// </summary>
        ArmorDown,
        /// <summary>
        /// 指示该Buff会提高目标闪避值
        /// </summary>
        DodgeUp,
        /// <summary>
        /// 指示该Buff会降低目标闪避值
        /// </summary>
        DodgeDown,
        /// <summary>
        /// 指示该Buff会使目标陷入眩晕状态；该状态会使目标当前回合无法行动
        /// </summary>
        Dizziness,
        /// <summary>
        /// 指示该Buff会使目标陷入冻伤状态；根据目标的生命上限造成伤害
        /// </summary>
        Freeze,
        /// <summary>
        /// 指示该Buff会使目标陷入灼烧状态；根据目标的生命上限造成伤害
        /// </summary>
        Firing,
        /// <summary>
        /// 指示该Buff可以使目标攻击多个敌人
        /// </summary>
        AreaAttack,
        /// <summary>
        /// 指示该Buff会清除目标的负面Buff
        /// </summary>
        ClearNegativeBuff,
        /// <summary>
        /// 指示该Buff会使目标生命值持续上升；根据目标生命上限进行回复
        /// </summary>
        HPRecovery,
        /// <summary>
        /// 指示该Buff会使目标技能冷却加速
        /// </summary>
        CoolDownBoost
    }
    enum Overlay
    {
        Add,
        Mul
    }
    public enum ReleaseType
    {
        Damage,   //以当前Damage结算
        Health    //以HP最大值结算
    }
    public enum PrefabType
    {
        Player,
        Monster
    }
    public enum ItemType
    {
        Common,
        Weapon,
        Armor,
        Drug,
        Currency
    }
    public enum TargetType
    {
        All,//对所有目标生效
        Self,//仅对自身生效
        Monster,//对Monster及敌对Player生效
        Player //对自身及友方Player生效
    }
    public enum MonsterType
    {
        Common,
        Elite,
        Boss
    }
    public enum ArmorType
    {
        Physical,
        Dodge
    }
    public enum RarityType
    {
        Common,
        Rare,
        Epic,
        Legacy
    }

    public enum FeatureType
    {
        IgnoreDodge,
        IgnoreArmor,
        IgnoreDamage
    }
    public struct Feature
    {
        /// <summary>
        /// 特性类型
        /// </summary>
        public FeatureType Type;
        /// <summary>
        /// 该特性出现的概率，最大1
        /// </summary>
        public float Probability;
        /// <summary>
        /// 该特性的百分比值
        /// </summary>
        public float Value;
    }
    public static class GameResources
    {
        internal static readonly ItemCollection ItemList = new()
        {
            new Weapon()
            {
                Name = "单手剑",
                Rarity = RarityType.Common,
                Type = ItemType.Weapon,
                Feature = null,
                Stackable = false,
                Value = 3
            },
            new Weapon()
            {
                Name = "因果剑",
                Rarity = RarityType.Epic,
                Type = ItemType.Weapon,
                Feature = new Feature()
                {
                    Type = FeatureType.IgnoreDodge,
                    Probability = 1,
                    Value = 0
                },
                Stackable = false,
                Value = 6
            },
            new Armor()
            {
                Name = "破烂的护甲",
                Rarity = RarityType.Common,
                Type = ItemType.Armor,
                Feature = null,
                ArmorProvide = ArmorType.Physical,
                Stackable = false,
                Value = 2
            },
            new Armor()
            {
                Name = "光滑的护甲",
                Rarity = RarityType.Rare,
                Type = ItemType.Armor,
                Feature = new Feature()
                {
                    Type = FeatureType.IgnoreDodge,
                    Probability = 1,
                    Value = 0
                },
                ArmorProvide = ArmorType.Dodge,
                Stackable = false,
                Value = 20
            },
            new Drug()
            {
                Name = "简陋的HP恢复药",
                Description = "粗制滥造的药剂;只能恢复很少一部分HP",
                Value = -0.01F,
                MaxStackCount = 20,
                Stackable = true,
                Type = ItemType.Drug,
                Rarity = RarityType.Common,
                ReleaseType = ReleaseType.Health,
                Target = TargetType.Player,
                Effect = new Buff[] { },
                Count = 1
            },
            new Drug()
            {
                Name = "初级HP恢复药",
                Description = "相对精造的药剂;可以恢复小一部分HP",
                Value = -0.05F,
                MaxStackCount = 20,
                Stackable = true,
                Type = ItemType.Drug,
                Rarity = RarityType.Common,
                ReleaseType = ReleaseType.Health,
                Target = TargetType.Player,
                Effect = new Buff[] { },
                Count = 1
            },
            new Drug()
            {
                Name = "中级HP恢复药",
                Description = "出自大师之手，比较精造的药剂;可以恢复一部分HP",
                Value = -0.15F,
                MaxStackCount = 20,
                Stackable = true,
                Type = ItemType.Drug,
                Rarity = RarityType.Rare,
                ReleaseType = ReleaseType.Health,
                Target = TargetType.Player,
                Effect = new Buff[] { },
                Count = 1
            },
            new Drug()
            {
                Name = "高级HP恢复药",
                Description = "出自隐士之手，纯度非常高的恢复药剂;可以恢复大部分HP，并在未来3回合持续恢复血量",
                Value = -0.35F,
                MaxStackCount = 20,
                Stackable = true,
                Type = ItemType.Drug,
                Rarity = RarityType.Epic,
                ReleaseType = ReleaseType.Health,
                Target = TargetType.Player,
                Effect = new Buff[]
                {
                    new Buff()
                    {
                        Rounds = 3,
                        Effect = BuffEffect.HPRecovery,
                        OverlayType = Overlay.Add,
                        Value = 0.10F
                    }
                },
                Count = 1
            },
            new Drug()
            {
                Name = "HP恢复药精华",
                Description = "经过萃取提纯后的恢复药剂，无杂质，世间难得;可以恢复绝大部分HP，并在未来7回合持续恢复血量",
                Value = -0.6F,
                MaxStackCount = 20,
                Stackable = true,
                Type = ItemType.Drug,
                Rarity = RarityType.Legacy,
                ReleaseType = ReleaseType.Health,
                Target = TargetType.Player,
                Effect = new Buff[]
                {
                    new Buff()
                    {
                        Rounds = 7,
                        Effect = BuffEffect.HPRecovery,
                        OverlayType = Overlay.Add,
                        Value = 0.25F
                    }
                },
                Count = 1
            },
            new Drug()
            {
                Name = "技能冷却加速剂",
                Description = "一管药剂，闻起来很清甜;可以使所有处于冷却的技能回复速度提高1.5倍，持续2轮",
                Value = 0,
                MaxStackCount = 20,
                Stackable = true,
                Type = ItemType.Drug,
                Rarity = RarityType.Rare,
                ReleaseType = ReleaseType.Health,
                Target = TargetType.Self,
                Effect = new Buff[]
                {
                    new Buff()
                    {
                        Rounds = 2,
                        Effect = BuffEffect.CoolDownBoost,
                        OverlayType = Overlay.Mul,
                        Value = 1.5F
                    }
                },
                Count = 1
            },
            new Item()
            {
                Name = "通用货币",
                Description = "这个大陆上的通用货币，可以与普通商人进行交易",
                Stackable = true,
                MaxStackCount = long.MaxValue,
                Type = ItemType.Currency,
                Rarity = RarityType.Common
            }

        };
        internal static readonly PrefabCollection MonsterList = new()
        {
            new Monster()
            {
                Name = "史莱姆",
                MaxHealth = 7,
                Health = 7,
                Armor = 0,
                Damage = 4,
                Dodge = 0.5F,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Common,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "红史莱姆",
                MaxHealth = 7,
                Health = 7,
                Armor = 0,
                Damage = 4,
                Dodge = 0.5F,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Common,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "蓝史莱姆",
                MaxHealth = 7,
                Health = 7,
                Armor = 0,
                Damage = 4,
                Dodge = 0.5F,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Common,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "哥布林",
                MaxHealth = 10,
                Health = 10,
                Armor = 1,
                Damage = 4,
                Dodge = 0,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Common,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "精英哥布林",
                MaxHealth = 16,
                Health = 16,
                Armor = 3,
                Damage = 8,
                Dodge = 0.05F,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Elite,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "兽人",
                MaxHealth = 14,
                Health = 14,
                Armor = 1,
                Damage = 5,
                Dodge = 0,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Common,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "精英兽人",
                MaxHealth = 20,
                Health = 20,
                Armor = 3,
                Damage = 10,
                Dodge = 0,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Elite,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "史莱姆王",
                MaxHealth = 40,
                Health = 40,
                Armor = 2,
                Damage = 8,
                Dodge = 0.75F,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Boss,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "龙",
                MaxHealth = 50,
                Health = 50,
                Armor = 5,
                Damage = 15,
                Dodge = 0,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Boss,
                Skills = new SkillCollection() { }
            }
        };
    }
}
