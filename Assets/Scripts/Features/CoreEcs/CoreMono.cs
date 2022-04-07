using System;
using System.Threading.Tasks;
using Data;
using Features._Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public class CoreMono : MonoBehaviour
    {
        public int level;
        public MergePlayerLinks links;
        public MergeConfig config;
        private Core _core;
        
        private void Start()
        {
            TaskScheduler.UnobservedTaskException += HandleTaskException;
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
            // Application.logMessageReceivedThreaded += HandleLog;
        }

        private void OnEnable()
        {
            CreateCore();
            NewGame();
        }

        private void OnDisable()
        {
            Clear();
            ReleaseCore();
        }

        [Button(ButtonSizes.Large), HorizontalGroup("next")]
        private async void Prev()
        {
            --level;
            Clear();
            await Task.Delay(300);
            NewGame();
        }
        [Button(ButtonSizes.Large), HorizontalGroup("next")]
        private async void Next()
        {
            ++level;
            Clear();
            await Task.Delay(300);
            NewGame();
        }
        
        [Button(ButtonSizes.Large), HorizontalGroup("next")]
        private async void Replay()
        {
            Clear();
            await Task.Delay(800);
            NewGame();
        }
        
        
        [Button]
        private void CreateCore()
            => _core = _core 
                       ?? new Core(Contexts.sharedInstance, links, config, new MergeDynamicData(), new MergeEvents());

        [Button]
        private void NewGame() 
            => _core?.NewGame(level);

        private void Update() 
            => _core?.Execute();

        [Button]
        private void Clear() 
            => _core?.Clear();

        [Button]
        private void ReleaseCore()
        {
            _core?.TearDown();
            _core = null;
        }
        
        // [Button, HorizontalGroup("cheat")]
        // private void ActivateCheatClick()
        //     => _core.cheatClick = true;
        //
        // [Button, HorizontalGroup("cheat")]
        // private void DeactivateCheatClick()
        //     => _core.cheatClick = false;
        
        private void HandleLog(string condition, string stacktrace, LogType type)
        {
        }

        private void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args) 
            => Debug.LogError((Exception)args.ExceptionObject);

        private void HandleTaskException(object sender, UnobservedTaskExceptionEventArgs e)
            => Debug.LogError(e.Exception);


    }
}