using Data;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Configs.Meta
{
    public class DebugConfig : ScriptableObject
    {
        public bool SkipQuests;
        public bool SkipTutors;
        public bool SkipMerge;
        public bool FreeBuy;
        [Range(0, 1)][OnValueChanged("ChangeTimeScale")][ShowInInspector] 
        private float TimeScale;

        public DynamicData DynamicData;

        private CoreRoot _root;
        
        public void SetCtx(CoreRoot root)
        {
            _root = root;
            DynamicData = root.Data;
            
            _root.View.Ui.Merge.Cheat_LevelWinBtn.OnClick(LevelWin);
            _root.View.Ui.Merge.Cheat_LevelFailBtn.OnClick(LevelFail);
        }

        [Button] public void LevelWin() 
            => _root.Merge.Cheat_WinLevel();
        
        [Button] public void LevelFail() 
            => _root.Merge.Cheat_FailLevel();
        
        [Button] public void Add5Xp()
        {
            _root.Data.Profile.Xp += 5;
            _root.Save.SaveProfile();
        }

        [Button] public void SpendHeart()
            => _root.Data.Profile.Hearts--;
        
        void ChangeTimeScale()
            => Time.timeScale = TimeScale;
        
        [Button] public void Add1000Coins()
        {
            _root.Data.Profile.Coins += 1000;
            _root.Save.SaveProfile();
        } 
        
        [Button] public void ClearProfile()
        {
            _root.Data.Profile = null;
            _root.Events.App.OnDataSave = null;
            PlayerPrefs.SetString("data", null);
            Application.Quit();
        }
        
    }
}