using System;
using System.Threading.Tasks;
using Configs;
using Data;
using Features._Events;
using Sirenix.OdinInspector;
using UniRx;
using Utils;

namespace Components
{
    [Serializable]
    public class UiNoHeartsApi : UiBaseApi
    {
        private UiNoHearts _view;
        private bool? _getFree;
        private DynamicData _dynamicData;
        private RootEvents _events;
        private ProfileData _profile;
        private TimeData _timeData;

        public void SetCtx(UiNoHearts view, CoreRoot root)
        {
            base.SetCtxBase(view);
            base.SetBlur(root);

            _events = root.Events;
            _view = view;
            _timeData = root.Data.Time;

            _view.CloseBtn.OnClick(Close);
            _view.GetFreeBtn.OnClick(Ads);

            _profile = root.Data.Profile;
            // _staticDataShop = root.Configs.Shop;
        }

        private void Ads()
            => _getFree = true;

        private void Close()
            => _getFree = false;

        [Button]
        private void TestShow()
            => Show().DoAsync();
        
        public async Task<bool> Show()
        {
            await DoShow();
            _events.App.OnUpdate += OnUpdate;
            
            _getFree = null;
            await WaitUntil(() => _getFree != null);
            
            _events.App.OnUpdate -= OnUpdate;

            await DoHide();
            
            return _getFree.Value;
        }

        private void OnUpdate()
        {
            _view.Timer.text = new TimeSpan(_timeData.TicksUntilNextHeart).ToString(@"mm\:ss");
        }
    }
}