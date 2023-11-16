using RoguelikeGame.Interfaces;
using System;

namespace RoguelikeGame.Class
{
    internal class Weapon : Item, IWearable,IUpgradeable
    {
        /// <summary>
        /// Weapon中，属性Count的值恒为1
        /// </summary>
        public new int Count
        {
            get { return 1; }
        }
        /// <summary>
        /// Weapon中,MaxStackCount的值恒为1（不可堆叠）
        /// </summary>
        public new int MaxStackCount
        {
            get { return 1; }
        }
        /// <summary>
        /// 表示该Weapon的等级
        /// </summary>
        public long Level { get; set; }
        /// <summary>
        /// 表示该Weapon提供的的攻击力
        /// </summary>
        public required long Value { get; set; }
        /// <summary>
        /// 表示该Weapon携带的特性，可为Null
        /// </summary>
        public required Feature? Feature { get; set; }
        public Weapon() 
        {
            this.Stackable = false;
        }
        long GetRandomValue(long input)
        {
            Random rd = new();
            var ratio = rd.Next(80, 121) / 100;
            return (long)(input * ratio);
        }
        double GetRandomValue(double input)
        {
            Random rd = new();
            var ratio = rd.Next(80, 121) / 100;
            return input * ratio;
        }
        public bool Upgrade(long newLevel)
        {
            var oldLevel = Level;
            Value = (long)(GetRandomValue(Value) * Math.Pow(GetRandomValue(1.062), newLevel - oldLevel));
            if (Feature is not null)
            {
                var feature = (Feature)Feature;
                feature.Probability = (float)Math.Min(1, GetRandomValue(feature.Probability));
                feature.Value = (float)Math.Min(1, GetRandomValue(feature.Probability));
                Feature = feature;
            }
            return false;
        }
    }
}