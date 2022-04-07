using System;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Api.Merge
{
    enum MouseDragState
    {
        None,
        StartDrag,
        DoDrag,
        EndDrag
    }
    
    [Serializable]
    public class MergeMouseDragApi 
    {
        public Camera Camera;
        public Transform CameraTransform;

        // public Action<MergeMouseDragApi> OnClick; 
        public Action<MergeMouseDragApi> OnMouseDown; 
        public Action<MergeMouseDragApi> OnDrag; 
        public Action<MergeMouseDragApi> OnMouseUp; 
        public Action<float> OnZoom;
        
        public Vector2 StartDragPos = Vector2.zero;
        public Vector2 LastPos = Vector2.zero;
        public Vector2 Pos = Vector2.zero;
        public Vector2 Delta = Vector2.zero;

        public Vector2 WorldPos => Camera.ScreenToWorldPoint(Pos);
        public Vector2 WorldStartPos => Camera.ScreenToWorldPoint(StartDragPos);

        private MouseDragState _dragState;  
        private bool _wasMouseDown;
        private bool _wasZoom;
        private readonly Contexts _contexts;
        private readonly MergeDynamicData.InputDynamicData _data;
        private Vector2 _lastMousePos;

        public MergeMouseDragApi(Contexts contexts, Camera camera, Transform cameraTransform)
        {
            Camera = camera;
            CameraTransform = cameraTransform;
            _contexts = contexts;

            _data = _contexts.game.ctx.dynamicData.Input;
        }
        
        public void OnUpdate()
        {
            if (_data.MouseDown)
                ProcessStartDrag();
            
            else if (_data.MouseUp)
                ProcessStopDrag();
            
            else if (_data.MousePressed)
                ProcessDoDrag();

            _data.MouseDown = false;
            _data.MouseUp = false;
            // _data.MousePressed = false;
        }

        private bool ProcessDoDrag()
        {
            if (!_wasMouseDown)
                return true;

            _lastMousePos = Pos;

            _dragState = MouseDragState.DoDrag;
            Pos = Input.mousePosition;
            Delta = LastPos - Pos;
            // if( (_lastMousePos - Pos).sqrMagnitude < 0.5f*0.5f)
            if (!_wasZoom)
                OnDrag?.Invoke(this);    
            LastPos = Pos;
            return false;
        }

        private void ProcessStopDrag()
        {
            _wasZoom = false;
            _wasMouseDown = false;
            _dragState = MouseDragState.None;
            OnMouseUp?.Invoke(this);
        }

        private void ProcessStartDrag()
        {
            _wasMouseDown = true;

            _dragState = MouseDragState.StartDrag;
            StartDragPos = Input.mousePosition;
            LastPos = Input.mousePosition;
            Pos = Input.mousePosition;
            Delta = Vector2.zero;
            OnMouseDown?.Invoke(this);
        }

//
//
//         [HorizontalGroup("0")]
//         public float mag0;
//         [HorizontalGroup("0")][HideLabel]
//         public Vector2 start0;
//         [HorizontalGroup("0")][HideLabel]
//         public Vector2 pos0;
//  
//         [HorizontalGroup("1")]
//         public float mag1;
//         [HorizontalGroup("1")][HideLabel]
//         public Vector2 start1;
//         [HorizontalGroup("1")][HideLabel]
//         public Vector2 pos1;
//         
//         [HorizontalGroup("Cam")]
//         public float ZoomModifier;
//         [HorizontalGroup("Cam2")][LabelWidth(120)]
//         public  float _cameraStartSize;
//         [HorizontalGroup("Cam2")][LabelWidth(120)]
//         public  float _cameraSizeOffset;
//         [HorizontalGroup("Cam2")][LabelWidth(120)]
//         public  float _newCameraSize;
//         
//         [HorizontalGroup("Dist")][LabelWidth(120)]
//         private float _prevDist;
//         [HorizontalGroup("Dist")][LabelWidth(120)]
//         private float _dist;
//
//         // public Action<Vector3> OnMouseDownCb;
//         // public Action<Vector3> OnMouseDownCb;
//
//         private InputAction Touch0 => _inputMap.Input.Touch0;
//         private InputAction Pos0 => _inputMap.Input.Pos0;
//         private InputAction StartPos0 => _inputMap.Input.StartPos0;
//         private void Awake()
//         {
//             _inputMap = new InputMap();
//             // _inputMap.Input.Touch0.started += OnClickStart;
//             // _inputMap.Input.Touch0.canceled += OnClickEnd;
//             // _inputMap.Input.Touch0.started += _ => Debug.Log("started");
//             // _inputMap.Input.Touch0.canceled += _ => Debug.Log("canceled");
//             // _inputMap.Input.Touch0.performed += _ => Debug.Log("performed");
//             
//             _inputMap.Input.Touch0.started += OnClickStart;
//             _inputMap.Input.Touch0.canceled += OnClickEnd;
//             _inputMap.Input.Touch1.started += OnZoomStart;
//             _inputMap.Input.Touch1.canceled += OnZoomEnd;
//             // _inputMap.Input.Touch0.started += _ => Debug.Log($"Click Start {EventSystem.current.IsPointerOverGameObject()} { EventSystem.current.currentSelectedGameObject == null}");
//             // _inputMap.Input.Touch0.canceled += _ => Debug.Log($"Click End {EventSystem.current.IsPointerOverGameObject()} { EventSystem.current.currentSelectedGameObject == null}");
//             // _inputMap.Input.Touch0.performed += _ => Debug.Log($"Click Perform {EventSystem.current.IsPointerOverGameObject()} { EventSystem.current.currentSelectedGameObject == null}");
//             // _inputMap.Input.Touch1.started += OnZoomStart;
//             // _inputMap.Input.Touch1.canceled += OnZoomEnd;
//             
//             _inputMap.Enable();
//
//             // TouchSimulation.Enable();
//             // TouchSim = TouchSimulation.instance;
//
//             Run();
//         }
//
//         private void OnZoomProcess(InputAction.CallbackContext ctx)
//         {
//             Debug.Log("OnZoomProcess");
//         }
//         
//         private void OnZoomStart(InputAction.CallbackContext ctx)
//         {
//             if (EventSystem.current.IsPointerOverGameObject())
//                 return;
//             
//             SetZoomStart();
//         }
//         
//         private async void SetZoomStart()
//         {
//             await Task.Delay(30);
//             
//             _cameraStartPos = Camera.transform.position;
//             _cameraStartSize = Camera.orthographicSize;
//             
//             var pos0 = _inputMap.Input.Pos0.ReadValue<Vector2>();
//             var pos1 = _inputMap.Input.Pos1.ReadValue<Vector2>();
//             _prevDist = Vector2.Distance(pos0, pos1);
//             
//             Debug.Log($"OnZoomEND");
//             _zoomPressed = true;
//             _pressed = false;
//         }
//         
//         private void OnZoomEnd(InputAction.CallbackContext ctx)
//         {
//             Debug.Log("OnZoomEnd");
//             _zoomPressed = false;
//             // OnClickStart(ctx);
//         }
//         
//         private void OnClickStart(InputAction.CallbackContext ctx)
//         {
//             if (EventSystem.current.IsPointerOverGameObject())
//                 return;
//                 
//             Debug.Log("Start");
//             _pressed = true;
//             _cameraStartPos = Camera.transform.position;
//         }
//
//         private void OnClickEnd(InputAction.CallbackContext ctx)
//         {
//             _pressed = false;
//             Debug.Log("ENd");
//             
//             if (EventSystem.current.IsPointerOverGameObject())
//                 return;    
//             
//             Vector2 mousePos = Camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
//             RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
//             if (hit)
//             {
//                 var decore = hit.collider.gameObject.GetComponent<DecoreMono>();
//                 if (decore != null)
//                 {
//                     decore.HandleMouseDown();
//                 }
//             }
//         }
//
//
//         async void Run()
//         {
//             while (true)
//             {
//                 // Process();
//                 await Task.Delay(800);
//             }
//         }
//         void Update()
//         // public void Process()
//         {
//             if (_zoomPressed)
//             {
//                 
//                 pos0 = _inputMap.Input.Pos0.ReadValue<Vector2>();
//                 pos1 = _inputMap.Input.Pos1.ReadValue<Vector2>();
//                 
//                 _dist = Vector2.Distance(pos0, pos1);
//                 
//                 if(_dist > _prevDist)
//                     Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize , Camera.orthographicSize - 1 , Time.deltaTime * zoomSpeed );
//                 if(_dist < _prevDist)
//                     Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize , Camera.orthographicSize + 1 , Time.deltaTime * zoomSpeed );
//                 
//                 _prevDist = _dist;
//                 
//                 return;
//             }
//
//             if (_pressed)
//             {
//                 var diff1 = Camera.ScreenToWorldPoint(Pos0.ReadValue<Vector2>()) - Camera.ScreenToWorldPoint(StartPos0.ReadValue<Vector2>());
//                 Camera.transform.position = _cameraStartPos - diff1;
//                 return;
//             }
//             
        
     }

}
