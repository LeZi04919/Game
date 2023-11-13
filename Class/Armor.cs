using RoguelikeGame.Interfaces;
using System;
using System.ComponentModel.Design;

namespace RoguelikeGame.Class
{
    internal class Armor : Item, IWearable,IUpgradeable
    {
        /// <summary>
        /// Armor中，Count的值恒为1
        /// </summary>
        public new int Count
        {
            get { return 1; }
        }
        /// <summary>
        /// Armor中，MaxStackCount恒为1（不可堆叠）
        /// </summary>
        public new int MaxStackCount
        {
            get { return 1; }            
        }
        /// <summary>
        /// 该Armor的等级
        /// </summary>
        public long Level { get; set; }
        /// <summary>
        /// 表示该Armor提供的防御值
        /// </summary>
        public required long Value { get; set; }
        /// <summary>
        /// 表示该Armor提供的防御类型
        /// </summary>
        public required ArmorType ArmorProvide { get; set; }
        /// <summary>
        /// 该Armor携带的特性，可为Null
        /// </summary>
        public required Feature? Feature { get; set; }
        public Armor() 
        {
            this.Stackable = false;
        }
        long GetRandomValue(long input)
        {
            Random rd = new();
            var ratio = rd.Next(80,120) / 100;
            return (long)(input * ratio);
        }
        double GetRandomValue(double input)
        {
            Random rd = new();
            var ratio = rd.Next(80, 120) / 100;
            return input * ratio;
        }
        public bool Upgrade(long newLevel)
        {
            return false;

        }
    }
}