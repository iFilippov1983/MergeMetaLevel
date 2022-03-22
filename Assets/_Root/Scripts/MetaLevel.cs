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


internal sealed class MetaLevel 
{
    private GameData _gameData;
    private LevelViewHandler _levelViewHandler;
    private PlayerHandler _playerHandler;
    private FightEventHandler _fightHandler;
    private PlayerProfile _playerProfile;
    private LevelRouteLogicHandler _routeHandler;
    private VirtualCameraHandler _cameraHandler;
    private EnemyProperties _lastEnemyProperties;

    public Action<EnemyProperties> OnFightEvent;
    public Action<ResourceProperties> OnResourcePickupEvent;

    public MetaLevel(GameData gameData , PlayerProfile playerProfile)
    {
        _gameData = gameData;
        _playerProfile = playerProfile;
        _levelViewHandler = new LevelViewHandler(_gameData.LevelData);
        _playerHandler = new PlayerHandler(_gameData, _playerProfile.Stats.CurrentCellID);
        _fightHandler = new FightEventHandler(_gameData.EnemiesData, _playerProfile);
        _routeHandler = new LevelRouteLogicHandler(_gameData.LevelData.CellsToVisit);
        _cameraHandler = new VirtualCameraHandler(_playerHandler.PlayerView.transform);
    }

    public int GetRouteCellsCount()
    {
        int id = _playerProfile.Stats.CurrentCellID + 1;
        return _routeHandler.GetRouteCountFrom(id);
    }

    public async Task MovePlayer()
    {
        int id =_playerProfile.Stats.CurrentCellID + 1;
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
        }

        await _fightHandler.ApplyFight(enemyProperties, OnFightEvent, enemySpawnPoint, enemyFightPoint, fisrtFightOnThisCell);
        bool playerWins = _playerProfile.Stats.LastFightWinner;
        await HandleFightResult(playerWins);
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
            bool brake = cellProps.Status.Equals(CellStatus.ToVisit) ? true : false;
            _playerHandler.PlayerView.NavMeshAgent.autoBraking = brake;

            Vector3 position = _levelViewHandler.GetCellPositionWithId(id);
            await _playerHandler.SetDestinationAndMove(position);

            ApplyCellPass(id);
            _playerProfile.Stats.CurrentCellID = id;
        }
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

    private async Task HandleFightResult(bool playerWins)
    {
        if (playerWins)
        {
            await _fightHandler.DestroyEnemy();
        }
        else
        {
            _playerProfile.Stats.LastFightWinner = false;
            //ui events
        }
        
    }
}
