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


    private MetaLevel _metaLevel;
    private UIHandler _uiHandler;

    public GameData GameData => _gameData;

    private void Awake()
    {
        var playerProfile = new PlayerProfile(_initialCellID);
        _metaLevel = new MetaLevel(_gameData, playerProfile);
        _uiHandler = new UIHandler(_gameData.UIData, _uiContainer);

        _uiHandler.OnDiceRollClick += OnDiceRollClick;
    }

    private async void OnDiceRollClick()
    { 
        _uiHandler.DesactivateUI();

        int count = _metaLevel.GetRouteCellsCount();
        await _uiHandler.PlayDiceRollAnimation(count);
        await _metaLevel.MovePlayer();

        _uiHandler.ActivateUI();
    }

    //GetMovesCount
    //Отрисовываем число на кубике Debug //await Task.Delay(1000)
    //Move<данные от клетки(Data)>
    //Анимация чего-то Debug
    //Apply

    private void OnDestroy()
    {
        _uiHandler.OnDiceRollClick -= OnDiceRollClick;
    }
}

