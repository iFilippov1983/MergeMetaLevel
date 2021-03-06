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
    internal sealed class MetaLevel : IDisposable
    {
        private GameData _gameData;
        private LevelViewHandler _levelViewHandler;
        private AnimationHandler _animationHandler;
        private PlayerHandler _playerHandler;
        private FightEventHandler _fightHandler;
        private PlayerProfile _playerProfile;
        private LevelRouteLogicHandler _routeHandler;
        private VirtualCameraHandler _cameraHandler;
        private EnemyHandler _enemyHandler;
        private EnemyProperties _lastEnemyProperties;

        public Action<EnemyProperties> OnFightEvent;
        public Action<ResourceProperties> OnResourcePickupEvent;

        public MetaLevel(GameData gameData, PlayerProfile playerProfile)
        {
            _gameData = gameData;
            _playerProfile = playerProfile;
            _levelViewHandler = new LevelViewHandler(_gameData.LevelData);
            _playerHandler = new PlayerHandler(_gameData, _playerProfile.Stats.CurrentCellID);
            _routeHandler = new LevelRouteLogicHandler(_gameData.LevelData.CellsToVisit);
            _cameraHandler = new VirtualCameraHandler(_playerHandler.PlayerView.transform);
            _animationHandler = new AnimationHandler(_playerHandler.PlayerView, _playerHandler.PlayerAnimController);
            _fightHandler = new FightEventHandler(_gameData.EnemiesData, _animationHandler, _playerProfile);
            _enemyHandler = new EnemyHandler(_gameData.EnemiesData, _animationHandler);

            SubscribeEntyties();
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

        public async Task ApplyCellEvent(Action<bool> OnFightCompleteEvent)
        {
            CellProperties propertiesToApply = _routeHandler.GetCellToVisitPropertyWhithId(_playerProfile.Stats.CurrentCellID);
            ContentType type = propertiesToApply.ContentProperties.GetContentType();
            ContentProperties content = propertiesToApply.ContentProperties;

            if (type.Equals(ContentType.Enemy))
            {
                if (_playerProfile.Stats.LastFightWinner)
                {
                    _lastEnemyProperties = (EnemyProperties)content;
                    await ApplyFight(_lastEnemyProperties);
                    OnFightCompleteEvent?.Invoke(_playerProfile.Stats.LastFightWinner);
                }
                else
                {
                    await ApplyFight(_lastEnemyProperties, false);
                    OnFightCompleteEvent?.Invoke(_playerProfile.Stats.LastFightWinner);
                }
            }
            if (type.Equals(ContentType.Resource))
            {
                await ApplyResourcePickup((ResourceProperties)content);
            }
        }

        private async Task ApplyFight(EnemyProperties enemyProperties, bool fisrtFightOnThisCell = true)
        {
            Transform enemySpawnPoint = null;
            Transform enemyFightPoint = null;

            if (fisrtFightOnThisCell)
            {
                var cell = _levelViewHandler.GetCellViewWithId(_playerProfile.Stats.CurrentCellID, true);
                enemySpawnPoint = cell.EnemySpawnPoint;
                enemyFightPoint = cell.EnemyFightPoint;
                await _enemyHandler.InitializeEnemy(enemyProperties, enemySpawnPoint, enemyFightPoint);
            }

            _playerHandler.PrepareToFight(_playerProfile.Stats.Power, _playerProfile.Stats.Health);
            await _fightHandler.ApplyFight(enemyProperties, OnFightEvent);//, enemySpawnPoint, enemyFightPoint, fisrtFightOnThisCell);
            bool playerWins = _playerProfile.Stats.LastFightWinner;
            await HandleFightResult(playerWins, enemyProperties);
        }

        private async Task ApplyResourcePickup(ResourceProperties resourceProperties)
        {
            await Task.Delay(1000);//resource pickup animation
            OnResourcePickupEvent?.Invoke(resourceProperties);
        }

        private async Task MovePlayerBy(List<int> route)
        {
            foreach (int id in route)
            {
                var cellProps = _routeHandler.GetCellPropertyWhithId(id);
                bool brake = cellProps.Status.Equals(CellStatus.ToVisit);
                _playerHandler.PlayerView.NavMeshAgent.autoBraking = brake;

                Vector3 position = _levelViewHandler.GetCellPositionWithId(id);
                await _playerHandler.SetDestinationAndMove(position);

                ApplyCellPass(id);
                _playerProfile.Stats.CurrentCellID = id;
            }

            _animationHandler.StopPlayer();
        }

        private void ApplyCellPass(int sellId)
        {
            var cellView = _levelViewHandler.GetCellViewWithId(sellId);
            var sRenderer = cellView.SpriteRenderer;
            sRenderer.gameObject.SetActive(true);

            var tmp = cellView.TextMeshPro;
            tmp.gameObject.SetActive(false);

            var pSystem = cellView.ParticleSystem;
            pSystem.gameObject.SetActive(true);
            pSystem.Play();
        }

        private async Task HandleFightResult(bool playerWins, EnemyProperties enemyProperties)
        {
            if (playerWins)
            {
                var reward = enemyProperties.Reward;
                var tasks = new List<Task>();
                foreach (var r in reward)
                    tasks.Add(ApplyResourcePickup(r));
                await Task.WhenAll(tasks);

                await Task.Delay(100);//ui events
            }
            else
            {
                _playerProfile.Stats.LastFightWinner = false;
                await Task.Delay(100);//ui events
            }

            _playerHandler.FinishFight();
        }

        private void SubscribeEntyties()
        {
            _fightHandler.OnPlayerHitsEnemy += _playerHandler.InitHit;
            _fightHandler.OnPlayerHitsEnemy += _enemyHandler.InitGotHit;
            _fightHandler.OnEnemyHitsPlayer += _playerHandler.InitGotHit;
            _fightHandler.OnEnemyHitsPlayer += _enemyHandler.InitHit;
        }

        public void Dispose()
        {
            _fightHandler.OnPlayerHitsEnemy -= _playerHandler.InitHit;
            _fightHandler.OnPlayerHitsEnemy -= _enemyHandler.InitGotHit;
            _fightHandler.OnEnemyHitsPlayer -= _playerHandler.InitGotHit;
            _fightHandler.OnEnemyHitsPlayer -= _enemyHandler.InitHit;
        }
    }
}
