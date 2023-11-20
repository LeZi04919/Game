using System.Net.NetworkInformation;
using RoguelikeGame.Class;
using RoguelikeGame.Prefabs;
using System;
using System.Linq;
using System.Collections.Generic;

namespace RoguelikeGame
{
    /// <summary>
    /// 表示Buff的效果
    /// </summary>
    internal enum BuffEffect
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
    /// <summary>
    /// 表示Buff的结算方式
    /// </summary>
    internal enum Overlay
    {
        Add,
        Mul
    }
    /// <summary>
    /// 表示Skill采用哪个数值进行结算
    /// </summary>
    internal enum ReleaseType
    {
        Damage,   //以当前Damage结算
        Health    //以HP最大值结算
    }
    /// <summary>
    /// 表示Prefab的类型
    /// </summary>
    internal enum PrefabType
    {
        Player,
        Monster
    }
    /// <summary>
    /// 表示Item的种类
    /// </summary>
    internal enum ItemType
    {
        Common,
        Weapon,
        Armor,
        Drug,
        Currency
    }
    /// <summary>
    /// 表示索敌范围
    /// </summary>
    internal enum TargetType
    {
        All,//对所有目标生效
        Self,//仅对自身生效
        Monster,//对Monster及敌对Player生效
        Player //对自身及友方Player生效
    }
    /// <summary>
    /// 表示Monster的阶级，Event表示该Monster为Event特有
    /// </summary>
    internal enum MonsterType
    {
        Common,
        Elite,
        Boss,
        Special
    }
    /// <summary>
    /// 表示Armor提供的防御类型
    /// </summary>
    internal enum ArmorType
    {
        Physical,
        Dodge
    }
    /// <summary>
    /// 表示Item的稀有度
    /// </summary>
    internal enum RarityType
    {
        Common,
        Rare,
        Epic,
        Legacy
    }
    /// <summary>
    /// 表示Feature的效果
    /// </summary>
    internal enum FeatureType
    {
        IgnoreDodge,
        IgnoreArmor,
        IgnoreDamage
    }
    /// <summary>
    /// 表示区域种类
    /// </summary>
    internal enum AreaType
    {
        /// <summary>
        /// 城市区域
        /// </summary>
        City,
        /// <summary>
        /// 冰原区域
        /// </summary>
        Icefield,
        /// <summary>
        /// 草原区域
        /// </summary>
        Grassland,
        /// <summary>
        /// 平原区域
        /// </summary>
        Plain,
        /// <summary>
        /// 火山区域
        /// </summary>
        Volcano,
        /// <summary>
        /// 沙漠区域
        /// </summary>
        Desert,
        /// <summary>
        /// 全区域
        /// </summary>
        Common
    }
    /// <summary>
    /// 表示Event种类
    /// </summary>
    internal enum EventType
    {
        /// <summary>
        /// 奇遇类Event
        /// </summary>
        Adventure,
        /// <summary>
        /// 商店类Event
        /// </summary>
        Shop,
        /// <summary>
        /// 陷阱类Event
        /// </summary>
        Trap,
        /// <summary>
        /// 会施加负面Buff的Event；如冻伤、灼伤
        /// </summary>
        Status
    }
    internal enum ResultType
    {
        Nothing,
        Event,
        Battle,
        Boss,
        AreaFinish
    }
    internal struct MapArea
    {
        /// <summary>
        /// 表示区域类型
        /// </summary>
        public AreaType Type;
        /// <summary>
        /// 完成该区域所需步数
        /// </summary>
        public int AreaStep;
        /// <summary>
        /// Player在该区域已走步数
        /// </summary>
        public int PlayerStep;
    }
    internal struct Result
    {
        public int Step;
        public ResultType Type;
        public AreaEvent Event;
        public Monster[] Monsters;
    }

    /// <summary>
    /// 用于Weapon和Armor
    /// </summary>
    internal struct Feature
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
    
