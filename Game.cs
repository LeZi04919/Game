using RoguelikeGame.Class;
using RoguelikeGame.Prefabs;
using System;

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
    }
}
