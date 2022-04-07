using Data;
using Features._Events;
using UnityEngine;

namespace Components.Services
{
    
    public class SaveApi
    {
        private RootEvents _events;
        private DynamicData _data;

        public void SetCtx(CoreRoot root)
        {
            _events = root.Events;
            _data = root.Data;
        }

        public void SaveProfile()
        {
            var values = _data.Profile;
            _events.App.OnDataSave?.Invoke(values);
            PlayerPrefs.SetString("data", values?.ToJson());
        }

        public void LoadProfile(DynamicData data)
        {
            var profileData = DoLoad();
            profileData ??= new ProfileData();
            profileData.Patch();
            Debug.Log(profileData.ToJson());
            data.Profile = profileData;
        }

        public void NotifyProfileReady()
        {
            _events.App.OnDataLoaded?.Invoke(_data.Profile);
        }

#if UNITY_EDITOR

        [UnityEditor.MenuItem("Data/Clear")]
        public static void ClearProfile()
            => PlayerPrefs.SetString("data", null);
#endif

        private static ProfileData DoLoad() 
            => PlayerPrefs
                .GetString("data")
                .FromJson<ProfileData>();
    }
}