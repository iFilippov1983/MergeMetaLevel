using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public bool attackAnimationFinished;
        public bool gotHitAnimationFinished;
        public bool deathAnimationFinished;

        private void Awake()
        {
            attackAnimationFinished = false;
            gotHitAnimationFinished = false;
            deathAnimationFinished = false;
        }

        public void AttackAnimationFinished() => attackAnimationFinished = true;
        public void GotHitAnimationFinisged() => gotHitAnimationFinished = true;
        public void DeathAnimationFinished() => deathAnimationFinished = true;

        public void ResetFlags()
        {
            attackAnimationFinished = false;
            gotHitAnimationFinished = false;
            deathAnimationFinished = false;
        }
    }
}
