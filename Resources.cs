using RoguelikeGame.Class;
using RoguelikeGame.Prefabs;

namespace RoguelikeGame
{
    public enum BuffEffect
    {
        DamageUp,
        DamageDown, 
        ArmorUp,
        ArmorDown,
        DodgeUp,
        DodgeDown,
        Dizziness,
        Freeze,
        Firing,
        AreaAttack,
        ClearNegativeBuff,
        HPRecovery,
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

    public static class GameResources
    {
        static readonly ItemCollection ItemList = new()
        {
            new Weapon()
            {
                Name = "单手剑",
                Rarity = RarityType.Common,
                Type = ItemType.Weapon,
                Stackable = false,
                Value = 3
            },
            new Armor()
            {
                Name = "破烂的护甲",
                Rarity = RarityType.Common,
                Type = ItemType.Armor,
                ArmorProvide = ArmorType.Physical,
                Stackable = false,
                Value = 2
            },
            new Armor()
            {
                Name = "光滑的护甲",
                Rarity = RarityType.Rare,
                Type = ItemType.Armor,
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
                        Value = -0.10F
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
                        Value = -0.25F
                    }
                },
                Count = 1
            },
            new Drug()
            {
                Name = "技能冷却加速剂",
                Description = "一管药剂，闻起来很清甜;可以使所有处于冷却的技能减少1轮的冷却时间",
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
                        Rounds = 1,
                        Effect = BuffEffect.CoolDownBoost,
                        OverlayType = Overlay.Add,
                        Value = 1
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
        static readonly PrefabCollection MonsterList = new()
        {
            new Monster()
            {
                Name = "史莱姆",
                MaxHealth = 5,
                Health = 5,
                Armor = 0,
                Damage = 2,
                Dodge = 0.5F,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Common,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "红史莱姆",
                MaxHealth = 5,
                Health = 5,
                Armor = 0,
                Damage = 2,
                Dodge = 0.5F,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Common,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "蓝史莱姆",
                MaxHealth = 5,
                Health = 5,
                Armor = 0,
                Damage = 2,
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
                Damage = 5,
                Dodge = 0,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Common,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "精英哥布林",
                MaxHealth = 20,
                Health = 20,
                Armor = 3,
                Damage = 7,
                Dodge = 0.05F,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Elite,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "兽人",
                MaxHealth = 20,
                Health = 20,
                Armor = 4,
                Damage = 8,
                Dodge = 0,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Common,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "精英兽人",
                MaxHealth = 30,
                Health = 30,
                Armor = 6,
                Damage = 12,
                Dodge = 0,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Elite,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "史莱姆王",
                MaxHealth = 50,
                Health = 50,
                Armor = 3,
                Damage = 10,
                Dodge = 0.75F,
                Level = 1,
                Type = PrefabType.Monster,
                Rank = MonsterType.Boss,
                Skills = new SkillCollection() { }
            },
            new Monster()
            {
                Name = "龙",
                MaxHealth = 100,
                Health = 100,
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
