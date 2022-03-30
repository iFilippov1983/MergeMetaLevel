using UnityEngine;

namespace Game
{
    public class CharacterAnimationControler : MonoBehaviour, IAnimationControler
    {
        private bool _appearAnimationFinished;
        private bool _attackAnimationFinished;
        private bool _gotHitAnimationFinished;
        private bool _deathAnimationFinished;
        private bool _finishOffAnimationFinished;

        private void Awake()
        {
            ResetFlags();
        }

        public bool GetAppearAnimationFinished() => _appearAnimationFinished;
        public bool GetAttackAnimationFinished() => _attackAnimationFinished;
        public bool GetGotHitAnimationFinished() => _gotHitAnimationFinished;
        public bool GetDeathAnimationFinished() => _deathAnimationFinished;
        public bool GetFinishOffAnimationFinished() => _finishOffAnimationFinished;

        public void ResetFlags()
        {
            _appearAnimationFinished = false;
            _deathAnimationFinished = false;
            _gotHitAnimationFinished = false;
            _attackAnimationFinished = false;
            _finishOffAnimationFinished = false;
        }

        private void AppearAnimationFinish() => _appearAnimationFinished = true;
        private void DeathAnimationFinish() => _deathAnimationFinished = true;
        private void GotHitAnimationFinish() => _gotHitAnimationFinished = true;
        private void AttackAnimationFinish() => _attackAnimationFinished = true;
        private void FinishOffAnimationFinish() => _finishOffAnimationFinished = true;
    }
}