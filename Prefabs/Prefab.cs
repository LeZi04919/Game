﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RoguelikeGame.Class;
using RoguelikeGame.Interfaces;

namespace RoguelikeGame.Prefabs
{
    internal class Prefab : IPrefab
    {
        static Random rd = new();
        public required string Name
        { get; set; }
        public required long MaxHealth
        { get; set; }//最大血量
        public long Health
        {
            get
            {
                return Health;
            }
            set
            {
                Health = Math.Min(MaxHealth,value); 
            }
        }//目前血量
        public required long Armor
        {
            get 
            {
                var _Armor = Armor;
                long bodyProvide = 0;
                if(Body is not null)
                    if (Body.ArmorProvide is ArmorType.Physical)
                        bodyProvide = Body.Value;
                _Armor += bodyProvide;

                List<Buff> buffs = new(Buffs[BuffEffect.ArmorUp]);
                buffs.AddRange(Buffs[BuffEffect.ArmorDown]);

                foreach (var buff in from buff in buffs where buff.OverlayType is Overlay.Add select buff)
                    _Armor += (long)buff.Value;
                foreach (var Buff in from buff in buffs where buff.OverlayType is Overlay.Mul select buff)
                    _Armor = (long)(_Armor * Buff.Value);
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

                List<Buff> buffs = new(Buffs[BuffEffect.DamageUp]);
                buffs.AddRange(Buffs[BuffEffect.DamageDown]);

                foreach (var buff in from buff in buffs where buff.OverlayType is Overlay.Add select buff)
                    _Damage += (long)buff.Value;
                foreach (var deBuff in from buff in buffs where buff.OverlayType is Overlay.Mul select buff)
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
                if(Body is not null)
                    if (Body.ArmorProvide is ArmorType.Dodge)
                        _Dodge += Body.Value / 100;

                List<Buff> buffs = new(Buffs[BuffEffect.DodgeUp]);
                buffs.AddRange(Buffs[BuffEffect.DodgeDown]);

                foreach (var buff in from buff in buffs where buff.OverlayType is Overlay.Add select buff)
                    _Dodge += (long)buff.Value;
                foreach (var deBuff in from buff in buffs where buff.OverlayType is Overlay.Mul select buff)
                    _Dodge = (long)(_Dodge * deBuff.Value);
                return Math.Min(0.95F, _Dodge);
            }
            set { Dodge = value; } }//闪避
        public required long Level { get; set; }//等级
        public required PrefabType Type { get; set; }

        protected Weapon? Hand;//手部穿戴物
        protected Armor? Body;//身体穿戴物

        protected BuffCollection Buffs = new();
        public required SkillCollection Skills {  get; set; }
        public Prefab() {  }
        public void ReleaseSkill(Prefab target, Skill skill)
        {
            Release(target, skill);
            Skills.ToCoolDown(skill);
        }
        void Release<T>(Prefab target,T released) where T: IReleasable
        {
            if(released.ReleaseType is ReleaseType.Damage)
                target.Health -= (long)(Damage * released.Value);
            else
                target.Health -= (long)(MaxHealth * released.Value);
            target.Buffs.AddRange(released.Effect);
        }
        /// <summary>
        /// 刷新Prefab等级
        /// </summary>
        /// <returns></returns>
        public virtual bool Upgrade()
        {
            MaxHealth *= (long)Math.Pow(1.057, Level - 1);
            Damage *= (long)Math.Pow(1.062, Level - 1);
            return true;
        }
        public long Attack(Prefab target)
        {
            var FeatureAvailability = GetFeatureAvailability(Hand);
            var targetArmor = target.Armor;
            var targetDodge = target.Dodge;
            if(FeatureAvailability)
            {
                var feature = (Feature)Hand.Feature;
                switch (feature.Type)
                {
                    case FeatureType.IgnoreDodge:
                        targetDodge *= feature.Value; 
                        break;
                }
            }
            if (rd.NextDouble() <= targetDodge)
                return 0;//闪避生效
            var damage = (long)Math.Max(Damage * 0.2, Damage - targetArmor);
            target.Health -= damage;
            return damage;
        }
        bool GetFeatureAvailability(IWearable? item)
        {
            if (item is null )
                return false;
            if(item.Feature is not null)
            {
                if (rd.NextDouble() > ((Feature)item.Feature).Probability)
                    return true;
            }
            return false;

        }
        /// <summary>
        /// 刷新Skill与Buff剩余轮数
        /// </summary>
        public async void NextRound()
        {
            await Task.Run(() => 
            {
                Buffs.NextRound();
                Skills.NextRound();
            });
        }
    }
    
    
    internal class PrefabCollection : IEnumerable 
    {
        List<Prefab> Prefabs = new();

        public PrefabCollection() { }
        public Prefab this[int index]
        {
            get { return Prefabs[index]; }
            set { Prefabs[index] = value; }
        }
        public Monster[] this[MonsterType rank]
        {
            get
            {
                return (from monster in Search<Monster>()
                        where monster.Rank == rank
                        select monster).ToArray();
            }
        }
        public Monster[] this[MonsterType[] ranks]
        {
            get
            {
                List<Monster> results = new();
                foreach(var rank in ranks)
                    results.AddRange(from monster in Search<Monster>()
                                     where monster.Rank == rank
                                     select monster);
                return results.ToArray();
            }
        }
        public void Add(Prefab newPrefab)
        {
            Prefabs.Add(newPrefab);
        }
        public void Remove(Prefab oldPrefab)
        {
            Prefabs.Remove(oldPrefab);
        }
        public Prefab[] GetAllPrefab()
        {
            return Prefabs.ToArray();
        }
        public T[] Search<T>() where T: Prefab
        {
            return Enumerable.Cast<T>(from prefab in Prefabs
                                       where prefab.GetType() == typeof(T)
                                       select prefab).ToArray();            
        }
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)Prefabs).GetEnumerator();
        }
    }
}
