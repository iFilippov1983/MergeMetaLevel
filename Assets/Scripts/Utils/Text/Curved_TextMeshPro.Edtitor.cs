using System.Threading.Tasks;
using UnityEngine;
using Utils.Serialize;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils.Text
{
    public class Curved_TextMeshProEditor 
    {
        private bool _break;
        private Curved_TextMeshPro _curveText;

        public static Curved_TextMeshProEditor Create(Curved_TextMeshPro curveText) => 
#if UNITY_EDITOR
            new Curved_TextMeshProEditor(curveText);
#else
            null;
#endif

        protected Curved_TextMeshProEditor(Curved_TextMeshPro curveText) 
        {
            _curveText = curveText;
        }

        public void OnEnable()
        {
#if UNITY_EDITOR
            _break = false;
            _curveText.VertexCurve = CurveTemplate();
            // RunEditCycle();
#endif
            _curveText.ChangeTextVertices();
        }

        public void OnDisable()
        {
#if UNITY_EDITOR
            _break = true;
#endif
        }

        public static void ApplyCurve( Curved_TextMeshPro curveText)
        {
#if UNITY_EDITOR
            curveText.VertexCurve = CurveTemplate();
#endif
        }

        private static AnimationCurve CurveTemplate() => CurveClone(Curved_TextMeshProConfig.instance.Curve);

        private static AnimationCurve CurveClone(AnimationCurve curve) => new AnimationCurve(curve.keys.Copy());


        public static void KeepScale(Curved_TextMeshPro curveText)
        {
#if UNITY_EDITOR
            SaveRuntimeScale(curveText);
#endif
        }
        
        public static void ApplyScale(Curved_TextMeshPro curveText )
        {
#if UNITY_EDITOR
            curveText.CurveScale = GetRuntimeScale();
#endif
        }


        private async void RunEditCycle()
        {
#if UNITY_EDITOR
            
            // AutoSAve();

            while (true)
            {
                if (_break)
                    return;

                _curveText.ChangeTextVertices();
                await Task.Delay(100);
            }
#endif
        }

        // private void AutoSAve()
        // {
        //     void SaveOnExitPlayMode(PlayModeStateChange stateChange)
        //     {
        //         if (stateChange != PlayModeStateChange.ExitingPlayMode)
        //             return;
        //         SaveRuntimeScale(_curveText);
        //         EditorApplication.playModeStateChanged -= SaveOnExitPlayMode;
        //     }
        //
        //     EditorApplication.playModeStateChanged += SaveOnExitPlayMode;
        // }

        private static float GetRuntimeScale()
        {
#if UNITY_EDITOR
            return EditorPrefs.GetFloat("warpText");
#else
            return 0;
#endif
            
        }

        private static void SaveRuntimeScale(Curved_TextMeshPro curveText)
        {
#if UNITY_EDITOR
            EditorPrefs.SetFloat("warpText", curveText.CurveScale);
#endif            
        } 
    }
}