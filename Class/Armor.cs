using RoguelikeGame.Interfaces;
using System;

namespace RoguelikeGame.Class
{
    internal class Armor : Item, IWearable,IUpgradeable
    {
        /// <summary>
        /// Armor�У�Count��ֵ��Ϊ1
        /// </summary>
        public new int Count
        {
            get { return 1; }
        }
        /// <summary>
        /// Armor�У�MaxStackCount��Ϊ1�����ɶѵ���
        /// </summary>
        public new int MaxStackCount
        {
            get { return 1; }            
        }
        /// <summary>
        /// ��Armor�ĵȼ�
        /// </summary>
        public long Level { get; set; }
        /// <summary>
        /// ��ʾ��Armor�ṩ�ķ���ֵ
        /// </summary>
        public required long Value { get; set; }
        /// <summary>
        /// ��ʾ��Armor�ṩ�ķ�������
        /// </summary>
        public required ArmorType ArmorProvide { get; set; }
        /// <summary>
        /// ��ArmorЯ�������ԣ���ΪNull
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
            var oldLevel = Level;
            if (ArmorProvide is ArmorType.Dodge)
                Value = Math.Min(100, GetRandomValue(Value));
            else
                Value = (long)(GetRandomValue(Value) * Math.Pow(GetRandomValue(1.062), newLevel - oldLevel));
            if(Feature is not null)
            {
                var feature = (Feature)Feature;
                feature.Probability = (float)Math.Min(1, GetRandomValue(feature.Probability));
                feature.Value = (float)Math.Min(1, GetRandomValue(feature.Probability));
                Feature = feature;
            }    
            return true;
        }
    }
}