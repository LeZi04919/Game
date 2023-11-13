using RoguelikeGame.Class;
using RoguelikeGame.Prefabs;

namespace RoguelikeGame.Interfaces
{
    internal interface IPrefab : IUpgradeable
    {
        long MaxHealth { get; set; }
        long Health { get; set; }
        long Armor { get; set; }
        long Damage { get; set; }
        float Dodge { get; set; }
        PrefabType Type { get; set; }
        SkillCollection Skills { get; set; }

        void ReleaseSkill(Prefab target, Skill skill);
        long Attack(Prefab target);
        void NextRound();
    }
}
