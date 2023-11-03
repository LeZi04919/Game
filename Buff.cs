using PyGame.Prefabs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        Buff? this[int index] 
        {
            get { return Buffs[index]; }
        }
        Buff? this[string buffName]
        {
            get 
            {
                if (!Contains(buffName))
                    return null;
                return Buffs.Where((buff) => buff.Name.Equals(buffName)).ToArray()[0];
            }
        }
        Buff? this[BuffEffect effect]
        {
            get
            {
                if (!this.Contains(effect))
                    return null;
                return Buffs.Where((buff) => buff.Effect.Equals(effect)).ToArray()[0];
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
            if(Contains(newBuff.Effect))
            {
                switch(newBuff.OverlayType)
                {
                    case Overlay.Add:
                        break;
                    case Overlay.Mul:
                        break;
                }
            }
        }
    }
}
