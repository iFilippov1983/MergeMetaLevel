using Data;
using Player;
using GameUI;
using UnityEngine;
using UnityEngine.Analytics;
using System;
using System.Threading.Tasks;

internal sealed class Root : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Transform _uiContainer;
    [SerializeField] private int _initialCellID;

    private LevelAPI _levelAPI;
    private PlayerAPI _playerAPI;

    public GameData GameData => _gameData;

    private void Awake()
    {
        var playerProfile = new PlayerProfile(_initialCellID);
        _levelAPI = new LevelAPI(_gameData, playerProfile, _uiContainer);
        _playerAPI = new PlayerAPI(_gameData, playerProfile);
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

