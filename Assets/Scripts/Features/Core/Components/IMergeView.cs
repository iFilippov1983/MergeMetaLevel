using System.Threading.Tasks;
using UnityEngine;

namespace Components.Merge
{
    public interface IMergeView
    {
        Task Animate(string name);
        void StopAnimate(string name);

        void SetImage(string name);

        Transform ChildByName(string name);
        
        // void SetSortingOrder(int y);
    }
}