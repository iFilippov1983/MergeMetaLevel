using Data;
using Player;
using Level;
using GameUI;
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
    public MetaLevel(GameData gameData , PlayerProfile playerProfile)
    {
        _gameData = gameData;
        _playerProfile = playerProfile;
        _levelViewHandler = new LevelViewHandler(_gameData.LevelData);
        _playerHandler = new PlayerHandler(_gameData, _playerProfile);
        _logicHandler = new LevelLogicHandler(_gameData.LevelData.CellsPropeties);

        _playerHandler.OnTargetPositionCall += _levelViewHandler.GetCellPositionWithID;
    }

    public int GetNuberOfRouteCells()
    {
        int id = _playerProfile.CurrentCellID.Value + 1;
        return _logicHandler.GetRouteCellsPropertiesFrom(id).Count;
    }

    public async void MovePlayer()
    {
        int id =_playerProfile.CurrentCellID.Value + 1;
        var route = _logicHandler.GetRouteCellsPropertiesFrom(id);
        _playerProfile.CurrentRoute.Value = route;

        await Task.Run(() => _playerHandler.hasArrived == true);
    }




    //private List<CellEntity> MakeEntitiesList(Dictionary<int, CellView> cellViews, CellProperties[] cellProperties)
    //{
    //    if (cellViews.Count != cellProperties.Length) return null;

    //    var entities = new List<CellEntity>();
    //    for (int index = 0; index < cellProperties.Length; index++)
    //    {
    //        var entity = new CellEntity(index, cellViews[index], cellProperties[index]);
    //        entities.Add(entity);
    //    }
    //    return entities;
    //}



    ///
    //public async Task<(CellProperties, CellView)> Move(int cellID)
    //{
    //    var properties = _metaLevel.CellEntities[cellID].Propeties;
    //    var view = _metaLevel.CellEntities[cellID].CellView;
    //    Apply();
    //    return (properties, view);
    //}

    //public async void Apply()
    //{
    //    await Task.Yield();
    //}

    //public Vector3 GetDestination(int cellID)
    //{
    //    return _metaLevel.CellEntities[cellID].Position;
    //}
}
