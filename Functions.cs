using RoguelikeGame.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoguelikeGame
{
    internal static partial class Game
    {
        static PrefabCollection Prefabs = new();//当前场景下的实体集合
        static event Action<Prefab> PrefabKilledEvent = prefab => 
        {
            if(prefab.Type is PrefabType.Monster)
                Console.WriteLine("");
        };//被杀死对象
        static event Action<Prefab,Prefab,long> PrefabAttacked;//Source(攻击者)，Target(受击者),伤害大小

        public static void CreateMonsters(MonsterType maxLevel)
        {
            
        }
        public static T WeightedRandom<T>(IDictionary<T, double> pairs) where T : notnull
        {
            double totalWeight = pairs.Values.Sum();
            var randomDouble = new Random().NextDouble();
            var randomResult = totalWeight * randomDouble;
            foreach (var pair in pairs)
                if ((randomResult -= pair.Value) < 0)
                    return pair.Key;
            return pairs.Keys.Last();
        }//加权随机
        public static T WeightedRandom<T>(IList<T> Keys, double[] weights) where T : notnull
        {
            double totalWeight = weights.Sum();
            var randomResult = new Random().NextDouble() * totalWeight;
            foreach (var weight in weights)
                if ((randomResult -= weight) < 0)
                    return Keys[Array.IndexOf(weights,weight)];
            return Keys.Last();
        }//加权随机
    }
}
