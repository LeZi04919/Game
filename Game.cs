using RoguelikeGame.Class;
using RoguelikeGame.Prefabs;
using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    internal static partial class Game
    {
        public static Player Player = new()
        {
            Name = "",
            MaxHealth = 20,
            Armor = 2,
            Damage = 5,
            Dodge = 0,
            Level = 1,
            Type = PrefabType.Player,
            Skills = new SkillCollection() { },
            Experience = 0,
            ExpMaxLimit = 25,
        };
        static void Main(string[] args)
        {
            var result = CreateMonsters(MonsterType.Boss);
            Console.WriteLine("Hello World");
            Console.ReadKey();
        }
        /// <summary>
        /// 战斗UI
        /// </summary>
        /// <param name="monsters"></param>
        public static void BattleUI(IList<Monster> monsters)
        {

        }
        /// <summary>
        /// 普通商店的对话UI
        /// </summary>
        /// <param name="shopItems"></param>
        /// <param name="itemPrice"></param>
        public static void ShopUI(List<Item> shopItems, Dictionary<Item, int> itemPrice)
        {
            Clear();
            WriteLine("                     商店");
            Console.WriteLine("     {0,-2} {1,-15} {2,-3} {3,-5}", "序号", "名称", "稀有度", "价格");
            foreach (var keyValuePair in itemPrice)
            {
                var item = keyValuePair.Key;
                var price = keyValuePair.Value;
                Console.WriteLine("     [{0,-2}] {1,-15} {2,-3} {3,-5}$", shopItems.IndexOf(item), item.Name, item.Rarity, price);
            }
            WriteLine("     稀有度一览:  \n0 普通\n1 稀有\n2 史诗\n3 传奇");
            WriteLine($"     您持有的通用货币:{Player.Items["通用货币"][0].Count}");
            WriteLine("     请输入商品序号以购买(非相关输入视为退出):");
            var userInput = Console.ReadLine();
            Clear();
            int index = 0;
            if (int.TryParse(userInput, out index) && index < shopItems.Count)
            {
                List<Item> failure = new();
                var item = shopItems[index];
                var price = itemPrice[item];
                var money = Player.Items["通用货币"][0].Count;
                WriteLine($"     您持有的通用货币:{Player.Items["通用货币"][0].Count}");
                WriteLine($"     您确定要购买{item.Name},价格为{price}吗?\n");
                WriteLine("     Y.是的(我对他一见钟情) N.还是算了(感觉一般般)");
                var inputKey = Console.ReadKey().KeyChar;
                if (inputKey == 'Y')
                {
                    if (money > price)
                    {
                        Player.Items["通用货币"][0].Count -= price;
                        if (!Player.Items.Add(item))
                            failure.Add(item);
                        shopItems.Remove(item);
                        itemPrice.Remove(item);
                        FailAddItemUI(failure);
                        CommonShopUI(shopItems, itemPrice);
                    }
                    else
                    {
                        WriteLine("     您身上的金钱貌似不够呢~请赚够钱后再来吧");
                        Console.ReadLine();
                        CommonShopUI(shopItems, itemPrice);
                    }

                }
                else
                    CommonShopUI(shopItems, itemPrice);
            }
        }
        /// <summary>
        /// 添加物品失败时调用
        /// </summary>
        /// <param name="items"></param>
        public static void FailAddItemUI(IList<Item> items)
        {
            Clear();
            var playerItems = Player.Items;

            WriteLine("以下物品无法添加入背包:");
            Console.WriteLine("     {0,-3} {0,-7}", "名称", "数量");
            foreach (var item in items)
                Console.WriteLine("     {0,-3} {0,-7}", item.Name, item.Count);
            WriteLine("##############################################");
            WriteLine("你背包里面有以下物品:");
            Console.WriteLine("     {0,-2} {0,-3} {0,-7}", "序号", "名称", "数量");
            foreach (Item item in playerItems)
                Console.WriteLine("     [{0,-2}] {0,-3} {0,-7}", playerItems.IndexOf(item), item.Name, item.Count);
            WriteLine("     请输入你想舍弃的物品序号(无关输入将视为退出):");
            int index = 0;
            if (int.TryParse(Console.ReadLine(), out index) && index < playerItems.Count)
            {
                Clear();
                Console.WriteLine("     {0,-2} {0,-3} {0,-7}", "序号", "名称", "数量");
                foreach (Item item in items)
                    Console.WriteLine("     [{0,-2}] {0,-3} {0,-7}", playerItems.IndexOf(item), item.Name, item.Count);
                WriteLine("     请输入你想加入的物品序号(无关输入将视为退出):");
                int failureIndex = 0;
                if (int.TryParse(Console.ReadLine(), out failureIndex) && failureIndex < items.Count)
                {
                    WriteLine($"     欲丢弃的物品:{playerItems[index].Name}");
                    WriteLine($"     欲加入的物品:{items[failureIndex].Name}");
                    WriteLine("     确定吗?");
                    WriteLine("     Y.确定 N.还是算了");
                    if (Console.ReadKey().KeyChar == 'Y')
                    {
                        playerItems.Remove(index);
                        playerItems.Add(items[failureIndex]);
                        items.RemoveAt(failureIndex);
                        Player.Items = playerItems;
                        if (items.Count > 0)
                            FailAddItemUI(items);
                    }
                    else
                        FailAddItemUI(items);
                }

            }

        }
    }
}
