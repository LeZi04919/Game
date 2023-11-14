using RoguelikeGame.Prefabs;
using System;

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

    }
}
