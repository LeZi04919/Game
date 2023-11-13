
namespace RoguelikeGame.Interfaces
{
    internal interface IUpgradeable
    {
        long Level { get; set; }
        bool Upgrade(long newLevel);
    }
}
