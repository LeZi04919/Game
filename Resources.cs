namespace RoguelikeGame
{
    public enum BuffEffect
    {
        AttackUp,
        AttackDown, 
        ArmorUp,
        ArmorDown,
        DodgeUp,
        DodgeDown,
        Dizziness,
        Freeze,
        Firing,
        AreaAttack
    }
    enum Overlay
    {
        Add,
        Mul
    }
    public enum SkillType
    {
        Normal, //直接造成伤害
        Buff    //施加正面或负面Buff
    }
    public enum PrefabType
    {
        Player,
        Monster
    }
    public enum ItemType
    {
        Common,
        Weapon,
        Armor,
        Drug
    }
}
