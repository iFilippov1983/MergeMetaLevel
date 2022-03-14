using Profile;
using Data;
using Player;
using UnityEngine;
using UnityEngine.Analytics;

internal sealed class Root : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Transform _uiContainer;

    private LevelAPI _levelAPI;
    private PlayerAPI _playerAPI;

    public GameData GameData => _gameData;

    private void Awake()
    {
        _levelAPI = new LevelAPI(_gameData);
        _levelAPI.InitializeLevel();
        _playerAPI = new PlayerAPI(_gameData);
        _playerAPI.InitializePlayer();
    }

    //GetMovesCount
    //������������ ����� �� ������ Debug //await Task.Delay(1000)
    //Move<������ �� ������(Data)>
    //�������� ����-�� Debug
    //Apply

    private void OnDestroy()
    {
       
    }
}

