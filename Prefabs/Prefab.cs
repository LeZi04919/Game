using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using RoguelikeGame.Class;
using static System.Net.Mime.MediaTypeNames;

namespace RoguelikeGame.Prefabs
{
    internal class Prefab
    {
        static Random rd = new();
        public long MaxHealth;//最大血量
        public long Health;//目前血量
        public long Armor
        {
            get 
            {
                var _Armor = Armor;
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
        public long Damage
        {
            get
            {
                var _Damage = Damage;
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
        public float Dodge
        { 
            get
            {
                var _Dodge = Dodge;
                List<Buff> buffs = new(Buffs[new BuffEffect[] { BuffEffect.DamageUp }]);
                buffs.AddRange(Buffs[new BuffEffect[] { BuffEffect.DamageDown }]);
                foreach (var buff in from buff in buffs where buff.OverlayType == Overlay.Add select buff)
                    _Dodge += (long)buff.Value;
                foreach (var deBuff in from buff in buffs where buff.OverlayType == Overlay.Mul select buff)
                    _Dodge = (long)(_Dodge * deBuff.Value);
                return _Dodge;
            }
            set { Dodge = value; } }//闪避
        public long Level;//等级
        public PrefabType Type;
        
        public BuffCollection Buffs = new();
        public SkillCollection Skills;
        
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
        public void ReleaseSkill(Prefab target,Skill skill)
        {
            switch(skill.Type)
            {
                case SkillType.Normal:
                    //选择目标
                    break;
                case SkillType.Buff:
                    target.Buffs.AddRange(skill.Effect);
                    break;
            }
            Skills.ToCoolDown(skill);
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
        public long Experience;//目前经验值
        public long ExpMaxLimit;//下一级

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
        public void DisposeItem(int index)
        {
            Items.Remove(index);
        }
    }
    internal class Monster : Prefab
    {
        public Monster(long MaxHealth, long Armor, long Damage, float Dodge, long Level, PrefabType Type, SkillCollection Skills) : base(MaxHealth, Armor, Damage, Dodge, Level, PrefabType.Monster, Skills)
        {

        }
    }
}
