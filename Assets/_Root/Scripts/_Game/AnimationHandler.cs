using Data;
using Enemy;
using Game;
using Player;
using System;
using System.Threading.Tasks;
using UnityEngine;

internal class AnimationHandler
{
    private PlayerView _playerView;
    private CharacterAnimationControler _playerAnimController;
    private EnemyView _enemyView;
    private CharacterAnimationControler _enemyAnimController;
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
    }

    public void SetEnemyToControl(GameObject enemyObject)
    { 
        _enemyView = enemyObject.GetComponent<EnemyView>();
        _enemyAnimController = enemyObject.GetComponent<CharacterAnimationControler>();
        //_enemyAnimController.SetAppearEffect(_enemyView.AppearEffect);
    }

    public async Task HandleEnemyAppearAnimation()
    {
        EnemySpawnEffect(_enemyView.EnemyType);
        bool animationCompleted = _enemyAnimController.GetAppearAnimationFinished();
        while (!animationCompleted)
        {
            await Task.Yield();
            animationCompleted = _enemyAnimController.GetAppearAnimationFinished();
        }
        _enemyView.GetAnimator().SetBool(AnimParameter.IsReady, true);
    }

    public void StopPlayer() => _playerView.GetAnimator().SetBool(AnimParameter.IsRunning, false);

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
        material.SetTexture(LiteralString.MainTexture, newMaterial.GetTexture(LiteralString.MainTexture));
        material.SetTexture(LiteralString.Noise, newMaterial.GetTexture(LiteralString.Noise));
        float amount = -0.7f;
        while (amount <= 1.3f)
        {
            material.SetFloat(LiteralString.Dissolve, amount);
            amount += 0.01f;
            await Task.Delay(5);
        }
    }

    private async void EnemySpawnEffect(EnemyType enemyType)
    {
        _enemyView.Model.SetActive(true);

        if (enemyType.Equals(EnemyType.Pigoblin)) return;

        var newMaterial = _enemyView.BurnMaterial;
        var material = _enemyView.Model.GetComponentInChildren<SkinnedMeshRenderer>().material;

        material.shader = newMaterial.shader;
        material.color = newMaterial.color;
        material.SetTexture(LiteralString.MainTexture, newMaterial.GetTexture(LiteralString.MainTexture));
        material.SetTexture(LiteralString.Noise, newMaterial.GetTexture(LiteralString.Noise));

        float amount = 1.4f;
        while (amount >= -0.8f)
        {
            material.SetFloat(LiteralString.Dissolve, amount);
            amount -= 0.035f;
            await Task.Delay(1);
        }

        newMaterial = _enemyView.DefaultMaterial;
        material.shader = newMaterial.shader;
        material.color = newMaterial.color;
        material.SetTexture(LiteralString.BaseTexture, newMaterial.GetTexture(LiteralString.BaseTexture));
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
        var attackType = UnityEngine.Random.Range(AnimParameter.AttackType_one, AnimParameter.AttackType_two + 1);
        if (defendingCharKilled)
        {
            _attackerAnimatorHolder.GetAnimator().SetBool(AnimParameter.IsFinishingOff, true);
        }
        else
        {
            _attackerAnimatorHolder.GetAnimator().SetBool(AnimParameter.IsAttacking, true);
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
            _playerView.GetAnimator().SetBool(AnimParameter.IsAttacking, false);
            _playerView.GetAnimator().SetInteger(AnimParameter.AttackType, AnimParameter.DefaulAttackType);
            _playerView.GetAnimator().SetBool(AnimParameter.IsFinishingOff, false);

            _enemyView.GetAnimator().SetBool(AnimParameter.IsAttacking, false);
            _enemyView.GetAnimator().SetInteger(AnimParameter.AttackType, AnimParameter.DefaulAttackType);
            _enemyView.GetAnimator().SetBool(AnimParameter.IsFinishingOff, false);
        }
        if (_defenderFinishedMove)
        {
            _enemyView.GetAnimator().SetBool(AnimParameter.IsKilled, false);
            _enemyView.GetAnimator().SetBool(AnimParameter.GotHit, false);
            _enemyView.GetAnimator().SetInteger(AnimParameter.GotHitType, AnimParameter.DefaultGotHitType);

            _playerView.GetAnimator().SetBool(AnimParameter.IsKilled, false);
            _playerView.GetAnimator().SetInteger(AnimParameter.DeathType, AnimParameter.DefaultDeathType);
            _playerView.GetAnimator().SetBool(AnimParameter.GotHit, false);
            _playerView.GetAnimator().SetInteger(AnimParameter.GotHitType, AnimParameter.DefaultGotHitType);
        }
    }
}

