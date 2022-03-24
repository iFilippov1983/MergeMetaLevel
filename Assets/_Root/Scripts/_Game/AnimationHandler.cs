using Data;
using Enemy;
using Player;
using System.Threading.Tasks;
using UnityEngine;

internal class AnimationHandler
{
    private PlayerView _playerView;
    private PlayerAnimationController _playerAnimController;
    private EnemyView _enemyView;
    private EnemyAnimationControler _enemyAnimController;
    private PlayerProfile _playerProfile;

    public AnimationHandler(PlayerView playerView, PlayerAnimationController playerAnimController)
    {
        _playerView = playerView;
        _playerAnimController = playerAnimController;
    }

    public void SetEnemyToControl(GameObject enemyObject)
    { 
        _enemyView = enemyObject.GetComponent<EnemyView>();
        _enemyAnimController = enemyObject.GetComponent<EnemyAnimationControler>();
    }

    public void StopPlayer() => _playerView.Animator.SetBool(PlayerState.IsRunning, false);

    public async Task PlayerHitsEnemyAnimation(bool enemyKilled)
    {
        bool playerAttacks = _playerAnimController.attackAnimationFinished;
        bool enemyGotHit = _enemyAnimController.deathAnimationFinished;
        //bool enemyGotHit = _enemyAnimController.gotHitAnimationFinished;

        while (!playerAttacks)// || !enemyGotHit)
        {
            await Task.Yield();
            playerAttacks = _playerAnimController.attackAnimationFinished;
            if (enemyKilled) 
                enemyGotHit = _enemyAnimController.deathAnimationFinished;
            //enemyGotHit = _enemyAnimController.gotHitAnimationFinished;
        }

        _enemyAnimController.ResetFlags();
        _playerAnimController.ResetFlags();
        _playerView.Animator.SetBool(PlayerState.IsAttacking, false);
        //_enemyView.Animator.SetBool(EnemyState.GotHit, false);
    }

    public async Task EnemyHitsPlayerAnimation(bool playerDefeated)
    {
        bool enemyAttacks = _enemyAnimController.attackAnimationFinished;
        bool playerGotHit = playerDefeated
            ? _playerAnimController.deathAnimationFinished
            : _playerAnimController.gotHitAnimationFinished;

        while (!enemyAttacks || !playerGotHit)
        {
            await Task.Yield();
            enemyAttacks = _enemyAnimController.attackAnimationFinished;
            playerGotHit = playerDefeated
            ? _playerAnimController.deathAnimationFinished
            : _playerAnimController.gotHitAnimationFinished;
        }

        _enemyAnimController.ResetFlags();
        _playerAnimController.ResetFlags();
        _playerView.Animator.SetBool(PlayerState.IsDefeated, false);
        _playerView.Animator.SetBool(PlayerState.GotHit, false);
        _enemyView.Animator.SetBool(EnemyState.IsAttacking, false);
    }

    public async Task EnemyAppearAnimation()
    {
        bool animationCompleted = _enemyAnimController.appearAnimationFinished;
        while (!animationCompleted)
        {
            await Task.Yield();
            animationCompleted = _enemyAnimController.appearAnimationFinished;
        }
        _enemyView.Animator.SetBool(EnemyState.IsReady, true);
    }

    public async Task EnemyDeathAnimation()
    {
        bool animationCompleted = _enemyAnimController.deathAnimationFinished;
        while (!animationCompleted)
        {
            await Task.Yield();
            animationCompleted = _enemyAnimController.deathAnimationFinished;
        }
    }
}

