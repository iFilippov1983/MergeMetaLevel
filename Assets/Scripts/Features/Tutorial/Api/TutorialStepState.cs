using System;
using System.Threading.Tasks;
using Configs.Tutorial;
using Data;
using Features._Events;
using UnityEngine;
using Utils;

namespace Components.Api.Tutorial
{
    [Serializable]
    public class TutorialStepState
    {
        private readonly TutorialStep _step;
        private readonly MergeDynamicData _data;
        private readonly UiTutorialApi _view;
        private readonly MergeEvents _events;
        private readonly Camera _camera;
        private Action _onComplete;
        private bool _active;
        private MergeDynamicData.InputDynamicData _inputData;
        public TutorialStep Step => _step;

        public TutorialStepState(TutorialStep step, MergeDynamicData data, UiTutorialApi view, MergeEvents events,  Camera camera, Action onComplete)
        {
            _step = step;
            _data = data;
            _view = view;
            _events = events;
            _camera = camera;
            _onComplete = onComplete;
            _inputData = _data.Input;
        }

        public void OnEnter()
        {
            Debug.Log($"OnEnter {_step.name}");
            _active = true;
            if (_step.Action != null)
            {
                _events.IsClickAllowed += ClickAllowed;
                _events.StartDragAllowed = StartDragAllowed;
                _events.EndDragAllowed = EndDragAllowed;
                _events.OnEndDrag = OnComplete;
                _events.OnClick = OnComplete;
                
                _view._view.ClickReceiver.OnMouseDown = OnMouseDown;
                _view._view.ClickReceiver.OnMouseUp = OnMouseUp;
            }
            _inputData.InputLocked = true;
            _view.Show(_step, OnComplete2);
        }

        public void OnExit()
        {
            if(_active == false)
                return;
            _active = false;
            Debug.Log($"OnExit {_step.name}");
            
            if (_step.Action != null)
            {
                _events.IsClickAllowed -= ClickAllowed;
                _events.StartDragAllowed = null;
                _events.EndDragAllowed = null;
                _events.OnEndDrag = null;
                _events.OnClick = null;
                
                _view._view.ClickReceiver.OnMouseDown = null;
                _view._view.ClickReceiver.OnMouseUp = null;
            }
            _inputData.InputLocked = false;
            _view.Hide();
        }
                
        private void OnMouseDown()
        {
            _inputData.MousePressed = true;
            _inputData.MouseDown = true;
            _inputData.MouseUp = false;
        }

        private void OnMouseUp()
        {
            _inputData.MouseDown = false;
            _inputData.MousePressed = false;
            _inputData.MouseUp = true;
        }

        private void OnComplete2()
        {
            _onComplete?.Invoke();
            _onComplete = null;
        }

        private void OnComplete(int x, int y)
        {
            _onComplete?.Invoke();
            _onComplete = null;
        }

        private bool ClickAllowed(int x, int y)
        {
            Debug.Log($"Click allowed : {StartDragAllowed(x,y) && _step.Action.clickOnly}");
            
            if (!_step.Action.clickOnly)
                return false;
            
            return _step.Action.fromCell.x == x
                          && _step.Action.fromCell.y == y; 
        }

        private bool StartDragAllowed(int x, int y) {
            Debug.Log($"StartDrag at {x} {y} allowed : {_step.Action.fromCell.x == x && _step.Action.fromCell.y == y}");

            if (_step.Action.clickOnly)
                return false;
            
            return _step.Action.fromCell.x == x
            && _step.Action.fromCell.y == y; 
        }

        private bool EndDragAllowed(int x, int y)
        {
            Debug.Log($"EndDrag at {x}=={_step.Action.toCell.x} {y}=={_step.Action.toCell.y} allowed : {_step.Action.toCell.x == x && _step.Action.toCell.y == y}");

            // Or Click Or Drag
            if (_step.Action.clickOnly)
                return false;
                
            return _step.Action.toCell.x == x
                   && _step.Action.toCell.y == y;
        }
    }
}