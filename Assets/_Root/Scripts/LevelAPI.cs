using Data;
using Profile;
using UnityEngine;
using Tool;
using Level;
using System.Threading.Tasks;
using System;
using Player;

internal class LevelAPI
{
    private LevelData _levelData;
    private MetaLevel _metaLevel;

    public LevelAPI(GameData gameData)
    {
        _levelData = gameData.LevelData;
        _metaLevel = new MetaLevel(_levelData);
    }

    public void InitializeLevel()
    {
        _metaLevel.FillLevel();
    }

    public async Task<(CellProperties, CellView)> Move(int cellID) 
    {
        var properties = _metaLevel.CellEntities[cellID].Propeties;
        var view = _metaLevel.CellEntities[cellID].CellView;
        await Apply();
        return (properties, view);
    }

    public async Task Apply()
    {
       
    }
}




