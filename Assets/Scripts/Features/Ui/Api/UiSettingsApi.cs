using System;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Utils;

namespace Components
{
    [Serializable]
    public class UiSettingsApi : UiBaseApi
    {
        private UiSettingsView _view;
        private bool? _result;

        public void SetCtx(UiSettingsView view, CoreRoot root)
        {
            base.SetCtxBase(view);
            base.SetBlur(root);
            _view = view;
            _view.CloseBtn.OnClick(() => _result = false);
            _view.ResetProgressBtn.OnClick(root.Configs.Debug.ClearProfile);
        }
        
        [Button]
        public async Task Show()
        {
            await DoShow();
            _result = null;
            await WaitUntil(() => _result != null);
            DoHide().DoAsync();
        }
    }
}