    /// <summary>
    /// 区域事件结构体
    /// </summary> 
    internal struct AreaEvent
    {
        /// <summary>
        /// 该Event名称
        /// </summary>
        public required string Name;
        /// <summary>
        /// 表示Event的种类
        /// </summary>
        public required EventType Type;
        /// <summary>
        /// 该Event可出现的区域
        /// </summary>
        public required AreaType Area;
        /// <summary>
        /// 该Event特有Monster
        /// </summary>
        public required Monster[] Monsters;
        /// <summary>
        /// 当EventType为Status时，默认以目标生命上限百分比造成伤害
        /// </summary>
        public required double Value;
    }
    internal static class GameResources
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
        internal static readonly AreaEvent[] EventList = new AreaEvent[]
        {
            new AreaEvent()
            {
                Name = "宝箱事件",
                Type = EventType.Adventure,
                Area = AreaType.Grassland,
                Monsters = new Monster[]
                {
                    new Monster()
                    {
                        Name = "宝箱怪",
                        MaxHealth = 15,
                        Armor = 2,
                        Damage = 3,
                        Dodge = 0,
                        Level = 1,
                        Type = PrefabType.Monster,
                        Rank = MonsterType.Special,
                        Skills = new SkillCollection()
                        {
                            new Skill()
                            {
                                Name = "强力撕咬",
                                ReleaseType = ReleaseType.Damage,
                                Target = TargetType.Player,
                                Effect = Array.Empty<Buff>(),
                                Value = 2F,
                                CoolDown = 4
                            },
                            new Skill()
                            {
                                Name = "宝箱盾",
                                ReleaseType = ReleaseType.Damage,
                                Target = TargetType.Self,
                                Effect = new Buff[]
                                {
                                    new Buff()
                                    {
                                        Effect = BuffEffect.ArmorUp,
                                        Rounds = 2,
                                        OverlayType = Overlay.Mul,
                                        Value = 2F
                                    }
                                },
                                Value = 0,
                                CoolDown = 4
                            }
                        }
                    }
                },
                Value = 0
            },
            new AreaEvent()
            {
                Name = "前辈",
                Type = EventType.Adventure,
                Area = AreaType.Common,
                Monsters = Array.Empty<Monster>(),
                Value = 0
            },
            new AreaEvent()
            {
                Name = "阿哈玩偶",
                Type = EventType.Adventure,
                Area = AreaType.Common,
                Monsters = Array.Empty<Monster>(),
                Value = 0
            },
            new AreaEvent()
            {
                Name = "动物聚会",
                Type = EventType.Adventure,
                Area = AreaType.Plain,
                Monsters = Array.Empty<Monster>(),
                Value = 0
            },
            new AreaEvent()
            {
                Name = "坎诺特",
                Type = EventType.Shop,
                Area = AreaType.Common,
                Monsters = Array.Empty<Monster>(),
                Value = 0
            },
            new AreaEvent()
            {
                Name = "史莱姆群",
                Type = EventType.Trap,
                Area = AreaType.Grassland,
                Monsters = Array.Empty<Monster>(),
                Value = 0
            },
            new AreaEvent()
            {
                Name = "灼伤",
                Type = EventType.Status,
                Area = AreaType.Volcano,
                Monsters = Array.Empty<Monster>(),
                Value = 0.05
            },
            new AreaEvent()
            {
                Name = "冻伤",
                Type = EventType.Status,
                Area = AreaType.Volcano,
                Monsters = Array.Empty<Monster>(),
                Value = 0.05
            },
            new AreaEvent()
            {
                Name = "北极熊窝",
                Type = EventType.Trap,
                Area = AreaType.Icefield,
                Monsters = new Monster[]
                {
                    new Monster()
                    {
                        Name = "北极熊",
                        MaxHealth = 20,
                        Armor = 1,
                        Damage = 5,
                        Dodge = 0,
                        Level = 1,
                        Type = PrefabType.Monster,
                        Rank = MonsterType.Special,
                        Skills = new SkillCollection()
                        {
                            new Skill()
                            {
                                Name = "强力击",
                                ReleaseType = ReleaseType.Damage,
                                Target = TargetType.Player,
                                Effect = Array.Empty<Buff>(),
                                Value = 1.5F,
                                CoolDown = 5
                            },
                            new Skill()
                            {
                                Name = "震慑",
                                ReleaseType = ReleaseType.Damage,
                                Target = TargetType.Player,
                                Effect = new Buff[]
                                {
                                    new Buff()
                                    {
                                        Effect = BuffEffect.ArmorDown,
                                        Rounds = 3,
                                        OverlayType = Overlay.Mul,
                                        Value = 0.75F
                                    }
                                },
                                Value = 0,
                                CoolDown = 3
                            },
                            new Skill()
                            {
                                Name = "防护强化",
                                ReleaseType = ReleaseType.Damage,
                                Target = TargetType.Self,
                                Effect = new Buff[]
                                {
                                    new Buff()
                                    {
                                        Effect = BuffEffect.ArmorUp,
                                        Rounds = 3,
                                        OverlayType = Overlay.Mul,
                                        Value = 1.5F
                                    }
                                },
                                Value = 0,
                                CoolDown = 2
                            }
                        }
                    }
                },
                Value = 0
            },
            new AreaEvent()
            {
                Name = "缺水",
                Type = EventType.Status,
                Area = AreaType.Desert,
                Monsters = Array.Empty<Monster>(),
                Value = 0.1
            },
            new AreaEvent()
            {
                Name = "昏厥",
                Type = EventType.Status,
                Area = AreaType.Plain,
                Monsters = Array.Empty<Monster>(),
                Value = 0.1
            },
            new AreaEvent()
            {
                Name = "普通商人",
                Type = EventType.Shop,
                Area = AreaType.City,
                Monsters = Array.Empty<Monster>(),
                Value = 0
            },
            new AreaEvent()
            {
                Name = "卫兵打劫",
                Type = EventType.Trap,
                Area = AreaType.City,
                Monsters = new Monster[]
                {
                    new Monster()
                    {
                        Name = "城市卫兵",
                        MaxHealth = 15,
                        Armor = 3,
                        Damage = 5,
                        Dodge = 0,
                        Level = 1,
                        Type = PrefabType.Monster,
                        Rank = MonsterType.Special,
                        Skills = new SkillCollection()
                        {
                            new Skill()
                            {
                                Name = "强力穿刺",
                                ReleaseType = ReleaseType.Damage,
                                Target = TargetType.Player,
                                Effect = Array.Empty<Buff>(),
                                Value = 1.25F,
                                CoolDown = 3
                            },
                            new Skill()
                            {
                                Name = "架起护盾",
                                ReleaseType = ReleaseType.Damage,
                                Target = TargetType.Self,
                                Effect = new Buff[]
                                {
                                    new Buff()
                                    {
                                        Effect = BuffEffect.ArmorUp,
                                        Rounds = 2,
                                        Value = 2,
                                        OverlayType = Overlay.Mul
                                    }
                                },
                                Value = 0,
                                CoolDown = 3                                
                            }
                        }
                    }
                },
                Value = 0
            }
        };

