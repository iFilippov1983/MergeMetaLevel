using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    internal class PopupHandler
    {
        private GameObject _popupPrefab;
        private Transform _popupTransform;
        private PopupView _popup;
        private List<PopupView> _popupList;
        private Camera _camera;
        private const float _spawnOffsetMin = 2.1f;
        private const float _spawnOffsetMax = 2.3f;
        private const float _minFontSize = 2f;
        private const float _maxFontSize = 4f;
        private readonly Color _damegeColor = new Color(255, 0, 0);
        private readonly Color _pickupColor = new Color(242, 177, 0);

        public PopupHandler(GameObject popupPrefab, Camera camera)
        {
            _camera = camera;
            _popupPrefab = popupPrefab;
            _popupList = new List<PopupView>();
        }

        public async void SpawnPopup(Vector3 position, int value, bool resourcePickup = false)
        {
            float spawnOffset = Random.Range(_spawnOffsetMin, _spawnOffsetMax);
            position.y += spawnOffset;
            _popupTransform = Object.Instantiate(_popupPrefab, position, _camera.transform.rotation).transform;
            _popup = _popupTransform.GetComponent<PopupView>();
            _popupList.Add(_popup);

            string text;
            if (resourcePickup)
            {
                text = string.Concat("+", value.ToString());
                _popup.PopupText.color = _pickupColor;
                _popup.PopupText.fontSize = _maxFontSize;
            }
            else
            { 
                text = string.Concat("-", value.ToString());
                _popup.PopupText.color = _damegeColor;
                _popup.PopupText.fontSize = _minFontSize;
            }

            _popup.PopupText.SetText(text);
            _popup.Animation.Play();

            while (_popup != null && _popup.Animation.isPlaying)
                await Task.Yield();
            DestroyPopups();
        }

        private void DestroyPopups()
        {
            _popup = null;
            foreach (var popup in _popupList)
                if(popup != null)
                    Object.DestroyImmediate(popup.gameObject);
            _popupList.Clear();
            //Object.Destroy(_popupTransform.gameObject);
        }
    }
}
