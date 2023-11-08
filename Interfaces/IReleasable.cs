using RoguelikeGame.Class;

namespace RoguelikeGame.Interfaces
{
    internal interface IReleasable
    {
        /// <summary>
        /// 倍数，造成相当于自身攻击力Value倍的伤害；为0时，仅生效Buff；不为0时，造成伤害的同时给目标附加Buff；
        /// Value为正数时，造成伤害；Value为负数时，造成回复效果
        /// </summary>
        float Value { get; set; }
        /// <summary>
        /// 表示附带的Buff
        /// </summary>
        Buff[] Effect { get; }
        /// <summary>
        /// 表示该Skill释放类型；分为一次性或对目标施加Buff
        /// </summary>
        ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 表示作用范围
        /// </summary>
        TargetType Target {  get; set; }
    }
}
