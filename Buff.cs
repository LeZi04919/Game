using PyGame.Prefabs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PyGame
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
    internal class BuffCollection :IEnumerable
    {
        List<Buff> Buffs = new();
        public int Count
        {
            get { return Buffs.Count; }
        }        
        Buff this[int index] 
        {
            get { return Buffs[index]; }
            set { this[index] = value; }
        }
        Buff this[string buffName]
        {
            get 
            {
                if (!Contains(buffName))
                    throw new ArgumentOutOfRangeException($"在{this}中未找到buffname为{buffName}的对象");
                return Buffs.Where((buff) => buff.Name.Equals(buffName)).ToArray()[0];
            }
            set { this[buffName] = value; }
        }
        Buff this[BuffEffect effect]
        {
            get
            {
                if (!this.Contains(effect))
                    throw new ArgumentOutOfRangeException($"在{this}中未找到buffeffect为{effect}的对象");
                return Buffs.Where((buff) => buff.Effect.Equals(effect)).ToArray()[0];
            }
            set { this[effect] = value; }
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
            if(Contains(newBuff.Effect))
            {
                var effect = newBuff.Effect;
                var oldBuff = this[effect];
                oldBuff.Rounds = Math.Max(oldBuff.Rounds, newBuff.Rounds);
                if(newBuff.OverlayType.Equals(Overlay.Add))
                    oldBuff.Value += newBuff.Value;
                else
                    oldBuff.Value *= newBuff.Value;
                this[effect] = oldBuff;
            }
            else
                Buffs.Add(newBuff);
        }
        public void Remove(Buff oldBuff)
        {
            Buffs.Remove(oldBuff);
        }
        public void Remove(BuffEffect effect)
        {
            Buffs.Remove(this[effect]);
        }
        public void Remove(string buffName)
        {
            Buffs.Remove(this[buffName]);
        }
        public void Clear()
        {
            Buffs.Clear();
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
            foreach(var buff in target)
                Add(buff);
        }
        public void ForEach(Action<Buff> action)
        {
            foreach(var buff in Buffs)
                action(buff);
        }
    }
}
