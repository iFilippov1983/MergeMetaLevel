using Level;
using GameCamera;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/LevelData", fileName = "LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private GameObject _levelCells;
        [SerializeField] private CameraContainerView  _cameraContainer;
        [SerializeField] private CellProperties[] _cellsToVisitProperties;


        public CellProperties[] CellsToVisit => _cellsToVisitProperties;
        public CellView[] CellsViews => GetCellsViews();
        public CameraContainerView CameraContainerView => GetCameraContainer();

        private CellView[] GetCellsViews()
        {
            if (_levelCells == null)
            {
                var viewObject = FindObjectOfType<CellsView>();
                _levelCells = viewObject.gameObject;
            }
            return _levelCells.GetComponentsInChildren<CellView>();
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
    public enum ResouceType { Gold, Gems, ExtraRolls, Power }
}