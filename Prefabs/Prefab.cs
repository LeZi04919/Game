using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoguelikeGame.Class;
using RoguelikeGame.Interfaces;

namespace RoguelikeGame.Prefabs
{
    internal class Prefab : IPrefab, IUpgradeable
    {
        static Random rd = new();
        long _maxHealth;
        long _health;
        long _armor;
        long _damage;
        float _dodge;
        public static event Action<Prefab, Prefab> PrefabKilledEvent = (source, target) => Console.WriteLine("     {0}已被{1}杀死", target.Name, source.Name);//被杀死对象
        public static event Action<Prefab, Prefab, long> PrefabAttacked;//Source(攻击者)，Target(受击者),Damage(伤害大小)
        public required string Name
        { get; set; }
        public required long MaxHealth
        { 
            get
            {
                return _maxHealth;
            }
            set 
            {
                _maxHealth = value;
                Health = MaxHealth;
            }
        }//最大血量
        public long Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = Math.Min(MaxHealth,value); 
            }
        }//目前血量
        public required long Armor
        {
            get 
            {
                var _Armor = _armor;
                if(Body is not null)
                    if (Body.ArmorProvide is ArmorType.Physical)
                        _Armor += Body.Value;

                List<Buff> buffs = new(Buffs[BuffEffect.ArmorUp]);
                buffs.AddRange(Buffs[BuffEffect.ArmorDown]);

                foreach (var buff in from buff in buffs where buff.OverlayType is Overlay.Add select buff)
                    _Armor += (long)buff.Value;
                foreach (var buff in from buff in buffs where buff.OverlayType is Overlay.Mul select buff)
                    _Armor = (long)(_Armor * buff.Value);
                return _Armor; 
            }
            set { _armor = value; }
        }//防御力
        public required long Damage
        {
            get
            {
                var _Damage = _damage;
                if (Hand is not null)
                    _Damage += Hand.Value;

                List<Buff> buffs = new(Buffs[BuffEffect.DamageUp]);
                buffs.AddRange(Buffs[BuffEffect.DamageDown]);

                foreach (var buff in from buff in buffs where buff.OverlayType is Overlay.Add select buff)
                    _Damage += (long)buff.Value;
                foreach (var buff in from buff in buffs where buff.OverlayType is Overlay.Mul select buff)
                    _Damage = (long)(_Damage * buff.Value);
                return _Damage;
            } 
            set {  _damage = value; } 
        }//攻击力
        public required float Dodge
        { 
            get
            {
                var FeatureAvailability = GetFeatureAvailability(Body);
                if (FeatureAvailability)
                {
                    var feature = (Feature)Body.Feature;
                    switch (feature.Type)
                    {
                        case FeatureType.IgnoreDamage:
                            return 1;
                    }
                }
                var _Dodge = _dodge;
                if(Body is not null)
                    if (Body.ArmorProvide is ArmorType.Dodge)
                        _Dodge += Body.Value / 100;

                List<Buff> buffs = new(Buffs[BuffEffect.DodgeUp]);
                buffs.AddRange(Buffs[BuffEffect.DodgeDown]);

                foreach (var buff in from buff in buffs where buff.OverlayType is Overlay.Add select buff)
                    _Dodge += (long)buff.Value;
                foreach (var buff in from buff in buffs where buff.OverlayType is Overlay.Mul select buff)
                    _Dodge = (long)(_Dodge * buff.Value);
                return Math.Min(0.95F, _Dodge);
            }
            set { _dodge = value; } }//闪避
        public float CoolDownRatio { get; set; }
        public required long Level { get; set; }//等级
        public required PrefabType Type { get; set; }//实体类别

        protected Weapon? Hand;//手部穿戴物
        protected Armor? Body;//身体穿戴物

        protected BuffCollection Buffs = new();
        public required SkillCollection Skills {  get; set; }
        public Prefab() 
        {
            CoolDownRatio = 1.0F;
        }
        public void ReleaseSkill(Prefab target, Skill skill)
        {
            Release(target, skill);
            Skills.ToCoolDown(skill);
        }
        public void Release<T>(Prefab target,T released) where T: IReleasable
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
        public virtual bool Upgrade(long newLevel)
        {
            var oldLevel = Level;
            MaxHealth = (long)(MaxHealth * Math.Pow(1.057, newLevel - oldLevel));
            Damage = (long)(Damage * Math.Pow(1.062, newLevel - oldLevel));
            Armor = (long)(Armor * Math.Pow(1.064, newLevel - oldLevel));
            return true;
        }
        /// <summary>
        /// 普通攻击
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
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
            if (target.Health <= 0)
                PrefabKilledEvent(this, target);
            var damage = (long)Math.Max(Damage * 0.2, Damage - targetArmor);
            target.Health -= damage;
            return damage;
        }
        /// <summary>
        /// 判断是否触发随机特性；触发返回True，传入参数为Null、传入参数的Feature属性为Null及未触发返回False
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool GetFeatureAvailability(IWearable? item)
        {
            if (item is null)
                return false;
            if(item.Feature is not null)
            {
                if (rd.NextDouble() <= ((Feature)item.Feature).Probability)
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
                Buffs.NextRound(this);
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
        public void AddRange(IEnumerable<Prefab> prefabs)
        {
            Prefabs.AddRange(prefabs);
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
        public T[] Search<T>(string prefabName) where T: Prefab
        {
            return Enumerable.Cast<T>(from prefab in Prefabs
                                       where (prefab.GetType() == typeof(T) && prefab.Name.Contains(prefabName))
                                       select prefab).ToArray();            
        }
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)Prefabs).GetEnumerator();
        }
    }
}
