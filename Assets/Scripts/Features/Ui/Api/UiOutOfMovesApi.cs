using System;
using System.Threading.Tasks;
using Configs;
using Data;
using Sirenix.OdinInspector;
using Utils;

namespace Components
{
    [Serializable]
    public class UiOutOfMovesApi : UiBaseApi
    {
        private DynamicData _dynamicData;
        private bool? _resultPlayOn;
        private ShopConfig _staticDataShop;
        private UiOutOfMoves _view;

        public void SetCtx(UiOutOfMoves view, CoreRoot root)
        {
            base.SetCtxBase(view);
            _view = view;
            _view.CloseBtn.OnClick(() => _resultPlayOn = false);
            // _view.GiveUpBtn.OnClick(() => _resultPlayOn = false);
            _view.PlayOnBtn.OnClick(() => _resultPlayOn = true);

            _dynamicData = root.Data;
            _staticDataShop = root.Configs.Shop;
        }

        [Button]
        void TestShow()
            => Show().DoAsync();

        public async Task<bool> Show()
        {
            await DoShow();
            _resultPlayOn = null;
            await WaitUntil(() => _resultPlayOn != null);
            await DoHide();
            return _resultPlayOn.Value;
        }

        protected override void BeforeShow()
        {
            base.BeforeShow();
            _view.Coins.TweenCount = _dynamicData.Profile.Coins;
            _view.Cost.text = _staticDataShop.BuyMovesCost.ToString();
        }
    }
}