using System;

namespace Features._Events
{
    public class MergeEvents
    {
        public Action<string> UseTool;
        public Action OnFieldChanged;
        public Action OnMoveSpend;
        
        public Func<int, int, bool> IsClickAllowed;
        public Func<int, int, bool> StartDragAllowed;
        public Func<int, int, bool> EndDragAllowed;
        public Action<int, int> OnEndDrag;
        public Action<int, int> OnClick;
        public Action OnStepEnd;

        public void Clear()
        {
            UseTool = null;
            OnFieldChanged = null;
            OnMoveSpend = null;
            IsClickAllowed = null;
            OnStepEnd = null;
        }
    }
}