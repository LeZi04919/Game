using RoguelikeGame.Interfaces;

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
        public bool Upgrade(long newLevel)
        {
            return false;
        }
    }
}