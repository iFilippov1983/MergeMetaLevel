using UnityEngine;

namespace Game
{
    internal interface IAnimatorHolder
    {
        Animator GetAnimator();
        ParticleSystem GetMainAttackEffect();
        ParticleSystem GetFinishAttackEffect();
    }
}
