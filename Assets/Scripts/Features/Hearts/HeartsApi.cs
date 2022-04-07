using System;
using Data;
using UnityEngine;

namespace Components.Services
{
    public class HeartsApi
    {
        private ProfileData _profile;
        private TimeData _timeData;
        private int _heartsCache;
        public const long HeartRestoreTicks = 60 * 1 * TimeSpan.TicksPerSecond;
        public const int MaxHearts = 5;

        public HeartsApi()
        {
            
        }

        public void SetCtx(CoreRoot root)
        {
            _profile = root.Data.Profile;
            _timeData = root.Data.Time;
            
            InitialRestoreHearts();
            _heartsCache = _profile.Hearts; // Note : Must be AFTER RestoreHearts
            
            root.Events.App.OnEverySecond += OnEverySecond;
            root.Events.Data.OnResourceChanged += OnResourceChanged;
        }

        private void OnResourceChanged(ResourceType type, int value, int oldValue)
        {
            if(type != ResourceType.Hearts) 
                return;
            
            _heartsCache = value;
            
            if (oldValue == MaxHearts && value != MaxHearts)
                ResetTimer();
        }

        private void ResetTimer()
            => _profile.HeartsChangedTime = _timeData.Now;

        private void OnEverySecond()
        {
            if (EnoughHearts())
                return;

            var timeDiff = TimeDiff();
            _timeData.TicksUntilNextHeart = HeartRestoreTicks - timeDiff;
            
            Debug.Log($"{_timeData.TicksUntilNextHeart/TimeSpan.TicksPerSecond} = {HeartRestoreTicks/TimeSpan.TicksPerSecond} {timeDiff/TimeSpan.TicksPerSecond} : ");
            if (TimeElapsed() && _heartsCache < MaxHearts)
                RestoreHeart();
        }

        private bool EnoughHearts()
            => _heartsCache >= MaxHearts;

        private long TimeDiff()
            => _timeData.Now.Ticks - _profile.HeartsChangedTime.Ticks;

        private bool TimeElapsed()
            => _timeData.TicksUntilNextHeart <= 0;

        private void InitialRestoreHearts()
        {
            var timeDiff = TimeDiff();
            while (_profile.Hearts < MaxHearts && timeDiff > HeartRestoreTicks)
            {
                RestoreHeart();
                timeDiff -= HeartRestoreTicks;
                _timeData.TicksUntilNextHeart = HeartRestoreTicks - timeDiff;
            }
        }
            
        private void RestoreHeart()
        {
            _profile.Hearts++;
            _profile.HeartsChangedTime = _timeData.Now;
        }
    }
}