using RoguelikeGame.Interfaces;

namespace RoguelikeGame.Class
{
    internal class Drug : Item, IReleasable
    {
        /// <summary>
        /// 倍数，造成相当于自身攻击力Value倍的伤害；为0时，仅生效Buff；不为0时，造成伤害的同时给目标附加Buff；
        /// Value为正数时，造成伤害；Value为负数时，造成回复效果，回复量为当前血量百分比
        /// </summary>
        public required float Value { get; set; }
        /// <summary>
        /// 表示该药品附带的Buff
        /// </summary>
        public required Buff[] Effect { get; set; }
        /// <summary>
        /// 表示该药品的值基准，可为攻击力或生命上限
        /// </summary>
        public required ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 表示该药品作用范围
        /// </summary>
        public required TargetType Target { get; set; }
        
        public Drug() { }
        //public Drug(string Name, ItemType Type, RarityType Rarity,long Value,ReleaseType ReleaseType,Buff[] Effect) : base(Name, true, Type, Rarity)
        //{
        //    this.Effect = Effect;
        //    this.Value = Value;
        //    this.ReleaseType = ReleaseType;
        //}
    }
}