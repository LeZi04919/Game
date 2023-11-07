namespace RoguelikeGame
{
    public enum BuffEffect
    {
        DamageUp,
        DamageDown, 
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
        Drug,
        Currency
    }
    public enum TargetType
    {
        All,//对所有目标生效
        Self,//仅对自身生效
        Monster,//对Monster及敌对Player生效
        Player //对自身及友方Player生效
    }
    public enum MonsterType
    {
        Common,
        Elite,
        Boss
    }
    public enum ArmorType
    {
        Physical,
        Dodge
    }
    public enum RarityType
    {
        Common,
        Rare,
        Epic,
        Legacy
    }
}
