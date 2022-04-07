using System.Linq;
using Data;
using Entitas.VisualDebugging.Unity;
using Features._Events;
using Sirenix.Utilities;
using UnityEngine;

namespace Core
{
    public class Core
    {
        private readonly Contexts _contexts;
        private readonly CoreSystems _systems;
        private bool _ready;

        public Core(Contexts contexts, MergePlayerLinks view, MergeConfig config, MergeDynamicData data, MergeEvents events)
        {
            _contexts = contexts;
            
            _contexts.game.SetCtx(
                view
                ,new CoreServices()
                ,data
                ,events
                ,config.VisualConfig
                ,config.Rules
                ,config
                ,null
            );
            
            // _contexts.game.ctx.dynamicData.Clear();
            _contexts.game.ctx.services.Init(_contexts, _contexts.game.ctx, view, config, _contexts.game.ctx.dynamicData);
            // _contexts.game.ctx.board.Init(_contexts.game.ctx);
            
            _systems = new CoreSystems(_contexts, _contexts.game.ctx);
            _systems.Initialize();
        }

        public CoreSystems Systems 
            => _systems;
        
        public MergeDynamicData data 
            => _contexts.game.ctx.dynamicData;

        public void NewGame(LevelConfig levelConfig)
        {
            _systems.ActivateReactiveSystems();
            _ready = true;

            var ctx = _contexts.game.ctx;
            ctx.levelConfig = levelConfig;
            ctx.dynamicData.Level = ctx.levelConfig;
            
            ctx.services.cameraFit.LoadLevel(ctx.levelConfig);
            
            _contexts.game.doCreateLevel = true;
        }
        
        public void NewGame(int level)
        {
            _systems.ActivateReactiveSystems();
            _ready = true;

            var ctx = _contexts.game.ctx;
            // ctx.level = level;
            ctx.levelConfig = ctx.services.levelConfig.Load(level);
            ctx.dynamicData.Level = ctx.levelConfig;
            
            ctx.services.cameraFit.LoadLevel(ctx.levelConfig);
            
            _contexts.game.doCreateLevel = true;
        }
        
        public void Execute()
        {
            if(!_ready)
                return;
            
            _systems.Execute();
            _systems.Cleanup();
        }

        public void Clear()
        {
            _contexts.chips.GetEntities().ForEach(e => e.Destroy());
            _contexts.board.GetEntities().ForEach(e => e.Destroy());
            _contexts.game.GetEntities().Where(e => !e.hasCtx).ForEach(e => e.Destroy());
            
            _contexts.game.ctx.Clear();
            _systems.DeactivateReactiveSystems();
            _ready = false;
        }
        
        public int ChipsCount(MergeItemConfig goalConfig) 
            => _contexts.game.ctx.dynamicData.ChipsCount(goalConfig);

        public void TearDown()
        {
            Clear();
            
            _contexts.game.ctx.TearDown(); 
            _systems.ClearReactiveSystems();
            _systems.TearDown();
            Contexts.sharedInstance.Reset();
            
            var debugSystems = GameObject.FindObjectsOfType<DebugSystemsBehaviour>();
            foreach (var debug in debugSystems)
                Object.Destroy(debug.gameObject);
            
            // var entitasCache = GameObject.FindObjectsOfType<ContextObserverBehaviour>();
            // foreach (var cache in entitasCache)
            //     Object.Destroy(cache.gameObject);
        }

        // public bool cheatClick
        // {
        //     set => _contexts.game.isCheatClick = value;
        // }
    }
}