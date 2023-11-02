using PyGame.Prefabs;

namespace PyGame
{
    internal class Buff
    {
        public required string Name;
        public required BuffEffect Effect;
        public required int Rounds;
        public required Prefab[] EffectiveObjects;
        public required float Value;

        public Buff(string Name, BuffEffect Effect, int Rounds, Prefab[] EffectiveObjects, float Value)
        {
            this.Name = Name;
            this.Effect = Effect;
            this.Rounds = Rounds;
            this.Value = Value;
            this.EffectiveObjects = EffectiveObjects;
        }
    }
}
