using System;
using UnityEngine;
using Utils;

namespace Api.Map
{
    [Serializable]
    public class MouseDragHandler 
    {
        public event Action<Vector3> OnDragEvent;
        public event Action<float> OnZoomEvent;
        public event Action<GameObject> OnDoubleTapEvent;
        public event Action<GameObject> OnTapEvent;
        public event Action<GameObject> OnHoldStartEvent;
        public event Action<GameObject> OnHoldEndEvent;
        
        public DataWrapper<Vector3> drag = new DataWrapper<Vector3>();
        public DataWrapper<float> zoom = new DataWrapper<float>();
        public DataWrapper<GameObject> doubleTap = new DataWrapper<GameObject>();
        public DataWrapper<GameObject> tap = new DataWrapper<GameObject>();
        public DataWrapper<GameObject> holdStart = new DataWrapper<GameObject>();
        public DataWrapper<GameObject> holdEnd = new DataWrapper<GameObject>();
        
        

        public float DoubleTapTimeout = 0.4f;
        private Camera _camera;
        
        private float _doubleTapTime;
        // private Plane _plane = new Plane(Vector3.forward, Vector3.zero) ;
        private Plane _plane = new Plane(Vector3.up, Vector3.zero) ;


        private MouseState _None;
        private MouseState _Zoom;
        private MouseState _Hold;
        private MouseState _Drag;
        // private MouseState _DblTap ;

        private MouseState _state;
        private int _lastTapId;
        private Touch _touch;
        private bool _drawEnabled = false;
        private float _clickTime;
        private MouseState _oldState;
        private MouseState _newState;


        public MouseDragHandler(Camera camera)
        {
            _camera = camera;
            
            _None = new MouseState(MouseStateFlags.None, MouseStateFlags.Any, OnNoneStart);
            _Zoom = new MouseState(MouseStateFlags.Zoom, MouseStateFlags.Any, OnZoomStart);
            _Hold = new MouseState(MouseStateFlags.Hold, MouseStateFlags.None, OnHoldEnter, OnHoldExit);
            _Drag = new MouseState(MouseStateFlags.Drag, MouseStateFlags.None|MouseStateFlags.Zoom|MouseStateFlags.Hold, OnDragEnter);
            // _DblTap = new MouseState(StateFlags.Drag, StateFlags.None|StateFlags.Zoom|StateFlags.Hold);
            
            _state = _None;
        }

        public void ChangeStateTo(MouseState newState)
        {
            if(!newState.CanTransitFrom(_state.Id))
                return;
            if(newState == _state)
                return;

            _oldState = _state;
            _newState = newState;
            
            Debug.Log($"{_oldState.Id} >>> {newState.Id}");
            
            _state.OnExit();
            _state = newState;
            _state.OnEnter();
        }

        public void OnZoom(Vector2 firstPos, Vector2 secondPos, float diff)
        {
            ChangeStateTo(_Zoom);
            
            var color = diff switch 
            {
                0 => Color.white,
                _ when diff > 0 => Color.green,
                _ when diff < 0 => Color.yellow
            };

            if (_state == _Zoom)
            {
                zoom.data = diff;
                OnZoomEvent?.Invoke(diff);
                DrawLine(PlaneRaycast(firstPos), PlaneRaycast(secondPos), color);
            }
        }

        public void OnZoomENd()
        {
            ChangeStateTo(_Drag);
        }

        public void OnDown(Touch touch)
        {
            _touch = touch;
            ChangeStateTo(_Hold);
            _clickTime = Time.time + 0.3f;
            // DrawLine(PlaneRaycast(touch.position),
            // PlaneRaycast(touch.position) + new Vector3(0, 4f), Color.magenta, 2f);
        }

        public void OnUp(Touch touch)
        {
            _touch = touch;
            ChangeStateTo(_None);
            
            // DrawLine(PlaneRaycast(touch.position),
            //     PlaneRaycast(touch.position) + new Vector3(0, 5f), Color.cyan, 2f);
        }

        public void OnDrag(Touch touch)
        {
            // Debug.Log("DRAG");
            ChangeStateTo(_Drag);
            // if (touch.deltaPosition.sqrMagnitude < 0.01f)
            // return true;

            if (_state == _Drag)
            {
                drag.data = PlaneRaycast(Vector2.zero) - PlaneRaycast(touch.deltaPosition);
                OnDragEvent?.Invoke( drag.data );
                DrawLine(PlaneRaycast(touch.position), PlaneRaycast(touch.position) - new Vector3(2f, 2f), Color.green);
            }
        }

        private void OnNoneStart()
        {
            // OnDoubleTap(_touch);
        }

        private void OnZoomStart()
        {
            
        }

        private void OnTap(Touch touch)
        {
            if (Time.time > _clickTime)
                return;

            var go2 = GORaycast(touch.position);
            tap.data = go2;
            Invoke(go2, OnTapEvent);
        }
        
        private void OnDoubleTap(Touch touch)
        {
            if (Time.time > _doubleTapTime || _lastTapId != touch.fingerId)
            {
                _doubleTapTime = Time.time + DoubleTapTimeout;
                
                var go2 = GORaycast(touch.position);
                tap.data = go2;
                Invoke(go2, OnTapEvent);
                
                return;
            }

            _lastTapId = touch.fingerId;

            _doubleTapTime = Time.time + 0.4f;

            var go = GORaycast(touch.position);
            doubleTap.data = go;
            Invoke(go, OnDoubleTapEvent);
            
            DrawLine(PlaneRaycast(touch.position),
                PlaneRaycast(touch.position) + new Vector3(0, 6f), Color.yellow, 2f);
        }

        private void Invoke(GameObject go, Action<GameObject> action)
        {
            if(go != null)
                action?.Invoke(go);
        }


        private void OnHoldEnter()
        {
            Debug.Log(">> OnMouseDowm");
            var go = GORaycast(_touch.position);
            holdStart.data = go;
            Invoke(go, OnHoldStartEvent);
        }

        private void OnHoldExit()
        {
            Debug.Log(">> OnMouseUp or Drug");
            
            if(_newState.Id == MouseStateFlags.None)
                OnTap(_touch);
            
            var go = GORaycast(_touch.position);
            holdEnd.data = go;
            Invoke(go, OnHoldEndEvent);
        }

        private void OnDragEnter()
        {
            
        }

        private void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            if(_drawEnabled)
                Debug.DrawLine(start, end, color, duration);
        }

        private void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            if(_drawEnabled)
                Debug.DrawLine(start, end, color);
        }

        GameObject GORaycast(Vector2 inputPos)
        {
            var ray = _camera.ScreenPointToRay(inputPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
                return hit.collider.gameObject;
            
            return null;
        }
        
        Vector3 PlaneRaycast(Vector2 inputPos)
        {
            var ray = _camera.ScreenPointToRay(inputPos);
            if (_plane.Raycast(ray, out float entry))
                return ray.GetPoint(entry);

            return Vector3.zero;
        }

        public void OnUpdateStart()
        {
        	drag.hasData = false;
        	zoom.hasData = false;
        	doubleTap.hasData = false;
        	tap.hasData = false;
        	holdStart.hasData = false;
        	holdEnd.hasData = false;
        }
    }
}