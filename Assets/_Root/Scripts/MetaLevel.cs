using Data;
using Game;
using Profile;
using UnityEngine;
using Tool;
using Level;
using System.Threading.Tasks;
using System;

internal class MetaLevel : BaseController
{
    private readonly Transform _uiContainer;
    private GameData _gameData;
    private LevelInitializer _levelInitializer;
    private GameController _gameController;

    public MetaLevel(GameData gameData)
    {
        _gameData = gameData;
        _uiContainer = _gameData.UiData.UIContainer;
        _levelInitializer = new LevelInitializer(_gameData.LevelData);
    }

    protected override void OnDispose()
    {
        _levelInitializer.Dispose();
        _gameController?.Dispose();
    }

}

internal class MetaLevelQ
{
    public void Fill() { }
    //public async Task<(CellProperties, CellView)> Move() { }

    public async Task Apply() { }
}



