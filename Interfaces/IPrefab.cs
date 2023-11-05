using System;

namespace RoguelikeGame.Interfaces
{
    internal interface IPrefab
    {
        void Upgrade();
        void Injuried();
        void ApplyBuff();
        void ReleaseSkill();
        void Attack();
    }
}
