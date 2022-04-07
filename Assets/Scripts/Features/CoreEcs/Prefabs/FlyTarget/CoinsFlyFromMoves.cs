using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class CoinsFlyFromMoves : MonoBehaviour
    {
        public PropertyName id => new PropertyName();
        
        
        public Transform target;

        public SimplePath BeginPath;
        public SimplePath FinalPath;
        
        public MoveCurves BeginCurves;
        public MoveCurves FinalCurves;
        
        public float duration = 1f;
        
        public void StartFly()
        {
            Debug.Log("Start fly");
            target.position = BeginPath.From;
            target.DOMoveX(BeginPath.To.x, duration).SetEase(BeginCurves.X);
            target.DOMoveY(BeginPath.To.y, duration).SetEase(BeginCurves.Y);
        }

        public void FinalFly()
        {
            Debug.Log("Final fly");
            target.position = FinalPath.From;
            target.DOMoveX(FinalPath.To.x, duration).SetEase(FinalCurves.X);
            target.DOMoveY(FinalPath.To.y, duration).SetEase(FinalCurves.Y);
        }
        
    }
}