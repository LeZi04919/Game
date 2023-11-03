using System;

namespace PyGame.Interfaces
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
