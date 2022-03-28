using UnityEngine;

namespace Game
{
    public class CharacterAnimationControler : MonoBehaviour, IAnimationControler
    {
        private bool appearAnimationFinished;
        private bool attackAnimationFinished;
        private bool gotHitAnimationFinished;
        private bool deathAnimationFinished;

        private void Awake()
        {
            ResetFlags();
        }

        public bool GetAppearAnimationFinished() => appearAnimationFinished;
        public bool GetAttackAnimationFinished() => attackAnimationFinished;
        public bool GetGotHitAnimationFinished() => gotHitAnimationFinished;
        public bool GetDeathAnimationFinished() => deathAnimationFinished;

        public void ResetFlags()
        {
            appearAnimationFinished = false;
            deathAnimationFinished = false;
            gotHitAnimationFinished = false;
            attackAnimationFinished = false;
        }

        private void AppearAnimationFinish() => appearAnimationFinished = true;
        private void DeathAnimationFinish() => deathAnimationFinished = true;
        private void GotHitAnimationFinish() => gotHitAnimationFinished = true;
        private void AttackAnimationFinish() => attackAnimationFinished = true;
    }
}