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
    private EnemiesHandler _enemiesHandler;
    private PlayerProfile _playerProfile;
    private LevelRouteLogicHandler _routeHandler;
    private VirtualCameraHandler _cameraHandler;

    public Action<EnemyProperties> OnFightEvent;
    public Action<ResourceProperties> OnResourcePickup;
    public Action<bool> OnFightComplete;

    public MetaLevel(GameData gameData , PlayerProfile playerProfile)
    {
        _gameData = gameData;
        _playerProfile = playerProfile;
        _levelViewHandler = new LevelViewHandler(_gameData.LevelData);
        _playerHandler = new PlayerHandler(_gameData, _playerProfile.Stats.CurrentCellID);
        _enemiesHandler = new EnemiesHandler(_gameData.EnemiesData, _playerProfile);
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

    public async Task ApplyCellEvent()
    {
        CellProperties propertiesToApply = _routeHandler.GetCellPropertyWhithId(_playerProfile.Stats.CurrentCellID);
        ContentType type = propertiesToApply.ContentProperties.GetContentType();
        ContentProperties content = propertiesToApply.ContentProperties;

        if (type.Equals(ContentType.Enemy))
        {
            await ApplyFight((EnemyProperties)content);
        }
        if (type.Equals(ContentType.Resource))
        {
            await ApplyResourcePickup((ResourceProperties)content);
        }
    }

    private async Task ApplyFight(EnemyProperties enemyProperties)
    {
        var cell = _levelViewHandler.GetCellViewWithId(_playerProfile.Stats.CurrentCellID, true);
        var enemySpawnPoint = cell.EnemySpawnPoint;
        var enemyFightPoint = cell.EnemyFightPoint;

        await _enemiesHandler.FightEvent(enemyProperties, enemySpawnPoint, enemyFightPoint, OnFightEvent);
        bool playerWins = _playerProfile.Stats.LastFightWinner;
        OnFightComplete?.Invoke(playerWins);
    }

    private async Task ApplyResourcePickup(ResourceProperties resourceProperties)
    {
        await Task.Delay(1000);//resource pickup animation
        OnResourcePickup?.Invoke(resourceProperties);
    }

    private async Task MovePlayerBy(List<int> route)
    {
        foreach (int id in route)
        {
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
        //while (cellView.ParticleSystem.isPlaying)
        //    await Task.Yield();
    }
}
