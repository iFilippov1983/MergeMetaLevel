using System;
using Configs.Tutorial;

namespace Data
{
    [Serializable]
    public class TutorialEvents
    {
        public Action<TutorialTriggerType> Check;
    }
}