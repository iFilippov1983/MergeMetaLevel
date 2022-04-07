using UnityEngine;
using UnityEngine.EventSystems;

namespace Api.Map
{
    public class FakeMouseApi : IMouseApi
    {
        private static Vector2 _prevTouch0Pos;
        private static Vector2 _prevTouch1Pos = new Vector2(Screen.width/2, Screen.height/2);

        public Touch? GetFirstTouch()
        {
            var prevPos = _prevTouch0Pos;
            var pos = (Vector2)Input.mousePosition;
            _prevTouch0Pos = pos;
            return FakeTouch(0, pos, prevPos);
        }

        public Touch? GetSecondTouch()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // SecondTouch.position = PlaneRaycast(Input.mousePosition);
                _prevTouch1Pos = Input.mousePosition;
            } 
                
            return FakeTouch(1, _prevTouch1Pos, _prevTouch1Pos);
        }

        public int GetTouchCount() 
            => GetFirstTouch() == null ? 0 : GetSecondTouch() == null ? 1 : 2;

        public bool IsOverUi(Touch? touch) => EventSystem.current.IsPointerOverGameObject();
        public bool IsFirstOverUi() => EventSystem.current.IsPointerOverGameObject();

        public bool IsSecondOverUi() => EventSystem.current.IsPointerOverGameObject();

        private static Touch? FakeTouch(int index, Vector2 mousePos, Vector2 prevMousePos)
        {
            var phase = GetPhase(index, mousePos, prevMousePos);
            if (phase == TouchPhase.Canceled)
                return null;

            return new Touch()
            {
                position = mousePos,
                phase = phase,
                deltaPosition = mousePos - prevMousePos,
            };
        }

        private static TouchPhase GetPhase(int index, Vector2 mousePos, Vector2 prevMousePos)
        {
            if (Input.GetMouseButtonDown(index))
                return TouchPhase.Began;
            if(Input.GetMouseButtonUp(index))
                return TouchPhase.Ended;
            if(Input.GetMouseButton(index))
                return Vector2.SqrMagnitude(mousePos-prevMousePos) < 0.01f ? TouchPhase.Stationary : TouchPhase.Moved;
            
            return TouchPhase.Canceled;
        }
    }
}