using System;
using System.Globalization;
using System.Threading.Tasks;
using Configs;
using Data;
using UnityEngine;
using Utils;

namespace Components
{
    public class ApplicationMono : MonoBehaviour
    {
        public CoreRoot root;
        public StaticData Configs;
        public RootView RootView;

        private void Awake()
        {
            TaskScheduler.UnobservedTaskException += HandleTaskException;
            Application.targetFrameRate = 60;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;  
            
            root = new CoreRoot(RootView, Configs);
            root.Run();
        }

        private void HandleTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.LogError(e.Exception);
        }
        
        private void Update()
        {
            root.OnUpdate();
        }

        private void OnDestroy()
        {
            // RootCtx.OnAppDestroy();
        }

        private void OnApplicationQuit()
        {
            root.OnQuit();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            root.OnPause(pauseStatus);
        }
    }
}