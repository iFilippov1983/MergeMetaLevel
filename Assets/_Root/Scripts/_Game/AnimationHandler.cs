using Data;
using Enemy;
using Game;
using Player;
using System.Threading.Tasks;
using UnityEngine;

internal class AnimationHandler
{
    private PlayerView _playerView;
    private CharacterAnimationControler _playerAnimController;
    private UiInfoHandler _playerInfoHandler;
    private EnemyView _enemyView;
    private CharacterAnimationControler _enemyAnimController;
    private UiInfoHandler _enemyInfoHandler;
    private bool _attackerMoves;
    private bool _defenderMoves;

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
        bool animationCompleted = _enemyAnimController.GetAppearAnimationFinished();
        while (!animationCompleted)
        {
            await Task.Yield();
            animationCompleted = _enemyAnimController.GetAppearAnimationFinished();
        }
        _enemyView.GetAnimator().SetBool(CharState.IsReady, true);
    }

    public void StopPlayer() => _playerView.GetAnimator().SetBool(CharState.IsRunning, false);

    public async Task AnimateHit(bool playerAttacking, bool defendingCharKilled)
    {
        await SetStartFlags(playerAttacking, defendingCharKilled);
        SetFinishFlags(playerAttacking, defendingCharKilled);

        while (!_attackerMoves || !_defenderMoves)
        {
            await Task.Yield();
            SetFinishFlags(playerAttacking, defendingCharKilled);
        }

        _playerAnimController.ResetFlags();
        _enemyAnimController.ResetFlags();
    }

    private async Task SetStartFlags(bool playerAttacking, bool defendingCharKilled)
    {
        if (playerAttacking)
            await SetAnimationStartFlags(_playerView, _enemyView, defendingCharKilled);
        else
            await SetAnimationStartFlags(_enemyView, _playerView, defendingCharKilled);
    }

    private async Task SetAnimationStartFlags(IAnimatorHolder attacker, IAnimatorHolder defender, bool defendingCharKilled)
    { 
        attacker.GetAnimator().SetBool(CharState.IsAttacking, true);

        await Task.Delay(500);//timing animation delay

        if (defendingCharKilled)
        {
            defender.GetAnimator().SetBool(CharState.GotHit, true);
            defender.GetAnimator().SetBool(CharState.IsKilled, true);
        }
        else
            defender.GetAnimator().SetBool(CharState.GotHit, true);
    }

    private void SetFinishFlags(bool playerAttacking, bool defendingCharKilled)
    {
        if (playerAttacking)
            SetAnimationFinishFlags(_playerAnimController, _enemyAnimController, defendingCharKilled);
        else
            SetAnimationFinishFlags(_enemyAnimController, _playerAnimController, defendingCharKilled);
    }

    private void SetAnimationFinishFlags(IAnimationControler attacker, IAnimationControler defender, bool defendingCharKilled)
    {
        _attackerMoves = attacker.GetAttackAnimationFinished();
        _defenderMoves = defendingCharKilled
            ? defender.GetDeathAnimationFinished()
            : defender.GetGotHitAnimationFinished();

        if (_attackerMoves)
        {
            _playerView.GetAnimator().SetBool(CharState.IsAttacking, false);
            _enemyView.GetAnimator().SetBool(CharState.IsAttacking, false);
        }
        if (_defenderMoves)
        {
            _enemyView.GetAnimator().SetBool(CharState.IsKilled, false);
            _enemyView.GetAnimator().SetBool(CharState.GotHit, false);
            _playerView.GetAnimator().SetBool(CharState.IsKilled, false);
            _playerView.GetAnimator().SetBool(CharState.GotHit, false);
        }
    }
}

