using System;
using System.Threading.Tasks;
using Configs;
using Configs.Meta;
using Data;
using Sirenix.OdinInspector;
using Utils;

namespace Components
{
    [Serializable]
    public class UiDebugApi : UiBaseApi
    {
        private UiDebugView _view;
        private DebugConfig _debug;
        private CoreRoot _root;
        private ProfileData _profileData;

        public void SetCtx(UiDebugView view, CoreRoot root)
        {
            base.SetCtxBase(view);
            base.SetBlur(root);
            _view = view;
            _debug = root.Configs.Debug;
            _root = root;
            _profileData = root.Data.Profile;
            
            _view.ClearProfile.OnClick(_debug.ClearProfile);
            _view.Add5Xp.OnClick(_debug.Add5Xp);
            _view.Add100Coins.OnClick(_debug.Add1000Coins);
            
            SetToggles();
            _view.Level.text = (_profileData.LevelIndex + 1).ToString();

            _view.SkipQuests.onValueChanged.AddListener(value => _debug.SkipQuests = value);
            _view.SkipTutors.onValueChanged.AddListener(value => _debug.SkipTutors = value);
            _view.SkipMerge.onValueChanged.AddListener(value => _debug.SkipMerge = value);
            _view.Level.onValueChanged.AddListener(value =>
            {
                if (int.TryParse(value, out int level))
                {
                    var newLevel = Math.Max(0, level - 1);
                    _profileData.LevelIndex = newLevel;
                }
            });

            _view.Close.OnClick(() => _closed = true);
        }
        
        [Button]
        public async Task Show()
        {
            SetToggles();
            _view.Level.text = (_profileData.LevelIndex + 1).ToString();

            await DoShow();
            _closed = false;
            await WaitUntil(() => _closed );

            await DoHide();
        }

        private void SetToggles()
        {
            _view.SkipQuests.SetIsOnWithoutNotify(_debug.SkipQuests);
            _view.SkipTutors.SetIsOnWithoutNotify(_debug.SkipTutors);
            _view.SkipMerge.SetIsOnWithoutNotify(_debug.SkipMerge);
        }
    }
}