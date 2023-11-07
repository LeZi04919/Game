using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RoguelikeGame.Class
{
    internal class Item
    {
        public required string Name;
        public required bool Stackable;
        public required ItemType Type;
        public required RarityType Rarity;
        public int Count = 1;
        public int MaxStackCount = 40;
        public Item() { }
        public Item(string Name, bool Stackable, ItemType Type, RarityType Rarity) => (this.Name, this.Stackable, this.Type,this.Rarity) = (Name, Stackable, Type, Rarity);
        public bool Equals(Item targetItem)
        {
            if(targetItem.Name.Equals(Name) && targetItem.Type.Equals(Type))
                return true;
            return false;
        }
        public bool StackItem(Item targetItem)
        {
            if (!targetItem.Stackable || !targetItem.Equals(this))
                return false;
            var maxAcceptCount = MaxStackCount - Count;
            if (targetItem.Count <= maxAcceptCount)
            {
                Count += targetItem.Count;
                targetItem.Count = 0;
            }
            else
            {
                Count += maxAcceptCount;
                targetItem.Count -= maxAcceptCount;
            }
            return true;
        }
    }
    internal class ItemCollection :IEnumerable
    {
        List<Item> items;
        public ItemCollection():this(15)
        {
            
        }
        public ItemCollection(int capacity)
        {
            items = new(capacity);
        }
        //indexer用于查询，无set
        public Item this[int index]
        {
            get { return items[index]; }
        }
        public Item[] this[ItemType Type]
        {
            get 
            {
                return (from item in items
                       where item.Type == Type
                       select item).ToArray();
            }
        }
        public Item[] this[bool Stackable]
        {
            get
            {
                return (from item in items
                        where item.Stackable == Stackable
                        select item).ToArray();
            }
        }
        public Item[] this[string itemName]
        {
            get
            {
                return (from item in items
                        where item.Name.Equals(itemName)
                        select item).ToArray();
            }
        }
        public Item[] this[RarityType rarity]
        {
            get 
            {
                return (from item in items
                        where item.Rarity == rarity
                        select item).ToArray();
            }
        }
        public bool Add(Item? sItem)
        {
            if (sItem is null)
                return true;
            if (sItem.Stackable)
                items.ForEach(item =>
                {
                    if (item.Equals(sItem))
                        item.StackItem(sItem);
                });
            if (sItem.Count > 0)
                if (items.Count > 15)
                    return false;
                else
                    items.Add(sItem);
            return true;
        }
        public void Remove(int index)
        {
            items.Remove(this[index]);
        }
        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
    internal class Wear : Item
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
        public ArmorType? ArmorProvide;
        public required long Value;
        public Wear() { }
        public Wear(string Name,ItemType Type, RarityType Rarity,ArmorType ArmorProvide,long Value): base(Name, false, Type, Rarity)
        {
            this.ArmorProvide = ArmorProvide;
            this.Value = Value;
        }
    }
}
