using System;

namespace Data
{
    [Serializable]
    public class AppEvents
    {
        public Action<bool> OnAppPause;
        public Action<ProfileData> OnDataSave;
        public Action<ProfileData> OnDataLoaded;
        public Action OnUpdate;
        public Action OnEverySecond;
    }
}