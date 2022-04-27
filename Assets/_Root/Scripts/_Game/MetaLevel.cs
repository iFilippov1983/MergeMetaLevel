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
using Lofelt.NiceVibrations;

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
        private CameraContainerView _cameraView;

        private List<GameObject> _playerObjectChilds;
        private bool _isVisible;

        public Action OnFightEvent;
        public Action OnPowerUpgradeAvailableEvent;
        public Action<ResourceProperties> OnResourcePickupEvent;
        public Action<ResourceProperties, Vector3> OnResourcePickupExecute;
        public Action<int> OnLevelCompletionProgressEvent;

        public MetaLevel(GameData gameData, PlayerProfile playerProfile)
        {
            _gameData = gameData;
            _playerProfile = playerProfile;
            _cameraView = _gameData.LevelData.CameraContainerView;
            _playerHandler = new PlayerHandler(_gameData, _playerProfile);
            _levelViewHandler = new LevelViewHandler(_gameData.LevelData);
            _routeHandler = new LevelRouteLogicHandler(_gameData.LevelData.CellsToVisit);
            _cameraHandler = new CameraHandler(_cameraView, _playerHandler.PlayerView.transform);
            _animationHandler = new AnimationHandler(_playerHandler.PlayerView, _playerHandler.PlayerAnimController);
            _fightHandler = new FightEventHandler(_animationHandler, _playerProfile);
            _enemyHandler = new EnemyHandler(_gameData.EnemiesData, _animationHandler);
            _playerObjectChilds = new List<GameObject>();
            _isVisible = true;

            CashPlayerChildGameObjects();
            InitialCameraSwitch(true);
            _playerHandler.PlayerAnimController.TurnAroundActionEvent += _animationHandler.PlayerFacedStateChange;
        }

        public void Cleanup()
        {
            _playerHandler.PlayerAnimController.TurnAroundActionEvent -= _animationHandler.PlayerFacedStateChange;
        }

        public async Task MakePlayerMove(Action<bool> OnFightCompleteEvent, Action OnResourcePickupEvent = null)
        {
            _playerHandler.StopLookingAtCamera();
            int id = _playerProfile.Stats.CurrentCellID + 1;
            var route = _routeHandler.GetRouteIDsFrom(id);
            await MovePlayerBy(route, OnFightCompleteEvent, OnResourcePickupEvent);
        }

        public void TeleportPlayerToStart()
        {
            _playerHandler.PlayerAnimController.TurnAroundActionEvent -= _animationHandler.PlayerFacedStateChange;
            _playerHandler.DestroyPlayer();
            _playerProfile.Stats.CurrentCellID = 0;
            _playerHandler.InitPlayer();
            _cameraHandler = new CameraHandler(_gameData.LevelData.CameraContainerView, _playerHandler.PlayerView.transform);
            _animationHandler = new AnimationHandler(_playerHandler.PlayerView, _playerHandler.PlayerAnimController);
            _fightHandler = new FightEventHandler(_animationHandler, _playerProfile);
            _enemyHandler = new EnemyHandler(_gameData.EnemiesData, _animationHandler);
            _playerObjectChilds = new List<GameObject>();
            _isVisible = true;

            CashPlayerChildGameObjects();
            InitialCameraSwitch(false);
            _playerHandler.PlayerAnimController.TurnAroundActionEvent += _animationHandler.PlayerFacedStateChange;
        }

        public void HandlePlayerVisibility()
        {
            var layer = _isVisible
                ? LayerMask.NameToLayer(Literal.LayerName_IgnoreRaicast)
                : LayerMask.NameToLayer(Literal.LayerName_Default);

            foreach (var ch in _playerObjectChilds)
                ch.layer = layer;
            
            _isVisible = !_isVisible;
            if (_playerHandler.PlayerView.Weapon.gameObject != null)
                _playerHandler.PlayerView.Weapon.gameObject.SetActive(_isVisible);
        }

        public async Task PrepareAction(bool needToMove = true)
        {
            if (needToMove)
            {
                int number = GetRouteCellsCount();
                _cameraView.Dice.gameObject.SetActive(true);
                _cameraView.Dice.Animator.SetInteger(AnimParameter.DiceNumber, number);
                await Task.Delay(1000);
                _cameraView.Dice.gameObject.SetActive(false);
            }

            --_playerProfile.Stats.DiceRolls;
        }

        public async Task ApplyCellEvent(Action<bool> OnFightCompleteEvent, Action OnResourcePickupEvent = null)
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
                OnResourcePickupEvent?.Invoke();
                await ApplyResourcePickup((ResourceProperties)content);
            }
        }

        public void AnimatePlayerLevelUp() => _animationHandler.ActivateLevelUpParticle();

        private async void InitialCameraSwitch(bool gameStart)
        {
            var cellView = _levelViewHandler.GetCellViewWithId(_playerProfile.Stats.CurrentCellID);
            if(gameStart)
                await Task.Delay(1000);

            await HandleCameraSwitch(false, true, cellView.IdleCameraPosition);
        }

        private void CashPlayerChildGameObjects()
        {
            var playerTransform = _playerHandler.PlayerView.gameObject.transform;
            var childsCount = playerTransform.childCount;
            for (int i = 0; i < childsCount; i++)
            {
                _playerObjectChilds.Add(playerTransform.GetChild(i).gameObject);
            }
        }

        private void ExecuteResourcePickup(ResourceProperties resourceProperties)
        { 
            _cellView = _levelViewHandler.GetCellViewWithId(_playerProfile.Stats.CurrentCellID);
            var positionToSpawnEffect = _cellView.ResourcePickupEffectSpawnPoint.position;
            OnResourcePickupExecute?.Invoke(resourceProperties, positionToSpawnEffect);

            _playerHandler.SpawnPopupAbovePlayer(resourceProperties.Amount, PopupType.Resource);
        }

        private async Task ApplyResourcePickup(ResourceProperties resourceProperties)
        {
            _cellView = _levelViewHandler.GetCellViewWithId(_playerProfile.Stats.CurrentCellID);
            var effectObject = GameObject.Instantiate(resourceProperties.PickupEffectPrefab, _cellView.ResourcePickupEffectSpawnPoint.position, Quaternion.identity);
            var particleEffect = effectObject.GetComponent<ParticleSystem>();
            var sr = particleEffect.GetComponent<SpriteRenderer>();
            Debug.Log(sr != null);
            particleEffect.Play();
            _playerHandler.SpawnPopupAbovePlayer(resourceProperties.Amount, PopupType.Resource);
            OnResourcePickupEvent?.Invoke(resourceProperties);

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

            while (particleEffect.isPlaying)
                await Task.Yield();

            GameObject.Destroy(effectObject);
        }

        private async Task ApplyFight(EnemyProperties enemyProperties, Action<bool> OnFightCompleteEvent, bool fisrtFightOnThisCell = true)
        {
            Transform enemySpawnPoint = null;

            if (fisrtFightOnThisCell)
            {
                _cellView = _levelViewHandler.GetCellViewWithId(_playerProfile.Stats.CurrentCellID, true);
                enemySpawnPoint = _cellView.EnemySpawnPoint;
                _enemyHandler.InitializeEnemy(enemyProperties, enemySpawnPoint); //await?
            }

            _animationHandler.SetPlayerAwareState();
            await _cameraHandler.SwitchCamera(true, false, _cellView.FightCameraPosition);
            _playerHandler.PrepareToFight(_playerProfile.Stats.Power, _playerProfile.Stats.Health);
            _enemyHandler.InitHealthBar();
            OnFightEvent?.Invoke();
            await _fightHandler.ApplyFight(_playerHandler.OnGetHitEvent, _enemyHandler.OnGetHitEvent, _cameraHandler.ShakeCamera, enemyProperties);

            bool playerWins = _playerProfile.Stats.LastFightWinner;
            OnFightCompleteEvent?.Invoke(playerWins);
            await HandleFightResult(playerWins, enemyProperties);

            await HandleCameraSwitch(false, true, _cellView.IdleCameraPosition, playerWins);
        }

        private async Task MovePlayerBy(List<int> route, Action<bool> OnFightCompleteEvent, Action OnResourcePickupEvent = null)
        {
            CellProperties cellProps = null;
            int valueForMovesPopup = route.Count;
            CellView lastCelView = null;
            foreach (int id in route)
            {
                cellProps = _routeHandler.GetCellPropertyWhithId(id);
                bool brake = cellProps.Status.Equals(CellStatus.ToVisit);
                _playerHandler.PlayerView.NavMeshAgent.autoBraking = brake;

                bool first = id.Equals(route[0])
                    ? true : false;

                _playerHandler.SpawnPopupAbovePlayer(valueForMovesPopup, PopupType.Moves, first);
                _playerHandler.StopLookingAtCamera();
                await _animationHandler.SetPlayerRunState();

                var t = new List<Task>();
                var s = _cameraHandler.SwitchCamera(false, false);
                t.Add(s);
                Vector3 position = _levelViewHandler.GetCellPositionWithId(id);
                var move = _playerHandler.SetDestinationAndMove(position);
                t.Add(move);
                
                await Task.WhenAll(t);
                valueForMovesPopup--;

                ApplyCellPass(id);
                _playerProfile.Stats.CurrentCellID = id;
                OnLevelCompletionProgressEvent?.Invoke(id);
                
                lastCelView = _levelViewHandler.GetCellViewWithId(id);
            }

            var cellType = cellProps.ContentProperties.GetType();
            bool prepareToFight = cellType.Equals(typeof(EnemyProperties))
                ? true
                : false;
            bool gotResource = cellType.Equals(typeof(ResourceProperties))
                ? true
                : false;

            _animationHandler.StopPlayer(prepareToFight, gotResource);
            var camPosition = prepareToFight
                ? lastCelView.FightCameraPosition
                : lastCelView.IdleCameraPosition;

            var tasks = new List<Task>();
            var camSwitch = HandleCameraSwitch(prepareToFight, gotResource, camPosition, !prepareToFight);
            tasks.Add(camSwitch);
            var apply = ApplyCellEvent(OnFightCompleteEvent, OnResourcePickupEvent);
            tasks.Add(apply);

            await Task.WhenAll(tasks);
        }

        private void ApplyCellPass(int sellId)
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

            var cellView = _levelViewHandler.GetCellViewWithId(sellId);
            var passEffect = cellView.CellPassEffect;
            passEffect.gameObject.SetActive(true);
            passEffect.Play();
        }

        private int GetRouteCellsCount()
        {
            int id = _playerProfile.Stats.CurrentCellID + 1;
            return _routeHandler.GetRouteCountFrom(id);
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

                _animationHandler.SetPlayerIdleState(reward != null);

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

        private async Task HandleCameraSwitch(bool fightMode, bool idleMode, Transform transformToPlaceCamera, bool lookToCamera = true)
        {
            await _cameraHandler.SwitchCamera(fightMode, idleMode, transformToPlaceCamera);
            if (lookToCamera)
                await _playerHandler.LookAtCamera();
        }
    }
}
