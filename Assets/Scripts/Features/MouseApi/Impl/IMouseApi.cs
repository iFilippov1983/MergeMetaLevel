using UnityEngine;

namespace Api.Map
{
    public interface IMouseApi
    {
        public Touch? GetFirstTouch();
        public Touch? GetSecondTouch();
        public int GetTouchCount();
        public bool IsOverUi(Touch? touch);
        public bool IsFirstOverUi();
        public bool IsSecondOverUi();
    }
}