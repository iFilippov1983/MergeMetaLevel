using Data;
using Player;
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
    private PlayerProfile _playerProfile;
    private LevelLogicHandler _logicHandler;
    private VirtualCameraHandler _virtualCameraHandler;

    public Action<EnemyProperties> OnFightEvent;
    public Action<ResourceProperties> OnResourcePickup;

    public MetaLevel(GameData gameData , PlayerProfile playerProfile)
    {
        _gameData = gameData;
        _playerProfile = playerProfile;
        _levelViewHandler = new LevelViewHandler(_gameData.LevelData);
        _playerHandler = new PlayerHandler(_gameData, _playerProfile.CurrentCellID);
        _logicHandler = new LevelLogicHandler(_gameData.LevelData.CellsToVisit);
        _virtualCameraHandler = new VirtualCameraHandler(_playerHandler.PlayerView.transform);
    }

    public int GetRouteCellsCount()
    {
        int id = _playerProfile.CurrentCellID + 1;
        return _logicHandler.GetRouteCountFrom(id);
        //return _logicHandler.GetRouteCellsPropertiesFrom(id).Count;
    }

    public async Task MovePlayer()
    {
        int id =_playerProfile.CurrentCellID + 1;
        //var route = _logicHandler.GetRouteCellsPropertiesFrom(id);
        var route = _logicHandler.GetRouteIDsFrom(id);
        await MovePlayerBy(route);

    }
    public async Task ApplyCellEvent()
    {
        CellProperties propertiesToApply = _logicHandler.GetCellPropertyWhithId(_playerProfile.CurrentCellID);
        ContentType type = propertiesToApply.ContentProperties.GetContentType();
        ContentProperties content;

        if (type.Equals(ContentType.Enemy))
        {
            content = propertiesToApply.ContentProperties;
            await ApplyFight((EnemyProperties)content);
        }
        if (type.Equals(ContentType.Resource))
        {
            content = propertiesToApply.ContentProperties;
            await ApplyResourcePickup((ResourceProperties)content);
        }
    }

    private async Task MovePlayerBy(List<int> route)
    {
        foreach (int id in route)
        {
            Vector3 position = _levelViewHandler.GetCellPositionWithId(id);
            await _playerHandler.SetDestinationAndMove(position);
            ApplyCellAnimation(id);
            _playerProfile.CurrentCellID = id;
        }
    }

    private async Task ApplyFight(EnemyProperties enemyProperties)
    {
        OnFightEvent?.Invoke(enemyProperties);
        await Task.Delay(1000);
    }

    private async Task ApplyResourcePickup(ResourceProperties resourceProperties)
    {
        OnResourcePickup?.Invoke(resourceProperties);
        await Task.Delay(1000);
    }

    //private async Task MovePlayerBy(List<CellProperties> route)
    //{
    //    foreach (CellProperties property in route)
    //    {
    //        Vector3 position = _levelViewHandler.GetCellPositionWithId(property.Id);
    //        await _playerHandler.SetDestinationAndMove(position);
    //        ApplyCellAnimation(property.Id);
    //        _playerProfile.CurrentCellID = property.Id;
    //    }
    //}

    private void ApplyCellAnimation(int sellId)
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
