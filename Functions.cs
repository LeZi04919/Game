using RoguelikeGame.Class;
using RoguelikeGame.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using static RoguelikeGame.GameResources;

namespace RoguelikeGame
{
    internal static partial class Game
    {
        static Random rd = new();
        /// <summary>
        /// 表示当前Area信息
        /// </summary>
        static MapArea Area = new MapArea()
        {
            Type = AreaType.Plain,
            AreaStep = 90,
            PlayerStep = 0
        };
        /// <summary>
        /// 表示Player已走步数
        /// </summary>
        static int TotalStep = 0;
        static PrefabCollection Prefabs = new();//当前场景下的实体集合   
        static event Action<Prefab> PrefabKilledEvent = prefab => 
        {
            if(prefab.Type is PrefabType.Monster)
                Console.WriteLine("");
        };//被杀死对象
        static event Action<Prefab, Prefab, long> PrefabAttacked;//Source(攻击者)，Target(受击者),伤害大小

        /// <summary>
        /// 投骰子进行下一步
        /// </summary>
        /// <returns></returns>
        public static Result NextStep()
        {
            var result = new Result();
            var step = rd.Next(1,7);
            Area.PlayerStep += step;
            var remainingStep = Area.AreaStep - Area.PlayerStep;

            if (remainingStep <= 0)//已完成区域
            {
                result.Step = step + remainingStep;
                TotalStep += result.Step;
                Area.PlayerStep = Area.AreaStep;
                result.Type = ResultType.Boss;
                result.Monsters = CreateMonsters(MonsterType.Boss);
                return result;
            }
            else
            {
                result.Step = step;
                TotalStep += step;
                result.Type = WeightedRandom(new ResultType[]{ ResultType.Nothing,ResultType.Event,ResultType.Battle},new double[] {0.2,0.4,0.4});
                switch(result.Type)
                {
                    case ResultType.Event:
                        result.Event = RandomEvent();
                        break;
                    case ResultType.Battle:
                        result.Monsters = CreateMonsters(WeightedRandom(new MonsterType[] {MonsterType.Common,MonsterType.Elite },new double[] { 0.6,0.4 }));
                        break;
                }
                return result;
            }
        }
        /// <summary>
        /// 随机生成一个事件
        /// </summary>
        public static AreaEvent RandomEvent()
        {
            var eventType = WeightedRandom(new EventType[] {EventType.Adventure,EventType.Shop,EventType.Trap,EventType.Status},new double[] {0.4,0.1,0.4,0.1});
            var Events = EventList.Where(e => (e.Area == Area.Type || e.Area == AreaType.Common) && e.Type == eventType);

            return RandomChoose(Events.ToList());

        }
        /// <summary>
        /// 随机选择一个地图
        /// </summary>
        /// <returns></returns>
        public static AreaType NextArea()
        {
            var AreaTypes = new AreaType[] { AreaType.Desert, AreaType.City, AreaType.Icefield, AreaType.Grassland, AreaType.Plain, AreaType.Volcano };
            var areaType = RandomChoose(AreaTypes);
            double bonus = rd.Next(70,131) / 100;
            
            while (areaType == Area.Type )
                areaType = RandomChoose(AreaTypes);
            Area.AreaStep = (int)(90 * bonus);
            Area.Type = areaType;
            return areaType;
        }
        public static void EventHandle(AreaEvent e)
        {            

        }
       /// <summary>
       /// 根据预设权重，随机生成指定数量的Item
       /// </summary>
       /// <param name="maxRank"></param>
       /// <param name="count"></param>
       /// <returns></returns>
        public static Item[] CreateItems(RarityType maxRank,int count)
        {
            const double legacyP = 0.02;
            const double epicP = 0.1;
            const double rareP = 0.20;
            const double commonP = 0.68;

            return CreateItems(maxRank, count, commonP, rareP, epicP, legacyP);
        }
        /// <summary>
        /// 根据权重，随机生成指定数量的Item
        /// </summary>
        /// <param name="maxRank"></param>
        /// <param name="count"></param>
        /// <param name="commonP"></param>
        /// <param name="rareP"></param>
        /// <param name="epicP"></param>
        /// <param name="legacyP"></param>
        /// <returns></returns>
        public static Item[] CreateItems(RarityType maxRank, int count,double commonP,double rareP,double epicP,double legacyP)
        {
            List<Item> items = new();
            while (items.Count != count)
            {
                var legacy = RandomChoose(legacyItems);
                var epic = RandomChoose(epicItems);
                var rare = RandomChoose(rareItems);
                var common = RandomChoose(commonItems);
                switch (maxRank)
                {
                    case RarityType.Legacy:
                        items.Add(WeightedRandom(new Item[] { common, rare, epic, legacy }, new double[] { commonP, rareP, epicP, legacyP }));
                        break;
                    case RarityType.Epic:
                        items.Add(WeightedRandom(new Item[] { common, rare, epic }, new double[] { commonP, rareP, epicP }));
                        break;
                    case RarityType.Rare:
                        items.Add(WeightedRandom(new Item[] { common, rare }, new double[] { commonP, rareP }));
                        break;
                    default:
                        items.Add(common);
                        break;
                }
            }
            return items.ToArray();
        }
        /// <summary>
        /// 随机生成一个或一组不超过指定阶级的Monster
        /// </summary>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        public static Monster[] CreateMonsters(MonsterType maxRank)
        {
            return CreateMonsters(maxRank, WeightedRandom(new int[] { 1, 2, 3, 4 }, new double[] { 0.35, 0.35, 0.2, 0.1 }));
        }
        /// <summary>
        /// 随机生成指定数量不超过指定阶级的Monster
        /// </summary>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        public static Monster[] CreateMonsters(MonsterType maxRank,int count)
        { 
            List<Monster> monsters;
            if(maxRank is MonsterType.Boss)
            {
                count = Math.Max(2, count);
                var boss = RandomChoose(MonsterList[MonsterType.Boss]);
                var eliteCount = Math.Min(count - 1, WeightedRandom(new int[] { 1, 2, 3 }, new double[] { 0.45, 0.35, 0.2 }));
                var commonCount = count - 1 - eliteCount;
                var elites = RandomChoose(MonsterList[MonsterType.Elite], eliteCount);
                var common = RandomChoose(MonsterList[MonsterType.Common], commonCount);
                monsters = new(elites)
                {
                    boss
                };
                monsters.AddRange(common);
                return monsters.ToArray();
            }
            else if(maxRank is MonsterType.Elite)
            {
                monsters = new(RandomChoose(MonsterList[MonsterType.Elite], rd.Next(1, count)));
                monsters.AddRange(RandomChoose(MonsterList[MonsterType.Common], count - monsters.Count));
                return monsters.ToArray();
            }
            else
                return RandomChoose(MonsterList[MonsterType.Elite], count);

        }
        /// <summary>
        /// 随机从Array中选取指定数量的对象并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] RandomChoose<T>(IList<T> array,int count)
        {
            List<T> results = new();
            while (results.Count != count)
                results.Add(RandomChoose(array));
            return results.ToArray();
        }
        /// <summary>
        /// 随机从Array中选取一个对象并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T RandomChoose<T>(IList<T> array)
        {
            return array[new Random().Next(array.Count)];
        }
        /// <summary>
        /// 根据权重，随机从array中选取指定数量的对象并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pairs"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] WeightedRandom<T>(IDictionary<T, double> pairs,int count) where T : notnull
        {
            List<T> results = new();
            while(results.Count != count)
                results.Add(WeightedRandom(pairs));
            return results.ToArray();
        }
        /// <summary>
        /// 根据权重，随机从array中选取指定数量的对象并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="weights"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] WeightedRandom<T>(IList<T> array, double[] weights, int count) where T : notnull
        {
            List<T> results = new();
            while(results.Count != count)
                results.Add(WeightedRandom(array, weights));
            return results.ToArray();
        }
        /// <summary>
        /// 根据权重，随机从array中选取一个对象并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static T WeightedRandom<T>(IDictionary<T, double> pairs) where T : notnull
        {
            double totalWeight = pairs.Values.Sum();
            var randomDouble = new Random().NextDouble();
            var randomResult = totalWeight * randomDouble;
            foreach (var pair in pairs)
                if ((randomResult -= pair.Value) < 0)
                    return pair.Key;
            return pairs.Keys.Last();
        }
        /// <summary>
        /// 根据权重，随机从array中选取一个对象并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="weights"></param>
        /// <returns></returns>
        public static T WeightedRandom<T>(IList<T> array, double[] weights) where T : notnull
        {
            double totalWeight = weights.Sum();
            var randomResult = new Random().NextDouble() * totalWeight;
            foreach (var weight in weights)
                if ((randomResult -= weight) < 0)
                    return array[Array.IndexOf(weights,weight)];
            return array.Last();
        }
    }
}
