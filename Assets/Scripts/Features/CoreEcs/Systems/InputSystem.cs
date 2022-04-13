using Data;
using Entitas;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class InputSystem : IExecuteSystem
    {
        private MergeDynamicData.InputDynamicData _data;

        public InputSystem(Contexts contexts)
        {
            _data = contexts.game.ctx.dynamicData.Input;
        }
        
        public void Execute()
        {
            if(_data.InputLocked)
                return;
            
            _data.MouseDown = false;
            _data.MouseUp = false;
            _data.MousePressed = false;

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _data.MouseDown = true;
                _data.SkipPressed = true;
            }

            else if (Input.GetMouseButtonUp(0))
                _data.MouseUp = true;

            else if (Input.GetMouseButton(0))
                _data.MousePressed = true;
        }
    }
}