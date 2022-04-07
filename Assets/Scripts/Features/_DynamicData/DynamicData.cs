using System;
using Features._Events;

namespace Data
{
    [Serializable]
    public class DynamicData 
    {
        public ProfileData Profile = new ProfileData();
        public MergeDynamicData Merge = new MergeDynamicData();
        public TutorialData Tutorial = new TutorialData();
        public UiDynamicData Ui = new UiDynamicData();
        public TimeData Time = new TimeData();

        public void SetCtx(RootEvents events)
        {
            Profile.SetCtx(events);
        }
    }

    public class TutorialData
    {
        public bool Active;
    }

    public class UiDynamicData
    {
        public bool PlayFromQuests;
    }

    public class TimeData
    {
        public DateTime Now;
        public long TicksUntilNextHeart;
    }
}