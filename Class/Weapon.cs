
namespace RoguelikeGame.Class
{
    internal class Weapon : Item
    {
        public new int Count
        {
            get
            {
                return 1;
            }
        }
        public new int MaxStackCount
        {
            get { return 1; }
        }
        /// <summary>
        /// 提供的的攻击力
        /// </summary>
        public required long Value;
        public Weapon() { }
        public Weapon(string Name,RarityType Rarity,long Value): base(Name, false, ItemType.Weapon, Rarity)
        {
            this.Value = Value;
        }
    }
}