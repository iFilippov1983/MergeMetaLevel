using System;
using UnityEngine;

namespace Api.Map
{
    public class MouseState
    {
        public MouseStateFlags Id;
        public MouseStateFlags FromStates;
        public Action OnEnterEvent;
        public Action OnExitEvent;

        public bool CanTransitFrom(MouseStateFlags from) => FromStates.HasFlag(@from);
        public void OnEnter()
        {
            Debug.Log($">>> {Id}");
            OnEnterEvent?.Invoke();
        }

        public void OnExit()
        {
            OnExitEvent?.Invoke();
        }

        public MouseState(MouseStateFlags id, MouseStateFlags fromStates, Action onEnter = null, Action onExit = null)
        {
            Id = id;
            FromStates = fromStates;
            OnEnterEvent = onEnter;
            OnExitEvent = onExit;
        }
            
    }
}