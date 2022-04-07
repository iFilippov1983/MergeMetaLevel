using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Api.Map
{
    [Serializable]
    public class MouseApi 
    {
        private Camera _camera;

        private Plane _plane = new Plane(Vector3.up, Vector3.zero) ;

        private IMouseApi _api;
        private MouseDragHandler _handler ;
        public MouseDragHandler Handler => _handler ;


        [Button] void Editor_UseFake()
        {
            _api = new FakeMouseApi();
            _handler = new MouseDragHandler(_camera);
        }

        [Button] void Editor_UseTouch()
        {
            _api = new TouchMouseApi();
            _handler = new MouseDragHandler(_camera);
        }

        public void SetCtx(Camera camera)
        {
            _camera = camera;
            // _api = new TouchMouseApi();
            _api = Application.platform == RuntimePlatform.WindowsEditor
                ? (IMouseApi)new FakeMouseApi()
                : (IMouseApi)new TouchMouseApi();
            _handler = new MouseDragHandler(camera);
        }

        public void OnUpdate()
        {
            _handler.OnUpdateStart();
            // DebugDraw();
            // return;
            var firstTouch = GetFirstTouch();
            var secondTouch = GetSecondTouch();
            // if(secondTouch != null && EventSystem.current.IsPointerOverGameObject(secondTouch.Value.fingerId))
                // return;
            
            if(ZoomEnd(firstTouch, secondTouch))
                return;
            if(Zoom(firstTouch, secondTouch))
                return;
            if(Up(firstTouch))
                return;
            if(Down(firstTouch))
                return; 
            if(Drag(firstTouch, secondTouch))
                return;
        }

        private bool AlreadyInited() => _api != null;

        private bool ZoomEnd(Touch? firstTouch, Touch? secondTouch)
        {
            if (firstTouch == null || secondTouch == null)
                return false;

            if (firstTouch.Value.phase != TouchPhase.Ended || secondTouch.Value.phase != TouchPhase.Ended)
                return false;
                
            _handler.OnZoomENd();
            return true;
        }

        private bool Down(Touch? firstTouch)
        {
            if (firstTouch == null || firstTouch.Value.phase != TouchPhase.Began)
                return false;

            if(_api.IsOverUi(firstTouch))
                return false;

            var touch = firstTouch.Value;
            _handler.OnDown(touch);

            return true;
        }

        private bool Up(Touch? firstTouch)
        {
            // if (!OldUp(firstTouch, secondTouch)) return false;

            if (firstTouch == null || firstTouch.Value.phase != TouchPhase.Ended)
                return false;

            _handler.OnUp(firstTouch.Value);
            return true;
        }
        
        private bool Drag(Touch? firstTouch, Touch? secondTouch)
        {
            var nullableTouch = firstTouch ?? secondTouch;
            if (nullableTouch == null)
                return false;

            var touch = nullableTouch.Value;
            if(touch.phase != TouchPhase.Moved )
                return false;

            if(_api.IsOverUi(firstTouch))
                return false;

            _handler.OnDrag(touch);

            return true;
        }
        
        private bool Zoom(Touch? firstTouch, Touch? secondTouch)
        {
            if (firstTouch == null || secondTouch == null )
                return false;
            // if(firstTouch.Value.phase != TouchPhase.Ended && secondTouch.Value.phase != TouchPhase.Ended)
                // return false;

            var touchOne = firstTouch.Value;
            var touchTwo = secondTouch.Value;
            var prevFirstPos = touchOne.position - touchOne.deltaPosition;
            var prevSecondPos = touchTwo.position - touchTwo.deltaPosition;
            var wasDiff = prevFirstPos - prevSecondPos;

            var firstPos = touchOne.position;
            var secondPos = touchTwo.position;

            var diff = firstPos - secondPos;

            var zoomIn = diff.sqrMagnitude > wasDiff.sqrMagnitude;
            var ratioDiff = (touchOne.deltaPosition.magnitude + touchTwo.deltaPosition.magnitude) / Screen.width;
            ratioDiff *= zoomIn ? -1 : 1;
            _handler.OnZoom(firstPos, secondPos, ratioDiff);
            
            return true;
        }

        // private bool OldUp(Touch? firstTouch, Touch? secondTouch)
        // {
        //     if (!OneNull_OneNotNull(firstTouch, secondTouch, out var notNullTouch))
        //         return false;
        //
        //     if (notNullTouch.Value.phase != TouchPhase.Ended) // || secondTouch.Value.phase == TouchPhase.Ended)
        //         return false;
        //     _handler.OnUp(notNullTouch.Value);
        //     return true;
        // }
        //
        // private static bool OneNull_OneNotNull(Touch? firstTouch, Touch? secondTouch, out Touch? notNullTouch)
        // {
        //     notNullTouch = (firstTouch ?? secondTouch);
        //     return (notNullTouch != null && (firstTouch == null || secondTouch == null));
        // }

        private void DebugDraw()
        {
            var firstTouch = GetFirstTouch();
            var secondTouch = GetSecondTouch();

            if (GetTouchCount() == 2)
                Debug.DrawLine(PlaneRaycast(firstTouch.Value.position), PlaneRaycast(secondTouch.Value.position), Color.yellow);

            if (firstTouch != null)
                Debug.DrawLine(PlaneRaycast(firstTouch.Value.position),
                    PlaneRaycast(firstTouch.Value.position) - new Vector3(0.4f, 0.4f), Color.green);
            if (secondTouch != null)
                Debug.DrawLine(PlaneRaycast(secondTouch.Value.position),
                    PlaneRaycast(secondTouch.Value.position) - new Vector3(0.4f, 0.4f), Color.red);
        }

        public Vector3 PlaneRaycast(Vector2 inputPos)
        {
            var ray = _camera.ScreenPointToRay(inputPos);
            if (_plane.Raycast(ray, out float entry))
                return ray.GetPoint(entry);

            return Vector3.zero;
        }

        private int GetTouchCount() => _api.GetTouchCount();

        private Touch? GetFirstTouch() => _api.GetFirstTouch();

        private Touch? GetSecondTouch() => _api.GetSecondTouch();
    }
}