using RoguelikeGame.Interfaces;

namespace RoguelikeGame.Class
{
    internal class Weapon : Item, IWearable
    {
        public new int Count
        {
            get { return 1; }
        }
        public new int MaxStackCount
        {
            get { return 1; }
        }
        /// <summary>
        /// 提供的的攻击力
        /// </summary>
        public required long Value { get; set; }
        public required Feature? Feature { get; set; }
        public Weapon() { }
    }
}