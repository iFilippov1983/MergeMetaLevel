using UnityEngine;
using UnityEngine.EventSystems;

namespace Api.Map
{
    public class TouchMouseApi : IMouseApi
    {
        public Touch? GetFirstTouch() 
            => Input.touchCount > 0 ? Input.GetTouch(0) : (Touch?)null;

        public Touch? GetSecondTouch()
            => Input.touchCount > 1 ? Input.GetTouch(1) : (Touch?)null;

        public int GetTouchCount() => Input.touchCount;
        
        public bool IsOverUi(Touch? touch) 
            => touch != null && EventSystem.current.IsPointerOverGameObject(touch.Value.fingerId);
        public bool IsFirstOverUi() 
            => Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch (0).fingerId);

        public bool IsSecondOverUi() 
            => Input.touchCount > 1 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch (1).fingerId);
    }
}