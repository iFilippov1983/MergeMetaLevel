using Data;
using Player;
using Enemy;
using Level;
using GameCamera;
using UnityEngine;
using UnityEngine.Analytics;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Game
{
    internal sealed class MetaLevel
    {
        private GameData _gameData;
        private PlayerProfile _playerProfile;

        private LevelViewHandler _levelViewHandler;
        private AnimationHandler _animationHandler;
        private PlayerHandler _playerHandler;
        private FightEventHandler _fightHandler;
        private LevelRouteLogicHandler _routeHandler;
        private CameraHandler _cameraHandler;
        private EnemyHandler _enemyHandler;

        private EnemyProperties _lastEnemyProperties;
        private CellView _cellView;

        public Action OnFightEvent;
        public Action OnPowerUpgradeAvailableEvent;
        public Action<ResourceProperties> OnResourcePickupEvent;
        public Action<int> OnLevelCompletionProgressEvent;

        public MetaLevel(GameData gameData, PlayerProfile playerProfile)
        {
            _gameData = gameData;
            _playerProfile = playerProfile;
            _levelViewHandler = new LevelViewHandler(_gameData.LevelData);
            _playerHandler = new PlayerHandler(_gameData, _playerProfile);
            _routeHandler = new LevelRouteLogicHandler(_gameData.LevelData.CellsToVisit);
            _cameraHandler = new CameraHandler(_gameData.LevelData.CameraContainerView, _playerHandler.PlayerView.transform);
            _animationHandler = new AnimationHandler(_playerHandler.PlayerView, _playerHandler.PlayerAnimController);
            _fightHandler = new FightEventHandler(_animationHandler, _playerProfile);
            _enemyHandler = new EnemyHandler(_gameData.EnemiesData, _animationHandler);
        }

        public int GetRouteCellsCount()
        {
            int id = _playerProfile.Stats.CurrentCellID + 1;
            return _routeHandler.GetRouteCountFrom(id);
        }

        public async Task MovePlayer()
        {
            int id = _playerProfile.Stats.CurrentCellID + 1;
            var route = _routeHandler.GetRouteIDsFrom(id);
            await MovePlayerBy(route);
        }

        public void TeleportPlayerToStart()
        {
            _playerHandler.DestroyPlayer();
            _playerProfile.Stats.CurrentCellID = 0;
            _playerHandler.InitPlayer();
            _cameraHandler = new CameraHandler(_gameData.LevelData.CameraContainerView, _playerHandler.PlayerView.transform);
            _animationHandler = new AnimationHandler(_playerHandler.PlayerView, _playerHandler.PlayerAnimController);
            _fightHandler = new FightEventHandler(_animationHandler, _playerProfile);
            _enemyHandler = new EnemyHandler(_gameData.EnemiesData, _animationHandler);
        }

        public async Task ApplyCellEvent(Action<bool> OnFightCompleteEvent)
        {
            CellProperties propertiesToApply = _routeHandler.GetCellToVisitPropertyWhithId(_playerProfile.Stats.CurrentCellID);
            ContentType type = propertiesToApply.ContentProperties.GetContentType();
            ContentProperties content = propertiesToApply.ContentProperties;

            if (type.Equals(ContentType.Enemy))
            {
                if (_playerProfile.Stats.LastFightWinner || _lastEnemyProperties == null)
                {
                    _lastEnemyProperties = (EnemyProperties)content;
                    await ApplyFight(_lastEnemyProperties, OnFightCompleteEvent);
                }
                else
                {
                    await ApplyFight(_lastEnemyProperties, OnFightCompleteEvent, false);
                }
            }
            if (type.Equals(ContentType.Resource))
            {
                await ApplyResourcePickup((ResourceProperties)content);
            }
        }

        private async Task ApplyResourcePickup(ResourceProperties resourceProperties)
        {
            _cellView = _levelViewHandler.GetCellViewWithId(_playerProfile.Stats.CurrentCellID);
            var effectObject = GameObject.Instantiate(resourceProperties.PickupEffectPrefab, _cellView.ResourcePickupEffectSpawnPoint.position, Quaternion.identity);
            var particleEffect = effectObject.GetComponent<ParticleSystem>();
            particleEffect.Play();
            _playerHandler.SpawnPopupAbovePlayer(resourceProperties.Amount, PopupType.Resource);

            while (particleEffect.isPlaying)
                await Task.Yield();

            OnResourcePickupEvent?.Invoke(resourceProperties);
            GameObject.Destroy(effectObject);
        }

        private async Task ApplyFight(EnemyProperties enemyProperties, Action<bool> OnFightCompleteEvent, bool fisrtFightOnThisCell = true)
        {
            Transform enemySpawnPoint = null;

            if (fisrtFightOnThisCell)
            {
                _cellView = _levelViewHandler.GetCellViewWithId(_playerProfile.Stats.CurrentCellID, true);
                enemySpawnPoint = _cellView.EnemySpawnPoint;
                await _enemyHandler.InitializeEnemy(enemyProperties, enemySpawnPoint);
            }

            await _cameraHandler.SwitchCamera(_cellView.FightCameraPosition);
            _playerHandler.PrepareToFight(_playerProfile.Stats.Power, _playerProfile.Stats.Health);
            _enemyHandler.InitHealthBar();
            OnFightEvent?.Invoke();
            await _fightHandler.ApplyFight(_playerHandler.OnGetHitEvent, _enemyHandler.OnGetHitEvent, enemyProperties);

            bool playerWins = _playerProfile.Stats.LastFightWinner;
            await HandleFightResult(playerWins, enemyProperties);
            OnFightCompleteEvent?.Invoke(playerWins);

            await _cameraHandler.SwitchCamera();
        }

        private async Task MovePlayerBy(List<int> route)
        {
            int valueForMovesPopup = route.Count;
            foreach (int id in route)
            {
                var cellProps = _routeHandler.GetCellPropertyWhithId(id);
                bool brake = cellProps.Status.Equals(CellStatus.ToVisit);
                _playerHandler.PlayerView.NavMeshAgent.autoBraking = brake;

                _playerHandler.SpawnPopupAbovePlayer(valueForMovesPopup, PopupType.Moves);
                Vector3 position = _levelViewHandler.GetCellPositionWithId(id);
                await _playerHandler.SetDestinationAndMove(position);
                valueForMovesPopup--;

                ApplyCellPass(id);
                _playerProfile.Stats.CurrentCellID = id;
                OnLevelCompletionProgressEvent?.Invoke(id);
            }

            _animationHandler.StopPlayer();
        }

        private void ApplyCellPass(int sellId)
        {
            var cellView = _levelViewHandler.GetCellViewWithId(sellId);

            var passEffect = cellView.CellPassEffect;
            passEffect.gameObject.SetActive(true);
            passEffect.Play();
        }

        private async Task HandleFightResult(bool playerWins, EnemyProperties enemyProperties)
        {
            if (playerWins)
            {
                _enemyHandler.OnFightFinishEvent(playerWins);
                var reward = enemyProperties.Reward;
                var tasks = new List<Task>();
                foreach (var r in reward)
                    tasks.Add(ApplyResourcePickup(r));
                await Task.WhenAll(tasks);

                await Task.Delay(100);//ui events
            }
            else
            {
                _enemyHandler.OnFightFinishEvent(playerWins);
                _playerProfile.Stats.LastFightWinner = false;
                _playerProfile.Stats.PowerUpgradeAvailable = true;
                OnPowerUpgradeAvailableEvent?.Invoke();
                await Task.Delay(100);//ui events
            }

            _playerHandler.FinishFight();
        }

        public void HandlePlayerActivity()
        {
            var state = _playerHandler.PlayerView.gameObject.activeSelf
                ? false
                : true;
            _playerHandler.PlayerView.gameObject.SetActive(state);
        }
    }
}
