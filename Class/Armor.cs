using RoguelikeGame.Interfaces;

namespace RoguelikeGame.Class
{
    internal class Armor : Item, IWearable
    {
        public new int Count
        {
            get { return 1; }
        }
        public new int MaxStackCount
        {
            get { return 1; }            
        }
        public required ArmorType ArmorProvide { get; set; }
        public required long Value { get; set; }
        public required Feature? Feature { get; set; }
        public Armor() { }
    }
}