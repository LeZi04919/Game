using RoguelikeGame;
using RoguelikeGame.Prefabs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RoguelikeGame.Class
{
    internal class Buff
    {
        public required string Name;
        public required BuffEffect Effect;
        public required int Rounds;
        public required Overlay OverlayType;
        public required Prefab[] EffectiveObjects;
        public required float Value;

        public Buff(string Name, BuffEffect Effect, int Rounds, Overlay OverlayType, Prefab[] EffectiveObjects, float Value)
        {
            this.Name = Name;
            this.Effect = Effect;
            this.OverlayType = OverlayType;
            this.Rounds = Rounds;
            this.Value = Value;
            this.EffectiveObjects = EffectiveObjects;
        }
    }
    internal class BuffCollection : IEnumerable
    {
        static BuffEffect[] NegativeEffect = new BuffEffect[]
        {
            BuffEffect.DamageDown,
            BuffEffect.ArmorDown,
            BuffEffect.DodgeDown,
            BuffEffect.Dizziness,
            BuffEffect.Freeze,
            BuffEffect.Firing,
        };
        List<Buff> Buffs = new();
        public int Count
        {
            get { return Buffs.Count; }
        }
        public Buff this[int index]
        {
            get { return Buffs[index]; }
            set { Buffs[index] = value; }
        }
        public Buff[] this[string buffName]
        {
            get
            {
                if (!Contains(buffName))
                    return new Buff[] { };
                return (from buff in Buffs
                       where buff.Name == buffName
                       select buff).ToArray();
            }
        }
        public Buff[] this[BuffEffect effect]
        {
            get
            {
                if (!Contains(effect))
                    return new Buff[] { };
                return (from buff in Buffs
                        where buff.Effect == effect
                        select buff).ToArray();
            }
        }
        public Buff[] this[BuffEffect[] effects]
        {
            get
            {
                List<Buff> Buffs = new();
                foreach (var effect in effects)
                    Buffs.AddRange(this[effect]);
                return Buffs.ToArray();
            }
        }
        public bool Contains(string buffName)
        {
            foreach (var buff in Buffs)
                if (buff.Name.Equals(buffName))
                    return true;
            return false;
        }
        public bool Contains(BuffEffect effect)
        {
            foreach (var buff in Buffs)
                if (buff.Effect.Equals(effect))
                    return true;
            return false;
        }
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)Buffs).GetEnumerator();
        }
        public void Add(Buff newBuff)
        {
            if (newBuff.Effect == BuffEffect.ClearNegativeBuff)
                Buffs.Clear();
            else
            {
                if (Contains(newBuff.Effect))
                {
                    var effect = newBuff.Effect;
                    var oldBuffs = this[effect];
                    foreach (var oldBuff in oldBuffs)
                    {
                        var index = Buffs.IndexOf(oldBuff);
                        oldBuff.Rounds = Math.Max(oldBuff.Rounds, newBuff.Rounds);
                        if (newBuff.OverlayType == oldBuff.OverlayType)
                        {
                            if (newBuff.OverlayType.Equals(Overlay.Add))
                                oldBuff.Value += newBuff.Value;
                            else
                                oldBuff.Value *= newBuff.Value;
                        }
                        else
                            continue;
                        this[index] = oldBuff;
                    }
                }
                else
                    Buffs.Add(newBuff);
            }
        }
        public void Remove(Buff oldBuff)
        {
            Buffs.Remove(oldBuff);
        }
        public void Remove(BuffEffect effect)
        {
            var buffs = this[effect];
            foreach (var buff in buffs)
                Buffs.Remove(buff);
        }
        public void Remove(string buffName)
        {
            var buffs = this[buffName];
            foreach(var buff in buffs)
                Buffs.Remove(buff);
        }
        public void Clear()
        {
            var negativeBuffs = this[NegativeEffect];
            foreach (var buff in negativeBuffs)
                Remove(buff);
        }
        public void NextRound()
        {
            ForEach(buff =>
            {
                if (--buff.Rounds <= 0)
                    Buffs.Remove(buff);
            });
        }
        public void AddRange(IEnumerable<Buff> target)
        {
            foreach (var buff in target)
                Add(buff);
        }
        public void ForEach(Action<Buff> action)
        {
            foreach (var buff in Buffs)
                action(buff);
        }
    }
}
