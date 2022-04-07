using System.Collections.Generic;
using System.Linq;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Dynamic
{
    public class MergeView : MonoBehaviour
    {
        public MergeEditor Editor;
        public MergePlayerLinks Player;
        public GridGeneratorLinks GridGenerator;
        // public CameraFitApi CameraFit;

        [Button]
        void Calc()
        {
            var availablePositions = new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(1, 1),
                new Vector2(5, 1),
                new Vector2(5, 2),
            };
            var targetPos = new Vector2(1, 1);
            var pos51 = new Vector2(5, 1);
            var pos11 = new Vector2(1, 1);
            
            availablePositions = availablePositions.OrderBy(v => (targetPos - v).sqrMagnitude).ToList();
            

            var res = "";
            foreach (var v in availablePositions)
                res += $"{v.x}.{v.y}   ";
            
            Debug.Log((targetPos - pos51).sqrMagnitude);
            Debug.Log((targetPos - pos11).sqrMagnitude);
            Debug.Log(res);
        }
    }
}