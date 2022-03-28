using Level;
using GameCamera;
using UnityEngine;
using UnityEditor;
using System;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/LevelData", fileName = "LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private GameObject _level;
        [SerializeField] private CameraContainerView  _cameraContainer;
        [SerializeField] private CellProperties[] _cellsToVisitProperties;


        public CellProperties[] CellsToVisit => _cellsToVisitProperties;
        public CellView[] CellsViews => GetCellsViews();
        public CameraContainerView CameraContainer => GetCameraContainer();

        private CellView[] GetCellsViews()
        {
            if (_level == null)
            {
                var viewObject = FindObjectOfType<LevelView>();
                _level = viewObject.gameObject;
            }
            return _level.GetComponentsInChildren<CellView>();
        }

        private CameraContainerView GetCameraContainer()
        {
            if (_cameraContainer == null)
                _cameraContainer = FindObjectOfType<CameraContainerView>();
            return _cameraContainer;
        }
    }

    public enum CellStatus { ToSkip, ToVisit }
    public enum ContentType { Resource, Enemy }
    public enum ResouceType { Coins, Gems, ExtraRolls, Power }
}