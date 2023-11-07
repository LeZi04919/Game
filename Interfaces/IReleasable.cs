using RoguelikeGame.Class;

namespace RoguelikeGame.Interfaces
{
    internal interface IReleasable
    {
        float Value { get; set; }//代表倍数，造成相当于自身攻击力200%的伤害；为0时，仅生效Buff；不为0时，造成伤害的同时给目标附加Buff
        Buff[] Effect { get; }
        ReleaseType ReleaseType { get; set; }
        TargetType Target {  get; set; }
    }
}
