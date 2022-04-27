using GameCamera;
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
        private CameraContainerView _cameraContainer;
        private Camera _camera;
        private string _text;
        private const float _damageFontSize = 2.5f;
        private const float _goldFontSize = 3f;
        private const float _movesFontSize = 4f;
        private readonly Color _damageColor = new Color(255, 0, 0);
        private readonly Color _pickupColor = new Color(250, 162, 0);
        private readonly Color _movesColor = new Color(0, 0, 0);
        private readonly Vector3 _firstPopupInitialScale = new Vector3(0.3f, 0.3f, 0.3f);
        private readonly Vector3 _defaultScale = new Vector3(1f, 1f, 1f);

        public PopupHandler(GameObject popupPrefab, Camera camera, CameraContainerView cameraContainerView = null)
        {
            _cameraContainer = cameraContainerView;
            _camera = camera;
            _popupPrefab = popupPrefab;
            _popupList = new List<PopupView>();
        }

        public async void SpawnPopup(Transform transformToSpawn, int value, PopupType popupType = PopupType.Damage, bool firstPopup = false)
        {
            Vector3 spawnPosition = firstPopup
                ? _cameraContainer.FirstPopupSpawnPoint.position
                : transformToSpawn.position;

            _popupTransform = Object.Instantiate(_popupPrefab, spawnPosition, _camera.transform.rotation).transform;

            if (firstPopup)
                _popupTransform.localScale = _firstPopupInitialScale;
            else
                _popupTransform.SetParent(transformToSpawn.transform, true);

            _popup = _popupTransform.GetComponent<PopupView>();

            if (popupType.Equals(PopupType.Damage))
            {
                SetupDamagePopup(value);
            }
            else if (popupType.Equals(PopupType.Resource))
            {
                SetupResourcePopup(value);
            }
            else
            {
                SetupMovesPopup(value);
            }

            _popupList.Add(_popup);
            _popup.PopupText.SetText(_text);
            _popup.Animation.Play();

            while (_popup != null && _popup.Animation.isPlaying)
            {
                _popup.transform.rotation = _camera.transform.rotation;

                if (firstPopup)
                { 
                    Vector3 position = Vector3.Lerp(_popup.transform.position, transformToSpawn.position, 0.03f);
                    _popupTransform.position = position;
                    Vector3 scale = Vector3.Lerp(_popup.transform.localScale, _defaultScale, 0.03f);
                    _popupTransform.localScale = scale;
                }
                    
                await Task.Yield();
            }
                
            DestroyPopups();
        }

        private void SetupDamagePopup(int value)
        {
            _text = string.Concat("-", value.ToString());
            _popup.PopupText.color = Color.clear;
            _popup.PopupText.color = _damageColor;
            _popup.PopupText.fontSize = _damageFontSize;
            _popup.Animation.clip = _popup.ClipFly;
            _popup.PopupText.alignment = TMPro.TextAlignmentOptions.Center;
        }

        private void SetupResourcePopup(int value)
        {
            _text = string.Concat("+", value.ToString());
            _popup.PopupText.color = Color.clear;
            _popup.PopupText.color = _pickupColor;
            _popup.PopupText.fontSize = _goldFontSize;
            _popup.Canvas.gameObject.SetActive(true);
            _popup.Animation.clip = _popup.ClipFly;
            _popup.PopupText.alignment = TMPro.TextAlignmentOptions.Right;
        }

        private void SetupMovesPopup(int value)
        {
            _text = value.ToString();
            _popup.PopupText.color = Color.clear;
            _popup.PopupText.color = _movesColor;
            _popup.PopupText.fontSize = _movesFontSize;
            _popup.Animation.clip = _popup.ClipHide;
            _popup.PopupText.alignment = TMPro.TextAlignmentOptions.Center;
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
