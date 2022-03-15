using Data;
using UnityEngine;
using Tool;
using Level;
using System.Threading.Tasks;
using System;
using Player;
using System.Collections.Generic;
using GameUI;

internal class LevelAPI
{
    private GameData _gameData;
    private PlayerProfile _playerProfile;
    private MetaLevel _metaLevel;
    private UIHandler _uiHandler;
    private Transform _uiContainer;

    private Action OnDiceRollCall;

    public LevelAPI(GameData gameData, PlayerProfile gameProfile, Transform uiContainer)
    {
        _gameData = gameData;
        _playerProfile = gameProfile;
        _uiContainer = uiContainer;
        _metaLevel = new MetaLevel(_gameData.LevelData);
        _uiHandler = new UIHandler(_gameData.UIData, OnDiceRollCall, _uiContainer);

        _uiHandler.NumberForDiceCall += () => _metaLevel.GetRouteCellsFrom(_playerProfile.CurrentCellID.Value + 1).Count;
    }




    public async Task<(CellProperties, CellView)> Move(int cellID)
    {
        var properties = _metaLevel.CellEntities[cellID].Propeties;
        var view = _metaLevel.CellEntities[cellID].CellView;
        Apply();
        return (properties, view);
    }

    public async void Apply()
    {
        await Task.Yield();
    }

    public Vector3 GetDestination(int cellID)
    {
        return _metaLevel.CellEntities[cellID].Position;
    }
}




