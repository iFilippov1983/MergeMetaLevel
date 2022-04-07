using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Configs.Tutorial
{
    public class TutorialConfig: SerializedScriptableObject
    {
        public List<TutorialStep> Steps;
    }
}