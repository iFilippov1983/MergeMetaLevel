using Data;
using Enemy;
using Game;
using Player;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game 
{
    internal class AnimationHandler
    {
        private PlayerView _playerView;
        private CharacterAnimationControler _playerAnimController;
        private Animator _playerAnimator;
        private EnemyView _enemyView;
        private CharacterAnimationControler _enemyAnimController;
        private Animator _enemyAnimator;
        private bool _attackerFinishedMove;
        private bool _defenderFinishedMove;

        private IAnimatorHolder _attackerAnimatorHolder;
        private IAnimatorHolder _defenderAnimatorHolder;
        private IAnimationControler _attackerController;
        private IAnimationControler _defenderController;

        public AnimationHandler(PlayerView playerView, CharacterAnimationControler playerAnimController)
        {
            _playerView = playerView;
            _playerAnimController = playerAnimController;
            _playerAnimator = _playerView.GetAnimator();
        }

        public void SetEnemyToControl(GameObject enemyObject)
        {
            _enemyView = enemyObject.GetComponent<EnemyView>();
            _enemyAnimController = enemyObject.GetComponent<CharacterAnimationControler>();
            _enemyAnimator = _enemyView.GetAnimator();
        }

        public async void HandleEnemyAppearAnimation()
        {
            EnemySpawnEffect(_enemyView.EnemyType);
            bool animationCompleted = _enemyAnimController.GetAppearAnimationFinished();
            while (!animationCompleted)
            {
                await Task.Yield();
                animationCompleted = _enemyAnimController.GetAppearAnimationFinished();
            }
            _enemyAnimator.SetBool(AnimParameter.IsReady, true);
        }

        public void StopPlayer(bool prepareToFight, bool gotResource)
        {
            _playerAnimator.SetBool(AnimParameter.IsRunning, false);
            _playerAnimator.SetBool(AnimParameter.IsAware, prepareToFight);
        }

        public async void SetPlayerAwareState(bool prepareToFight = true)
        {
            _playerAnimator.SetBool(AnimParameter.IsAware, prepareToFight);
            while (_playerAnimator.GetBool(AnimParameter.IsFacedToCamera) != false)
                await Task.Yield();
        }

        public async Task SetPlayerRunState()
        {
            if (_playerAnimator.GetBool(AnimParameter.IsRunning)) return;

            _playerAnimator.SetBool(AnimParameter.IsRunning, true);
            while (_playerAnimator.GetBool(AnimParameter.IsFacedToCamera) != false)
                await Task.Yield();
        }

        public async void SetPlayerIdleState(bool gotResource = false)
        {
            _playerAnimator.SetBool(AnimParameter.IsAware, false);
            while (_playerAnimator.GetBool(AnimParameter.IsFacedToCamera) != true)
                await Task.Yield();
        }

        public void PlayerFacedStateChange(bool facedToPlayer) => _playerAnimator.SetBool(AnimParameter.IsFacedToCamera, facedToPlayer);

        public async Task AnimateHit(bool playerAttacking, bool defendingCharKilled, Action<bool> onHitEvent)
        {
            SetSides(playerAttacking);
            SetAttackType(defendingCharKilled);
            await Hit();
            onHitEvent?.Invoke(playerAttacking);
            await AttackFinish(defendingCharKilled);
            _playerAnimController.ResetFlags();
            _enemyAnimController.ResetFlags();
        }

        public async Task BurnEnemyEffect()
        {
            var newMaterial = _enemyView.BurnMaterial;
            var material = _enemyView.Model.GetComponentInChildren<SkinnedMeshRenderer>().material;
            material.shader = newMaterial.shader;
            material.color = newMaterial.color;
            material.SetTexture(Literal.VarName_MainTexture, newMaterial.GetTexture(Literal.VarName_MainTexture));
            material.SetTexture(Literal.VarName_Noise, newMaterial.GetTexture(Literal.VarName_Noise));

#if UNITY_EDITOR
            float amount = -0.7f;
            while (amount <= 1.3f)
            {
                material.SetFloat(Literal.VarName_Dissolve, amount);
                amount += 0.01f;
                await Task.Delay(5);
            }
#elif UNITY_IOS
            float amount = 1.3f;
            while (amount >= -0.7f)
            {
                material.SetFloat(LiteralString.Dissolve, amount);
                amount -= 0.01f;
                await Task.Delay(5);
            }
#endif
        }

        public async void ActivateLevelUpParticle()
        {
            var particle = _playerView.LevelUpParticle;
            particle.gameObject.SetActive(true);
            particle.Play();
            while (particle.isPlaying)
                await Task.Yield();

            particle.gameObject.SetActive(false);
        }

        private async void EnemySpawnEffect(EnemyType enemyType)
        {
            _enemyView.Model.SetActive(true);

            if (enemyType.Equals(EnemyType.Pigoblin)) return;

            var newMaterial = _enemyView.BurnMaterial;
            var material = _enemyView.Model.GetComponentInChildren<SkinnedMeshRenderer>().material;

            material.shader = newMaterial.shader;
            material.color = newMaterial.color;
            material.SetTexture(Literal.VarName_MainTexture, newMaterial.GetTexture(Literal.VarName_MainTexture));
            material.SetTexture(Literal.VarName_Noise, newMaterial.GetTexture(Literal.VarName_Noise));

#if UNITY_EDITOR
            float amount = 1.4f;
            while (amount >= -0.8f)
            {
                material.SetFloat(Literal.VarName_Dissolve, amount);
                amount -= 0.035f;
                await Task.Delay(1);
            }
#elif UNITY_IOS
            float amount = -0.8f;
            while (amount <= 1.4f)
            {
                material.SetFloat(LiteralString.Dissolve, amount);
                amount += 0.035f;
                await Task.Delay(1);
            }
#endif
            newMaterial = _enemyView.DefaultMaterial;
            material.shader = newMaterial.shader;
            material.color = newMaterial.color;
            material.SetTexture(Literal.VarName_BaseTexture, newMaterial.GetTexture(Literal.VarName_BaseTexture));
        }

        private void SetSides(bool playerAttacking)
        {
            if (playerAttacking)
            {
                _attackerAnimatorHolder = _playerView;
                _attackerController = _playerAnimController;
                _defenderAnimatorHolder = _enemyView;
                _defenderController = _enemyAnimController;
            }
            else
            {
                _attackerAnimatorHolder = _enemyView;
                _attackerController = _enemyAnimController;
                _defenderAnimatorHolder = _playerView;
                _defenderController = _playerAnimController;
            }
        }

        private void SetAttackType(bool defendingCharKilled)
        {
            if (defendingCharKilled)
            {
                _attackerAnimatorHolder.GetAnimator().SetBool(AnimParameter.IsFinishingOff, true);
            }
            else
            {
                _attackerAnimatorHolder.GetAnimator().SetBool(AnimParameter.IsAttacking, true);
                var attackType = UnityEngine.Random.Range(AnimParameter.AttackType_one, AnimParameter.AttackType_two + 1);
                _attackerAnimatorHolder.GetAnimator().SetInteger(AnimParameter.AttackType, attackType);
            }
        }

        private async Task Hit()
        {
            var hit = _attackerController.IsHit();
            while (!hit)
            {
                await Task.Yield();
                hit = _attackerController.IsHit();
            }
        }

        private async Task AttackFinish(bool defendingCharKilled)
        {
            var gotHitType = UnityEngine.Random.Range(AnimParameter.GotHitType_one, AnimParameter.GotHitType_two + 1);
            _defenderAnimatorHolder.GetAnimator().SetInteger(AnimParameter.GotHitType, gotHitType);
            _defenderAnimatorHolder.GetAnimator().SetBool(AnimParameter.GotHit, true);

            if (defendingCharKilled)
            {
                var deathType = UnityEngine.Random.Range(AnimParameter.DeathType_one, AnimParameter.DeathType_two + 1);
                _defenderAnimatorHolder.GetAnimator().SetInteger(AnimParameter.DeathType, deathType);
                _defenderAnimatorHolder.GetAnimator().SetBool(AnimParameter.IsKilled, true);
            }

            SetAnimationFinishFlags(_attackerController, _defenderController, defendingCharKilled);

            while (!_attackerFinishedMove || !_defenderFinishedMove)
            {
                await Task.Yield();
                SetAnimationFinishFlags(_attackerController, _defenderController, defendingCharKilled);
            }
        }

        private void SetAnimationFinishFlags(IAnimationControler attacker, IAnimationControler defender, bool defendingCharKilled)
        {
            _attackerFinishedMove = defendingCharKilled
                ? attacker.GetFinishOffAnimationFinished()
                : attacker.GetAttackAnimationFinished();

            _defenderFinishedMove = defendingCharKilled
                ? defender.GetDeathAnimationFinished()
                : defender.GetGotHitAnimationFinished();

            if (_attackerFinishedMove)
            {
                _playerAnimator.SetBool(AnimParameter.IsAttacking, false);
                _playerAnimator.SetInteger(AnimParameter.AttackType, AnimParameter.DefaulAttackType);
                _playerAnimator.SetBool(AnimParameter.IsFinishingOff, false);

                _enemyAnimator.SetBool(AnimParameter.IsAttacking, false);
                _enemyAnimator.SetInteger(AnimParameter.AttackType, AnimParameter.DefaulAttackType);
                _enemyAnimator.SetBool(AnimParameter.IsFinishingOff, false);
            }
            if (_defenderFinishedMove)
            {
                _enemyAnimator.SetBool(AnimParameter.IsKilled, false);
                _enemyAnimator.SetBool(AnimParameter.GotHit, false);
                _enemyAnimator.SetInteger(AnimParameter.GotHitType, AnimParameter.DefaultGotHitType);

                _playerAnimator.SetBool(AnimParameter.IsKilled, false);
                _playerAnimator.SetInteger(AnimParameter.DeathType, AnimParameter.DefaultDeathType);
                _playerAnimator.SetBool(AnimParameter.GotHit, false);
                _playerAnimator.SetInteger(AnimParameter.GotHitType, AnimParameter.DefaultGotHitType);
            }
        }
    }
}

