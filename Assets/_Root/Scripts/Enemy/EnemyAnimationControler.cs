using UnityEngine;

namespace Enemy
{
    public class EnemyAnimationControler : MonoBehaviour
    {
        public bool appearAnimationFinished;
        public bool deathAnimationFinished;
        public bool gotHitAnimationFinished;
        public bool attackAnimationFinished;

        private void Awake()
        {
            appearAnimationFinished = false;
            deathAnimationFinished = false;
            gotHitAnimationFinished = false;
            attackAnimationFinished = false;
        }

        public void AppearAnimationFinished() => appearAnimationFinished = true;
        public void DeathAnimationFinished() => deathAnimationFinished = true;
        public void GotHitAnimationFinished() => gotHitAnimationFinished = true;
        public void AttackAnimationFinished() => attackAnimationFinished = true;

        public void ResetFlags()
        {
            appearAnimationFinished = false;
            deathAnimationFinished = false;
            gotHitAnimationFinished = false;
            attackAnimationFinished = false;
        }
    }
}