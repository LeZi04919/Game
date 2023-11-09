using RoguelikeGame.Class;

namespace RoguelikeGame.Prefabs
{
    internal class Monster : Prefab
    {
        /// <summary>
        /// 表示Monster的危险程度，分为普通、精英、首领三种
        /// </summary>
        public required MonsterType Rank;
        public Monster()
        {
            Type = PrefabType.Monster;
        }
        public Monster(long MaxHealth, long Armor, long Damage, float Dodge, long Level, MonsterType Rank,SkillCollection Skills) : base(MaxHealth, Armor, Damage, Dodge, Level, PrefabType.Monster, Skills)
        {
            this.Rank = Rank;
        }
    }
}