        internal static readonly Item[] commonItems = ItemList[RarityType.Common];
        internal static readonly Item[] rareItems = ItemList[RarityType.Rare];
        internal static readonly Item[] epicItems = ItemList[RarityType.Epic];
        internal static readonly Item[] legacyItems = ItemList[RarityType.Legacy];
        internal static readonly Item coinItem = ItemList[ItemType.Currency][0];

        //internal static IEnumerable<AreaEvent> CityEvent = EventList.Where(e => e.Area is AreaType.City or AreaType.Common);
        //internal static IEnumerable<AreaEvent> IcefieldEvent = EventList.Where(e => e.Area is AreaType.Icefield or AreaType.Common);
        //internal static IEnumerable<AreaEvent> GrasslandEvent = EventList.Where(e => e.Area is AreaType.Grassland or AreaType.Common);
        //internal static IEnumerable<AreaEvent> PlainEvent = EventList.Where(e => e.Area is AreaType.Plain or AreaType.Common);
        //internal static IEnumerable<AreaEvent> VolcanoEvent = EventList.Where(e => e.Area is AreaType.Volcano or AreaType.Common);
        //internal static IEnumerable<AreaEvent> DesertEvent = EventList.Where(e => e.Area is AreaType.Desert or AreaType.Common);
    }
}
