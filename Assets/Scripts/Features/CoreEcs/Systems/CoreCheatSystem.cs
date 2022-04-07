using Entitas;
using UnityEngine;

namespace Core
{
    public class CoreCheatSystem : IExecuteSystem
    {
        private Contexts _contexts;

        public CoreCheatSystem(Contexts contexts)
        {
            _contexts = contexts;
        }
        
        public void Execute()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                ShowFail1();
            if (Input.GetKeyDown(KeyCode.W))
                ShowFail2();
        }

        private void ShowFail2()
        {
            var prefab = _contexts.game.ctx.mergeConfig.Prefabs.GetFxGo("fx.fail2");
            GameObject.Instantiate(prefab);
        }

        private void ShowFail1()
        {
            var prefab = _contexts.game.ctx.mergeConfig.Prefabs.GetFxGo("fx.fail1");
            GameObject.Instantiate(prefab);
        }
    }
}