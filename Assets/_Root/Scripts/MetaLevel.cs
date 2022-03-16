using Data;
using Player;
using Level;
using GameUI;
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

    public MetaLevel(GameData gameData , PlayerProfile playerProfile)
    {
        _gameData = gameData;
        _playerProfile = playerProfile;
        _levelViewHandler = new LevelViewHandler(_gameData.LevelData);
        _playerHandler = new PlayerHandler(_gameData, _playerProfile.CurrentCellID);
        _logicHandler = new LevelLogicHandler(_gameData.LevelData.CellsPropeties);
        _virtualCameraHandler = new VirtualCameraHandler(_playerHandler.PlayerView.transform);
    }

    public int GetRouteCellsCount()
    {
        int id = _playerProfile.CurrentCellID + 1;
        return _logicHandler.GetRouteCellsPropertiesFrom(id).Count;
    }

    public async Task MovePlayer()
    {
        int id =_playerProfile.CurrentCellID + 1;
        var route = _logicHandler.GetRouteCellsPropertiesFrom(id);

        await MovePlayerBy(route);
    }

    private async Task MovePlayerBy(List<CellProperties> route)
    {
        foreach (CellProperties property in route)
        {
            Vector3 position = _levelViewHandler.GetCellPositionWithID(property.Id);
            await _playerHandler.SetDestinationAndMove(position);
            ApplyCellAnimation(property.Id);
            _playerProfile.CurrentCellID = property.Id;
        }
    }

    private void ApplyCellAnimation(int sellId)
    {
        var cellView = _levelViewHandler.GetCellViewWithID(sellId);
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
