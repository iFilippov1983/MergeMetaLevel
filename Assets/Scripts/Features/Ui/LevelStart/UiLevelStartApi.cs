using System;
using System.Threading.Tasks;
using Configs.Tutorial;
using Core;
using Data;
using Sirenix.OdinInspector;
using Utils;

namespace Components
{
    [Serializable]
    public class UiLevelStartApi : UiBaseApi
    {
        private UiLevelStart _view;
        private bool? _result;
        private CoreRoot _root;
        private TutorialEvents _tutorialEvents;

        public void SetCtx(UiLevelStart view, CoreRoot root)
        {
            base.SetCtxBase(view);
            base.SetBlur(root);
            _root = root;
            _view = view;
            _view.CloseBtn.OnClick(() => _result = false);
            _view.PlayBtn.OnClick(() => _result = true);
            _tutorialEvents = root.Events.Tutorial;
        }
        
        // [Button]
        // public async Task<bool> Show()
        // {
        //     _view.Caption.text = $"{"ui/level".Loc()} {_root.DynamicData.Profile.LevelIndex +1}";
        //     UpdateGoals(_root.DynamicData.Merge.Level);
        //     
        //     await DoShow();
        //     _tutorialEvents?.Check?.Invoke(TutorialTriggerType.Prelevel);
        //
        //     _result = null;
        //     await WaitUntil(() => _result != null);
        //     DoHide().DoAsync();
        //     
        //     return _result.Value;
        // }
        
        private void UpdateGoals(LevelConfig level)
        {
            _view.TargetImg.sprite = level.Goal.Config.Sprite;
        }
    }
}