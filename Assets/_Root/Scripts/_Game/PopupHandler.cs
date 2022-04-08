using UnityEngine;

namespace Game
{
    internal class PopupHandler
    {
        private GameObject _popupPrefab;
        private Transform _popupTransform;
        private PopupView _popup;
        private Camera _camera;

        public PopupHandler(GameObject popupPrefab, Camera camera)
        {
            _camera = camera;
            _popupPrefab = popupPrefab;
        }

        public void SpawnPopup(Vector3 position, int value)
        {
            _popupTransform = Object.Instantiate(_popupPrefab, position, _camera.transform.rotation).transform;
            _popup = _popupTransform.GetComponent<PopupView>();
            _popup.PopupText.SetText(value.ToString());
        }

        public void DestroyPopup()
        {
            _popup = null;
            Object.Destroy(_popupTransform.gameObject);
        }
    }
}
