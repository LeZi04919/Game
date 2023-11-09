using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoguelikeGame.Class
{
    internal class Armor : Item
    {
        public new int Count
        {
            get { return 1; }
        }
        public new int MaxStackCount
        {
            get { return 1; }            
        }
        public required ArmorType ArmorType;
        public required long Value;
        public Armor() { }
    }
}