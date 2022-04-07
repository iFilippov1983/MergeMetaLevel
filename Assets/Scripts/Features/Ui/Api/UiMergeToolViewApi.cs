using System;
using System.Threading.Tasks;
using Configs.Meta;
using Data;
using Sirenix.OdinInspector;
using Utils;

namespace Components
{
    [Serializable]
    public class UiMergeToolViewApi : UiBaseApi
    {
        private bool? _useTool;
        private UiMergeToolView _view;
        private ResourcesConfig _resourcesConfig;
        public UiMergeToolView View => _view;

        public void SetCtx(UiMergeToolView view, CoreRoot root)
        {
            base.SetCtxBase(view);
            
            _view = view;
            _view.CloseBtn.OnClick(() => _useTool = false);
            _view.OnClick = () => _useTool = true;
            _resourcesConfig = root.Configs.ResourcesConfig;
        }

        [Button]
        public async Task<bool> Show(string name)
        {
            var boosterOnfo = ToBoosterInfo(name);
            _view.CaptionText.text = boosterOnfo.Name.Loc("resources");
            _view.InfoText.text = boosterOnfo.Description.Loc("resources");
            await DoShow();
            _useTool = null;
            await WaitUntil(() => _useTool != null);
            await DoHide();
            return _useTool.Value;
        }

        private ResourceInfo ToBoosterInfo(string name)
        {
            var id =  name switch
            {
                "one" => ResourceType.GameBooster1,
                "upgradeOne" => ResourceType.GameBooster2,
                "nItems" => ResourceType.GameBooster3,
            };
            return _resourcesConfig.Resources.SaveGet(id);
        }
    }
}