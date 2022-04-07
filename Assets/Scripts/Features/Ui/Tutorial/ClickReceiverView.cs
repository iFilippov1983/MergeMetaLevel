using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Components
{
    public class ClickReceiverView : MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public Action OnMouseDown;
        public Action OnMouseUp;
        public Action OnMouseClick;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
            OnMouseDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp");
            OnMouseUp?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");
            OnMouseClick?.Invoke();
        }
    }
}