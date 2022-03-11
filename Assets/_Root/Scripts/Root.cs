using Profile;
using Data;
using UnityEngine;
using UnityEngine.Analytics;

internal sealed class Root : MonoBehaviour
{
    //private const GameState InitialState = GameState.Game;
    [SerializeField] private GameData _gameData;

    private MetaLevel _metaLevel;
    //private MetaLevel _mainController;

    public GameData GameData => _gameData;

    private void Awake()
    {
        _metaLevel = new MetaLevel(_gameData);
        //Fill
    }
    //GetMovesCount
    //Отрисовываем число на кубике Debug //await Task.Delay(1000)
    //Move<данные от клетки(Data)>
    //Анимация чего-то Debug
    //Apply
    private void OnDestroy()
    {
        _metaLevel.Dispose();
    }
}

