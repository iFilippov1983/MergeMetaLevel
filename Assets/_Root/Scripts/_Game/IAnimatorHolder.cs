using UnityEngine;

namespace Game
{
    internal interface IAnimatorHolder
    {
        Animator GetAnimator();
        ParticleSystem GetAppearEffect();
        ParticleSystem GetMainAttackEffect();
        ParticleSystem GetSecondaryAttackEffect();
        ParticleSystem GetFinishAttackEffect();
        ParticleSystem GetGotHitEffect();
        ParticleSystem GetDeathEffect();
    }
}
