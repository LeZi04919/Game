using RoguelikeGame.Class;
using RoguelikeGame.Prefabs;

namespace RoguelikeGame.Interfaces
{
    internal interface IPrefab
    {
        long MaxHealth { get; set; }
        long Health { get; set; }
        long Armor { get; set; }
        long Damage { get; set; }
        float Dodge { get; set; }
        long Level { get; set; }
        PrefabType Type { get; set; }
        SkillCollection Skills { get; set; }


        bool Upgrade();
        void ReleaseSkill(Prefab target, Skill skill);
        long Attack(Prefab target);
        void NextRound();
    }
}
