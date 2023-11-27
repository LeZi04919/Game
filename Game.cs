using RoguelikeGame.Class;
using RoguelikeGame.Prefabs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SecurityTool.Security;
using System.Threading;
using System.Xml.Serialization;
using static RoguelikeGame.GameResources;
using System.Text;
using System.Text.Json;

namespace RoguelikeGame
{
    internal static class Archive
    {
        static readonly string Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        static readonly string GamePath = $"{Path}/RoguelikeGame";
        static readonly string ArchivePath = $"{Path}/RoguelikeGame/save";
        static readonly string ChecksumFilePath = $"{Path}/RoguelikeGame/save/checksum.md5";
        public static void Init()
        {
            if(!Directory.Exists(GamePath))
                Directory.CreateDirectory(GamePath);
            if(!Directory.Exists(ArchivePath))
                Directory.CreateDirectory(ArchivePath);
            if (!File.Exists($"{ArchivePath}/Game00.sf"))
                EncryptArchive($"{ArchivePath}/Game00.sf", "");
            if (!File.Exists($"{ArchivePath}/Game01.sf"))
                EncryptArchive($"{ArchivePath}/Game01.sf", "");
            if (!File.Exists($"{ArchivePath}/Game02.sf"))
                EncryptArchive($"{ArchivePath}/Game02.sf", "");
            else
                Load();
        }
        static void Load()
        {
            Game.Player = Player.Deserialize(DecryptArchive($"{ArchivePath}/Game00.sf"));
            Game.TotalStep = JsonSerializer.Deserialize<int>(Game.Base64ToStr(DecryptArchive($"{ArchivePath}/Game01.sf")));
            Game.Area = JsonSerializer.Deserialize<MapArea>(Game.Base64ToStr(DecryptArchive($"{ArchivePath}/Game02.sf")));
        }
        public static void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            EncryptArchive($"{ArchivePath}/Game00.sf",Game.Player.Serialize());
            EncryptArchive($"{ArchivePath}/Game01.sf",Game.GetBase64Str(JsonSerializer.Serialize(Game.TotalStep,options)));
            EncryptArchive($"{ArchivePath}/Game02.sf", Game.GetBase64Str(JsonSerializer.Serialize(Game.Area, options)));
        }
        static void EncryptArchive(string Path,string serializeStr)//加密存档
        {
            //var sw = File.Open($"{ArchivePath}/Game.sf",FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
            var sw = File.Open(Path, FileMode.OpenOrCreate,FileAccess.Write,FileShare.ReadWrite);
            byte[] buffer = Encoding.UTF8.GetBytes(serializeStr);
            //byte[] encryptBuffer = Encrypt(buffer);
            //sw.Write(encryptBuffer, 0, encryptBuffer.Length);
            sw.Write(buffer, 0, buffer.Length);
            sw.Close();
        }
        static string DecryptArchive(string Path)//解密存档
        {
            try
            {
                var sw = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] buffer = new byte[sw.Length];
                while (sw.Read(buffer, 0, buffer.Length) > 0) ;
                //byte[] DecryptBuffer = Decrypt(buffer);
                byte[] DecryptBuffer = buffer;
                sw.Close();
                return Encoding.UTF8.GetString(DecryptBuffer);
            }
            catch
            {
                Game.WriteLine($"[ERROR]存档文件\"{Path}\"读取失败",Game.Red);
                Console.ReadKey();
                Environment.Exit(-1);
                return null;
            }
        }

    }
    internal static partial class Game
    {
        static int Layer = 0;
        public static Player Player = new()
        {
            Name = "NotDefine",
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
            WriteLine("[INFO]初始化中...");
            Thread.Sleep(2500);
            Console.Write("Done\n");
            WriteLine("[INFO]存档检查...");
            Archive.Init();
            Thread.Sleep(1000);
            Console.Write("Done\n");
            WriteLine("[Success]Finshed",Green);
            Thread.Sleep(1500);
            InitUI();
        }
        public static void InitUI()
        {
            Console.Clear();
            WriteLine("###################################################");
            WriteLine("                     RoguelikeGame               ");
            Console.WriteLine("            {0,-12} : {1,-50}               ","Author","LeZi");
            Console.WriteLine("            {0,-12} : {1,-50}               ","Version","v0.1");
            Console.WriteLine("            {0,-12} : {1,-50}               ","Project","https://github.com/LeZi9916/Game");
            Console.WriteLine("            {0,-12} : {1,-50}               ","Release Date","2023/11/27");
            WriteLine("###################################################");
            if (Player.Name == "NotDefine")
            {
                WriteLine("     你看上去是第一次游玩,请先给你自己起个名字吧:");
                Player.Name = Console.ReadLine();
                Archive.Save();
                InitUI();
            }
            WriteLine($"     你好,亲爱的{Player.Name},你想?");
            WriteLine("     A. 开始游戏");
            WriteLine("     B. 退出");
            WriteLine("     C. 我想薅羊毛!");
            switch(Console.ReadKey().Key)
            {
                case ConsoleKey.A:
                    CommonUI();
                    break;
                case ConsoleKey.B:
                    Environment.Exit(114514);
                    break;
                case ConsoleKey.C:
                    Console.Clear();
                    Thread.Sleep(5000);
                    Console.WriteLine("System: 好的，请稍等");
                    Thread.Sleep(int.MaxValue);
                    Environment.Exit(250);
                    break;
                default:
                    if(Layer < 500)
                        CommonUI();
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("System: 你弱智吗?");
                        Thread.Sleep(int.MaxValue);
                        Environment.Exit(250);
                    }
                    break;
            }
        }
        public static void CommonUI()
        {
            while(true)
            {
                Clear();
                WriteLine($"     你已走了{Area.PlayerStep}步\n");
                WriteLine("     Enter. 投骰子");
                WriteLine("     B.     查看背包");
                WriteLine("     S.     查看技能");
                WriteLine("     E.     保存并退出");
                switch(Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        WriteLine("     命运的骰子正在转动...");
                        Thread.Sleep(2500);
                        var result = NextStep();
                        WriteLine($"     你向前走了{result.Step}");

                        if (result.Type is ResultType.Nothing)
                            WriteLine("     你环顾四周，什么都没发生");
                        else if (result.Type is ResultType.Event)
                            EventHandle(result.Event);
                        else if (result.Type is ResultType.Battle)
                        {
                            WriteLine($"     突然，你在前进的路上遇到了魔物");
                            Thread.Sleep(2500);
                            WriteLine($"     准备战斗!");
                            Console.ReadKey();
                            BattleUI(result.Monsters);
                        }
                        else if (result.Type is ResultType.Boss)
                        {
                            WriteLine($"     你已接近该区域的尽头，是时候挑战区域Boss了");
                            Thread.Sleep(2500);
                            WriteLine($"     准备战斗!");
                            Console.ReadKey();
                            BattleUI(result.Monsters);
                        }
                        else if (result.Type is ResultType.AreaFinish)
                        {
                            WriteLine($"     经过了无数战斗的洗礼，你在这篇区域留下了属于你的足迹");
                            Thread.Sleep(2500);
                            WriteLine($"     准备好前往下一区域了吗？");
                            Thread.Sleep(2500);
                            WriteLine("     Y. 我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了我准备好了");
                            WriteLine("     N. 我想先休息一下");
                            Archive.Save();
                            while (Console.ReadKey().Key is not ConsoleKey.Y) ;
                            Clear();
                            WriteLine("     少年祷告中...");
                            Thread.Sleep(5000);
                            var area = NextArea();
                            switch(area)
                            {
                                case AreaType.City:
                                    WriteLine("     你已进入城市!");
                                    break;
                                case AreaType.Icefield:
                                    WriteLine("     你已进入冰川区域!");
                                    break;
                                case AreaType.Grassland:
                                    WriteLine("     您已进入草原区域!");
                                    break;
                                case AreaType.Plain:
                                    WriteLine("     您已进入平原区域!");
                                    break;
                                case AreaType.Volcano:
                                    WriteLine("     您已进入火山区域!");
                                    break;
                                case AreaType.Desert:
                                    WriteLine("     您已进入沙漠区域!");
                                    break;

                            }
                            Console.ReadKey();
                        }
                        break;
                    case ConsoleKey.B:
                        break;
                    case ConsoleKey.S:
                        break;
                    case ConsoleKey.E:
                        WriteLine("[System]正在保存，请稍后...",Green);
                        Thread.Sleep(2500);
                        Archive.Save();
                        Environment.Exit(0);
                        break;
                }
            }
        }
        /// <summary>
        /// 战斗UI
        /// </summary>
        /// <param name="monsters"></param>
        public static bool BattleUI(IList<Monster> monsters)
        {
            Action<Prefab, Prefab> PrefabKilledHandle = (source, target) =>
            {
                Prefabs.Remove(target);
                monsters.Remove((Monster)target);
            };
            Prefab.PrefabKilledEvent += PrefabKilledHandle;
            var _monsters = monsters;
            bool isEscaped = false;
            Prefabs.AddRange(monsters);
            while (monsters.Count > 0 && !isEscaped)
            {
                Clear();
                Prefabs.AddRange(monsters);                
                foreach (var monster in monsters)
                    Console.WriteLine("     [{0,-2}] {1,-5} {2,-8}", monsters.IndexOf(monster), monster.Name, $"{monster.Health}/{monster.MaxHealth}");
                WriteLine("##############################################");
                WriteLine("     1.战斗    2.技能    3.物品    4.逃跑");
                WriteLine("     (选择后将无法后退,请谨慎选择)");
                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        var target = (Monster)SelectTargetUI((IList<Prefab>)monsters);
                        var damage = Player.Attack(target);
                        Console.WriteLine("     你对[{0}]{1}造成了{2}点伤害", monsters.IndexOf(target), target.Name, damage);
                        //对方行动
                        MonsterBrain(monsters);
                        break;
                    case '2':
                        ReleaseSkillUI(monsters);
                        //对方行动
                        MonsterBrain(monsters);
                        NextRound();
                        break;
                    case '3':
                        var drug = SelectItemUI();
                        if (drug is null)
                        {
                            WriteLine("     您的背包并没有任何药品/食物可供使用");
                            Console.ReadKey();
                            break;
                        }
                        else
                            ReleaseItemUI(monsters, drug);
                        MonsterBrain(monsters);
                        NextRound();
                        break;
                    case '4':
                        int rdNum = rd.Next(0, 101);
                        if(rdNum >=75)
                        {
                            WriteLine("     你已成功逃跑!", Green);
                            Thread.Sleep(2500);
                            Console.ReadKey();
                            isEscaped = true;
                            break;
                        }
                        else
                        {
                            WriteLine("     你尝试逃跑，但是失败了", Red);
                            Thread.Sleep(2500);
                            Console.ReadKey();
                        }
                        MonsterBrain(monsters);
                        NextRound();
                        break;
                }
            }
            Prefab.PrefabKilledEvent -= PrefabKilledHandle;
            if (!isEscaped)
            {
                WriteLine("     抱歉喵，这次战斗您没有获得任何奖励");
                Console.ReadKey();
            }
            else
            {
                var commonCount = _monsters.Where(monster => monster.Rank is MonsterType.Common).Count();
                var eliteCount = _monsters.Where(monster => monster.Rank is MonsterType.Elite).Count();
                var bossCount = _monsters.Where(monster => monster.Rank is MonsterType.Boss).Count();
                int bonus = 0;
                bonus += rd.Next(5, 21) * commonCount;
                bonus += rd.Next(25, 46) * eliteCount;
                bonus += rd.Next(50, 201) * bossCount;
                var coin = coinItem;
                coin.Count = bonus;
                Player.Items.Add(coin);
                WriteLine($"     你获得了{bonus}枚通用货币!");
                Console.ReadKey();
            }
            foreach (var monster in monsters)
            {
                Prefabs.Remove(monster);
            }
            Player.Buffs.ClearAll();
            return isEscaped;
        }
        static void NextRound()
        {
            foreach (Prefab prefab in Prefabs)
                prefab.NextRound();
        }
        /// <summary>
        /// 敌方行动逻辑
        /// </summary>
        /// <param name="monsters"></param>
        public static void MonsterBrain(IList<Monster> monsters)
        {
            foreach(var monster in monsters)
            {
                /// 0 = Attack
                /// 1 = Skill
                var action = WeightedRandom(new int[] { 0 , 1 }, new double[] { 0.75,0.25 });
                Prefab target = RandomChoose(Prefabs.Search<Player>(),1)[0];
                if (monster.Skills.Count is 0)
                    action = 0;
                if(action is 0)
                {
                    var damage = monster.Attack(target);
                    WriteLine($"     {monster.Name}对{target.Name}造成了{damage}点伤害");                    
                }
                else
                {
                    var skill = RandomChoose(target.Skills.GetAvailableSkills(), 1)[0];
                    if (skill.Target is TargetType.Self)
                        target = monster;
                    else if (skill.Target is TargetType.Monster)
                        target = RandomChoose(Prefabs.Search<Monster>(), 1)[0];
                    monster.ReleaseSkill(target, skill);
                    WriteLine($"     {monster.Name}对{target.Name}释放了名为\"{skill.Name}\"的技能");
                }
                Thread.Sleep(2500);
            }

        }
        /// <summary>
        /// 物品详情
        /// </summary>
        /// <param name="item"></param>
        public static void ViewItemUI(Item item)
        {
            Clear();
            WriteLine($"                     {item.Name}");
            WriteLine($"         {item.Description}");
            WriteLine($"     数量:{item.Count}");
            WriteLine($"     可堆叠:{(item.Stackable is true ? "是":"否")}");
            WriteLine($"     稀有度:{GetItemRarity(item)}\n");
            WriteLine("     输入非相关选项以退出");
            if(item.Type is not ItemType.Currency or ItemType.Common)
                WriteLine("     1. 使用 2. 丢弃");
            else
                WriteLine("     1. 丢弃");
            int userInput = 0;
            if(int.TryParse(Console.ReadLine(),out userInput))
            {
                if (userInput == 1 && item.Type is not ItemType.Currency or ItemType.Common)
                {
                    if (item.Type is ItemType.Drug)
                        Player.Release(SelectTargetUI(Prefabs.GetAllPrefab()), (Drug)item);
                    else
                        Player.Dress(item);
                }
                else
                    Player.Items.Remove(item);
            }
        }
        /// <summary>
        /// 日常查看背包
        /// </summary>
        public static void ViewBagUI()
        {
            while(true)
            {
                Clear();
                WriteLine("                     背包");
                Console.WriteLine("     [{0,-2}] {1,-8} {2,-3} {3}", "序号", "名称", "稀有度", "数量");
                foreach (Item item in Player.Items)
                    Console.WriteLine("     [{0,-2}] {1,-8} {2,-3} {3}", Player.Items.IndexOf(item), item.Name, item.Rarity, item.Count);
                WriteLine("     请输入物品序号:");
                WriteLine("     输入无关选项以退出");
                int index = -1;
                if (int.TryParse(Console.ReadLine(), out index) && (index >= 0 && index < Player.Items.Count))
                    ViewItemUI(Player.Items[index]);
                else
                    break;
            }
            
        }
        /// <summary>
        /// 释放技能UI
        /// </summary>
        /// <param name="monsters"></param>
        public static void ReleaseSkillUI(IList<Monster> monsters)
        {
            Clear();
            var skill = SelectSkillUI();
            if(skill.Target is TargetType.Self)
            {
                WriteLine("     确认要对自己使用该技能吗?");
                WriteLine("     Y. 是的");
                WriteLine("     N. 不了");
                if (Console.ReadKey().KeyChar is 'Y')
                    Player.ReleaseSkill(Player, skill);
                else
                    ReleaseSkillUI(monsters);
            }
            else if(skill.Target is TargetType.Monster)
            {
                var target = SelectTargetUI(Prefabs.Search<Monster>());
                Console.WriteLine("     确认要对[{0}]{1}使用该技能吗?",monsters.IndexOf((Monster)target),target.Name);
                WriteLine("     Y. 是的");
                WriteLine("     N. 不了");
                if (Console.ReadKey().KeyChar is 'Y')
                    Player.ReleaseSkill(target, skill);
                else
                    ReleaseSkillUI(monsters);
            }
            else if (skill.Target is TargetType.Player)
            {
                var target = SelectTargetUI(Prefabs.Search<Player>());
                Console.WriteLine("     确认要对{1}使用该技能吗?", target.Name);
                WriteLine("     Y. 是的");
                WriteLine("     N. 不了");
                if (Console.ReadKey().KeyChar is 'Y')
                    Player.ReleaseSkill(target, skill);
                else
                    ReleaseSkillUI(monsters);
            }
            else
            {
                var target = SelectTargetUI(Prefabs.Search<Prefab>());
                Console.WriteLine("     确认要对{1}使用该技能吗?", target.Name);
                WriteLine("     Y. 是的");
                WriteLine("     N. 不了");
                if (Console.ReadKey().KeyChar is 'Y')
                    Player.ReleaseSkill(target, skill);
                else
                    BattleUI(monsters);
            }
        }
        /// <summary>
        /// 使用药品的对话UI
        /// </summary>
        /// <param name="monsters"></param>
        /// <param name="drug"></param>
        public static void ReleaseItemUI(IList<Monster> monsters,Drug drug)
        {
            Clear();
            if (drug.Target is TargetType.Self)
            {
                WriteLine("     确认要对自己使用该药品/食物吗?");
                WriteLine("     Y. 是的");
                WriteLine("     N. 不了");
                if (Console.ReadKey().KeyChar is 'Y')
                    Player.Release(Player, drug);
                else
                    ReleaseItemUI(monsters,drug);
            }
            else if (drug.Target is TargetType.Monster)
            {
                var target = SelectTargetUI(Prefabs.Search<Monster>());
                Console.WriteLine("     确认要对[{0}]{1}使用该药品/食物吗?", monsters.IndexOf((Monster)target), target.Name);
                WriteLine("     Y. 是的");
                WriteLine("     N. 不了");
                if (Console.ReadKey().KeyChar is 'Y')
                    Player.Release(target, drug);
                else
                    ReleaseItemUI(monsters, drug);
            }
            else if (drug.Target is TargetType.Player)
            {
                var target = SelectTargetUI(Prefabs.Search<Player>());
                Console.WriteLine("     确认要对{1}使用该药品/食物吗?", target.Name);
                WriteLine("     Y. 是的");
                WriteLine("     N. 不了");
                if (Console.ReadKey().KeyChar is 'Y')
                    Player.Release(target, drug);
                else
                    ReleaseItemUI(monsters, drug);
            }
            else
            {
                var target = SelectTargetUI(Prefabs.Search<Prefab>());
                Console.WriteLine("     确认要对{1}使用该药品/食物吗?", target.Name);
                WriteLine("     Y. 是的");
                WriteLine("     N. 不了");
                if (Console.ReadKey().KeyChar is 'Y')
                    Player.Release(target, drug);
                else
                    BattleUI(monsters);
            }
        }
        /// <summary>
        /// 选择技能UI
        /// </summary>
        /// <returns></returns>
        public static Skill SelectSkillUI()
        {
            Clear();
            Console.WriteLine("     [{0,-2}] {1,-6} {2,-6}","序号","技能名称","剩余冷却轮数");
            foreach (Skill skill in Player.Skills)
            {
                Console.WriteLine("     [{0,-2}] {1,-6} {2,-6}", Player.Skills.IndexOf(skill), skill.Name, Player.Skills.InCoolDown(skill) is true ? Player.Skills.GetCoolDownRound(skill) : "N/A");
                WriteLine($"              {skill.Description}",Green);
            }
            int index = -1;
            while (!(index >= 0 && index < Player.Skills.Count) && Player.Skills.InCoolDown(Player.Skills[index]))
                int.TryParse(Console.ReadLine(), out index);
            return Player.Skills[index];
        }
        /// <summary>
        /// 选择目标UI
        /// </summary>
        /// <param name="targets"></param>
        /// <returns></returns>
        public static Prefab SelectTargetUI(IList<Prefab> targets)
        {
            Clear();
            foreach(var target in targets)
                Console.WriteLine("     [{0,-2}] {1,-5} {2,-8}", targets.IndexOf(target), target.Name, $"{target.Health}/{target.MaxHealth}");
            WriteLine("     \n请输入你想选取的目标:");
            int index = -1;
            while(!(index >= 0 && index < targets.Count))
                int.TryParse(Console.ReadLine(), out index);
            return targets[index];
        }
        /// <summary>
        /// 选择物品
        /// </summary>
        /// <returns></returns>
        public static Drug? SelectItemUI()
        {
            Clear();
            var drugs = Player.Items[ItemType.Drug];
            if (drugs.Count() == 0)
                return null;
            WriteLine("                     背包");
            Console.WriteLine("     [{0,-2}] {1,-8} {2,-3} {3}", "序号", "名称", "稀有度", "数量");
            foreach (Item item in drugs)
            {
                Console.WriteLine("     [{0,-2}] {1,-8} {2,-3} {3}", drugs.ToList().IndexOf(item), item.Name, item.Rarity, item.Count);
                Console.WriteLine("               {0}",item.Description);
            }
            WriteLine("     请输入物品序号:");
            int index = -1;
            while (!(index >= 0 && index < drugs.Count()))
                int.TryParse(Console.ReadLine(), out index);
            return (Drug)drugs[index];
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
                Console.WriteLine("     [{0,-2}] {1,-15} {2,-3} {3,-5}", shopItems.IndexOf(item), item.Name, GetItemRarity(item), price);
                Console.WriteLine("               ", item.Description);
            }
            //WriteLine("     稀有度一览:  \n0 普通\n1 稀有\n2 史诗\n3 传奇");
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
                var coinIndex = Player.Items.IndexOf(Player.Items["通用货币"][0]);

                WriteLine($"     您持有的通用货币:{Player.Items["通用货币"][0].Count}");
                WriteLine($"     您确定要购买{item.Name},价格为{price}吗?\n");
                WriteLine("     Y.是的(我对他一见钟情) N.还是算了(感觉一般般)");
                var inputKey = Console.ReadKey().KeyChar;
                if (inputKey == 'Y')
                {
                    if (money > price)
                    {
                        Player.Items[coinIndex].Count -= price;
                        if (!Player.Items.Add(item))
                            failure.Add(item);
                        shopItems.Remove(item);
                        itemPrice.Remove(item);
                        FailAddItemUI(failure);
                        ShopUI(shopItems, itemPrice);
                    }
                    else
                    {
                        WriteLine("     您身上的金钱貌似不够呢~请赚够钱后再来吧");
                        Console.ReadLine();
                        ShopUI(shopItems, itemPrice);
                    }

                }
                else
                    ShopUI(shopItems, itemPrice);
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
