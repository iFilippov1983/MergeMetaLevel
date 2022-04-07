using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Components
{
    public class UiTutorialMouseListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool _MouseDown;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _MouseDown = true;
            Debug.Log("MouseDown");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _MouseDown = false;
            Debug.Log("MouseUp");
        }

        private void Update()
        {
            if(_MouseDown)
                Debug.Log("MouseDrg");
        }

    }
}