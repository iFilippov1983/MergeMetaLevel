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
        private const float _damageFontSize = 2.5f;
        private const float _goldFontSize = 3f;
        private const float _movesFontSize = 4f;
        private readonly Color _damageColor = new Color(255, 0, 0);
        private readonly Color _pickupColor = new Color(250, 162, 0);
        private readonly Color _movesColor = new Color(0, 64, 250);

        public PopupHandler(GameObject popupPrefab, Camera camera)
        {
            _camera = camera;
            _popupPrefab = popupPrefab;
            _popupList = new List<PopupView>();
        }

        public async void SpawnPopup(Transform transformToSpawn, int value, PopupType popupType = PopupType.Damage)
        {
            _popupTransform = Object.Instantiate(_popupPrefab, transformToSpawn.position, _camera.transform.rotation).transform;
            _popupTransform.SetParent(transformToSpawn.transform, true);
            _popup = _popupTransform.GetComponent<PopupView>();

            string text;
            if (popupType.Equals(PopupType.Damage))
            {
                text = string.Concat("-", value.ToString());
                _popup.PopupText.color = Color.clear;
                _popup.PopupText.color = _damageColor;
                _popup.PopupText.fontSize = _damageFontSize;
                _popup.Animation.clip = _popup.ClipFly;
                _popup.PopupText.alignment = TMPro.TextAlignmentOptions.Center;
            }
            else if (popupType.Equals(PopupType.Resource))
            {
                text = string.Concat("+", value.ToString());
                _popup.PopupText.color = Color.clear;
                _popup.PopupText.color = _pickupColor;
                _popup.PopupText.fontSize = _goldFontSize;
                _popup.Canvas.gameObject.SetActive(true);
                _popup.Animation.clip = _popup.ClipFly;
                _popup.PopupText.alignment = TMPro.TextAlignmentOptions.Right;
            }
            else
            {
                text = value.ToString();
                _popup.PopupText.color = Color.clear;
                _popup.PopupText.color = _movesColor;
                _popup.PopupText.fontSize = _movesFontSize; 
                _popup.Animation.clip = _popup.ClipHide;
                _popup.PopupText.alignment = TMPro.TextAlignmentOptions.Center;
            }

            _popupList.Add(_popup);
            _popup.PopupText.SetText(text);
            _popup.Animation.Play();

            while (_popup != null && _popup.Animation.isPlaying)
            {
                _popup.transform.rotation = _camera.transform.rotation;
                await Task.Yield();
            }
                
            DestroyPopups();
        }

        private void DestroyPopups()
        {
            _popup = null;
            foreach (var popup in _popupList)
                if(popup != null)
                    Object.Destroy(popup.gameObject);
            _popupList.Clear();
        }
    }
}
