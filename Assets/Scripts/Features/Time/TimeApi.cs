using System;
using Data;
using UniRx;

namespace Components.Systems
{
    public class TimeApi
    {
        private AppEvents _appEvents;
        private IDisposable _everySecond;
        private DynamicData _data;

        public TimeApi()
        {
            
        }

        public void SetCtx(CoreRoot root)
        {
            _appEvents = root.Events.App;
            _data = root.Data;
            _data.Time.Now = DateTime.Now;
        }

        public void Run()
        {
            _everySecond = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x =>
            {
                _data.Time.Now = DateTime.Now;                
                _appEvents.OnEverySecond?.Invoke();
            });
        }

        ~TimeApi()
        {
            _everySecond.Dispose();
        }
    }
}