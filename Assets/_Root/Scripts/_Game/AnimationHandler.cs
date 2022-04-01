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
    }

    public async Task HandleEnemyAppearAnimation()
    {
        EnemySpawnEffect();
        bool animationCompleted = _enemyAnimController.GetAppearAnimationFinished();
        while (!animationCompleted)
        {
            await Task.Yield();
            animationCompleted = _enemyAnimController.GetAppearAnimationFinished();
        }
        _enemyView.GetAnimator().SetBool(CharState.IsReady, true);
    }

    public void StopPlayer() => _playerView.GetAnimator().SetBool(CharState.IsRunning, false);

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

    public async Task BurnEnemy()
    {
        var newMaterial = _enemyView.BurnMaterial;
        var material = _enemyView.Model.GetComponentInChildren<SkinnedMeshRenderer>().material;
        material.shader = newMaterial.shader;
        material.color = newMaterial.color;
        material.SetTexture(LiteralString.MainTexture, newMaterial.GetTexture(LiteralString.MainTexture));
        material.SetTexture(LiteralString.Noise, newMaterial.GetTexture(LiteralString.Noise));
        float amount = -1f;
        while (amount <= 1.3f)
        {
            material.SetFloat(LiteralString.Dissolve, amount);
            amount += 0.01f;
            await Task.Delay(5);
        }
    }

    private async void EnemySpawnEffect()
    {
        _enemyView.Model.SetActive(true);

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
            amount -= 0.025f;
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
        if (defendingCharKilled)
        {
            _attackerAnimatorHolder.GetAnimator().SetBool(CharState.IsFinishingOff, true);
            _attackerAnimatorHolder.GetFinishAttackEffect()?.gameObject.SetActive(true);
        }
        else
        {
            _attackerAnimatorHolder.GetAnimator().SetBool(CharState.IsAttacking, true);
            _attackerAnimatorHolder.GetMainAttackEffect()?.gameObject.SetActive(true);
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
        _defenderAnimatorHolder.GetAnimator().SetBool(CharState.GotHit, true);
        if (defendingCharKilled)
            _defenderAnimatorHolder.GetAnimator().SetBool(CharState.IsKilled, true);

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
            _playerView.GetAnimator().SetBool(CharState.IsAttacking, false);
            _playerView.GetAnimator().SetBool(CharState.IsFinishingOff, false);
            _playerView.GetMainAttackEffect()?.gameObject.SetActive(false);
            _playerView.GetFinishAttackEffect()?.gameObject.SetActive(false);

            _enemyView.GetAnimator().SetBool(CharState.IsAttacking, false);
            _enemyView.GetAnimator().SetBool(CharState.IsFinishingOff, false);
            _enemyView.GetMainAttackEffect()?.gameObject.SetActive(false);
            _enemyView.GetFinishAttackEffect()?.gameObject.SetActive(false);
        }
        if (_defenderFinishedMove)
        {
            _enemyView.GetAnimator().SetBool(CharState.IsKilled, false);
            _enemyView.GetAnimator().SetBool(CharState.GotHit, false);
            _playerView.GetAnimator().SetBool(CharState.IsKilled, false);
            _playerView.GetAnimator().SetBool(CharState.GotHit, false);
        }
    }
}

