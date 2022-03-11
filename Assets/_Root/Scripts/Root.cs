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
    //������������ ����� �� ������ Debug //await Task.Delay(1000)
    //Move<������ �� ������(Data)>
    //�������� ����-�� Debug
    //Apply
    private void OnDestroy()
    {
        _metaLevel.Dispose();
    }
}

