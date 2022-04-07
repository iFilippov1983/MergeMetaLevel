    // using DG.Tweening;
    // using DG.Tweening.Core;
    // using DG.Tweening.Plugins.Options;

    using UnityEngine;
    using UnityEngine.UI;

    namespace Mono
    {
        public class ZoomInOut : MonoBehaviour {


            float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;

            Vector2 firstTouchPrevPos, secondTouchPrevPos;

            [SerializeField]
            float zoomModifierSpeed = 0.05f;

            [SerializeField]
            Text text;
            public Camera mainCamera;
            public Transform MoveTarget;

            public float TargetZoom = 4 ;
            public float ZoomLerpSpeed = 10f;
    
            public Vector3 HitPos;
            public Vector3 CameraPos;
    
            public float ZoomTime = 0.6f;
            // private TweenerCore<float, float, FloatOptions> Tw;
            private Vector3 TargetPosition = new Vector3(0,0,-10);
            private float UPP;

            public void DoUpdate () {
		
                if (Input.touchCount == 2) {
                    Touch firstTouch = Input.GetTouch (0);
                    Touch secondTouch = Input.GetTouch (1);

                    firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
                    secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

                    touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
                    touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

                    zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

                    if (touchesPrevPosDifference > touchesCurPosDifference)
                        TargetZoom += zoomModifier;
                    if (touchesPrevPosDifference < touchesCurPosDifference)
                        TargetZoom -= zoomModifier;
                }
//            if (mainCamera.orthographicSize != TargetZoom)
//            {
//                TargetZoom = Mathf.Clamp (TargetZoom, 4f, 10f);
//                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, TargetZoom, Time.deltaTime * ZoomLerpSpeed);
//                UPP = mainCamera.UnitsPerPixel();
//            }
//            if(UPP == 0f)
//                UPP = mainCamera.UnitsPerPixel();
            
                if(Input.GetMouseButtonDown(0)){
                    HitPos = Input.mousePosition;
//                CameraPos = mainCamera.transform.position;
                    CameraPos = MoveTarget.position;
                    Debug.Log("DOWN");
                }
            
                if(Input.GetMouseButton(0)){
                    Vector3 pos = Input.mousePosition;
                    var diff = mainCamera.ScreenToWorldPoint(pos) -  mainCamera.ScreenToWorldPoint(HitPos);
//                var diff = pos -  HitPos;
//                diff *= UPP;
//                var diff = mainCamera.ViewportToWorldPoint(pos) -  mainCamera.ViewportToWorldPoint(HitPos);
//                var diff = Camera.main.ScreenToWorldPoint(HitPos - pos);

//                diff.x = (int) (diff.x * 100) / 100;
//                diff.y = (int) (diff.y * 100) / 100;
                    Debug.Log($"{pos-HitPos} {HitPos} {UPP} => {diff}" );
                
                    var newPos = CameraPos - diff;
//                TargetPosition = new Vector3(newPos.x, newPos.y, mainCamera.transform.position.z);
                    TargetPosition = new Vector3(newPos.x, newPos.y, MoveTarget.position.z);
//                TargetPosition = new Vector3((int)(newPos.x*100)/100, (int)(newPos.y*100)/100, MoveTarget.position.z);
                
                }
            
//            if(TargetPosition != mainCamera.transform.position)
//                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, TargetPosition, Time.deltaTime * ZoomLerpSpeed);
//            if(TargetPosition != MoveTarget.position)
//                MoveTarget.position = Vector3.Lerp(MoveTarget.position, TargetPosition, Time.deltaTime * ZoomLerpSpeed);
                MoveTarget.position = TargetPosition;
            
            
                if (Input.touchCount == 1)
                {
//                Touch firstTouch = Input.GetTouch (0);
//                if (firstTouch.phase == TouchPhase.Began) 
//                {
////                    wasRotating = false;    
//                }      
//                Debug.Log(firstTouch.phase);
                              
//                if (firstTouch.phase == TouchPhase.Moved)
//                {
//                    mainCamera.transform.localPosition = mainCamera.transform.localPosition + firstTouch.deltaPosition;            
////                    targetItem.transform.Rotate(0, firstTouch.deltaPosition.x * rotationRate,0,Space.World);
////                    wasRotating = true;
//                }        
                
                }
            
//            if (mainCamera.orthographicSize != TargetZoom && (Tw == null || !Tw.IsPlaying()))
//            {
////                TargetZoom = Mathf.Clamp (TargetZoom, 4f, 10f);
//                text.text = "Camera size " + TargetZoom;
//
//                Tw = mainCamera.DOOrthoSize(TargetZoom, ZoomTime).SetEase(Ease.OutBack);
//            }
            }
        }
    }