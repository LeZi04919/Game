using RoguelikeGame.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RoguelikeGame.Class
{
    internal class Item
    {
        public required string Name;
        public string Description = "";
        public required bool Stackable;
        public required ItemType Type;
        public required RarityType Rarity;
        public long Count = 1;
        public long MaxStackCount = 40;
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
        /// <summary>
        /// 提供的的值,当Item为护盾，并且提供闪避时，闪避数值需乘100
        /// </summary>
        public required long Value;
        public Wear() { }
        public Wear(string Name,ItemType Type, RarityType Rarity,ArmorType ArmorProvide,long Value): base(Name, false, Type, Rarity)
        {
            this.ArmorProvide = ArmorProvide;
            this.Value = Value;
        }
    }
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
        /// 表示该药品释放类型；分为一次性或对目标施加Buff
        /// </summary>
        public required ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 表示该药品作用范围
        /// </summary>
        public required TargetType Target { get; set; }
        
        public Drug() { }
        public Drug(string Name, ItemType Type, RarityType Rarity,long Value,ReleaseType ReleaseType,Buff[] Effect) : base(Name, true, Type, Rarity)
        {
            this.Effect = Effect;
            this.Value = Value;
            this.ReleaseType = ReleaseType;
        }
    }
}
