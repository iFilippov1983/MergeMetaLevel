using System;
using System.Threading.Tasks;
using Configs;
using Data;
using Sirenix.OdinInspector;
using Utils;

namespace Components
{
    [Serializable]
    public class UiNoStarsApi : UiBaseApi
    {
        private UiNoStars _view;
        private bool? _resultPlayOn;
        private DynamicData _dynamicData;
        private ShopConfig _staticDataShop;

        public void SetCtx(UiNoStars view, CoreRoot root)
        {
            base.SetCtxBase(view);
            base.SetBlur(root);
            _view = view;
            _view.OkBtn.OnClick(() => _resultPlayOn = true);

            _dynamicData = root.Data;
            _staticDataShop = root.Configs.Shop;
        }
        
        [Button]
        public async Task Show()
        {
            await DoShow();
            _resultPlayOn = null;
            await WaitUntil(() => _resultPlayOn != null);
        }

        protected override void BeforeShow()
        {
            base.BeforeShow();
            _view.Xp.TweenCount = _dynamicData.Profile.Xp;
        }
    }
}