using System;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tutorial.UI
{
    public class UiTutorialItem : MonoBehaviour, IPointerClickHandler
    {
        public string Guid;
        
        private Action _onClick;
        private bool _destroyOnClick;
        private bool _isClicked;
        private Button _button;


        public void Init(Action onClick, bool destroyOnClick = false)
        {
            this._onClick = onClick;
            this._destroyOnClick = destroyOnClick;
            _isClicked = false;
            _button = GetComponentInChildren<Button>();
            if (_button)
            {
                _button.onClick.AddListener(ClickButton);
            }
        }

        [Button]
        public void NewGuid()
        {
            this.Guid = System.Guid.NewGuid().ToString();
        }

        private void ClickButton()
        {
            _button.onClick.RemoveListener(ClickButton);
            OnPointerClick(null);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isClicked)
                return;
            
            _isClicked = true;
            _onClick?.Invoke();
            if (_destroyOnClick)
                DestroyImmediate(this);
        }
    }
}
