using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using RoguelikeGame.Class;
using RoguelikeGame.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace RoguelikeGame.Prefabs
{
    internal class Prefab
    {
        static Random rd = new();
        public required long MaxHealth;//最大血量
        public long Health;//目前血量
        public required long Armor
        {
            get 
            {
                var _Armor = Armor;
                long bodyProvide = 0;
                if(Body is not null)
                    if (Body.ArmorProvide is not null and ArmorType.Physical)
                        bodyProvide = Body.Value;
                _Armor += bodyProvide;

                List<Buff> buffs = new(Buffs[new BuffEffect[] { BuffEffect.ArmorUp }]);
                buffs.AddRange(Buffs[new BuffEffect[] { BuffEffect.ArmorDown }]);

                foreach (var buff in from buff in buffs where buff.OverlayType == Overlay.Add select buff)
                    _Armor += (long)buff.Value;
                foreach (var deBuff in from buff in buffs where buff.OverlayType == Overlay.Mul select buff)
                    _Armor = (long)(_Armor * deBuff.Value);
                return _Armor; 
            }
            set { Armor = value; }
        }//防御力
        public required long Damage
        {
            get
            {
                var _Damage = Damage;
                if (Hand is not null)
                    _Damage += Hand.Value;

                List<Buff> buffs = new(Buffs[new BuffEffect[] { BuffEffect.DamageUp }]);
                buffs.AddRange(Buffs[new BuffEffect[] { BuffEffect.DamageDown }]);

                foreach (var buff in from buff in buffs where buff.OverlayType == Overlay.Add select buff)
                    _Damage += (long)buff.Value;
                foreach (var deBuff in from buff in buffs where buff.OverlayType == Overlay.Mul select buff)
                    _Damage = (long)(_Damage * deBuff.Value);
                return _Damage;
            } 
            set {  Damage = value; } 
        }//攻击力
        public required float Dodge
        { 
            get
            {
                var _Dodge = Dodge;
                float bodyProvide = 0;
                if(Body is not null)
                    if (Body.ArmorProvide is not null and ArmorType.Dodge)
                        bodyProvide = Body.Value / 100;
                _Dodge += bodyProvide;

                List<Buff> buffs = new(Buffs[new BuffEffect[] { BuffEffect.DamageUp }]);
                buffs.AddRange(Buffs[new BuffEffect[] { BuffEffect.DamageDown }]);

                foreach (var buff in from buff in buffs where buff.OverlayType == Overlay.Add select buff)
                    _Dodge += (long)buff.Value;
                foreach (var deBuff in from buff in buffs where buff.OverlayType == Overlay.Mul select buff)
                    _Dodge = (long)(_Dodge * deBuff.Value);
                return Math.Min(1, _Dodge);
            }
            set { Dodge = value; } }//闪避
        public required long Level;//等级
        public required PrefabType Type;

        protected Wear? Hand;//手部穿戴物
        protected Wear? Body;//身体穿戴物

        protected BuffCollection Buffs = new();
        public required SkillCollection Skills;
        
        public Prefab(long MaxHealth, long Armor, long Damage, float Dodge, long Level, PrefabType Type, SkillCollection Skills)
        {
            this.MaxHealth = MaxHealth;
            this.Health = MaxHealth;
            this.Armor = Armor;
            this.Damage = Damage;
            this.Dodge = Dodge;
            this.Level = Level;
            this.Type = Type;
            this.Skills = Skills;
        }
        public void ReleaseSkill(Prefab target, Skill skill)
        {
            Release(target, skill);
            Skills.ToCoolDown(skill);
        }
        void Release<T>(Prefab target,T released) where T: IReleasable
        {
            switch (released.ReleaseType)
            {
                case ReleaseType.AtOnce:
                    //选择目标
                    break;
                case ReleaseType.Buff:
                    target.Buffs.AddRange(released.Effect);
                    break;
            }
        }
        public virtual bool Upgrade()
        {
            MaxHealth *= (long)Math.Pow(1.057, Level - 1);
            Damage *= (long)Math.Pow(1.062, Level - 1);
            return true;
        }
        public long Attack(Prefab target)
        {
            var targetArmor = target.Armor;
            var targetDodge = target.Dodge;
            if (rd.NextDouble() <= targetDodge)
                return 0;//闪避生效
            var damage = (long)Math.Max(Damage * 0.2, Damage - targetArmor);
            target.Health -= damage;
            return damage;
        }

        
        public async void NextRound()
        {
            await Task.Run(() => 
            {
                Buffs.NextRound();
                Skills.NextRound();
            });
        }
    }
    internal class Player : Prefab
    {
        public required long Experience;//目前经验值
        public required long ExpMaxLimit;//下一级

        public ItemCollection Items = new();
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
    internal class Monster : Prefab
    {
        public MonsterType Rank;
        public Monster(long MaxHealth, long Armor, long Damage, float Dodge, long Level, MonsterType Rank,SkillCollection Skills) : base(MaxHealth, Armor, Damage, Dodge, Level, PrefabType.Monster, Skills)
        {
            this.Rank = Rank;
        }
    }
}
