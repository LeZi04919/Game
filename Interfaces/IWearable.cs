
namespace RoguelikeGame.Interfaces
{
    internal interface IWearable
    {
        int Count { get;}
        int MaxStackCount { get;}
        long Value { get; set; }
    }
}
