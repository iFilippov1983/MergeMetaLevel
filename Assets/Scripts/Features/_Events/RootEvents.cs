using Data;

namespace Features._Events
{
    public class RootEvents
    {
        public AppEvents App = new AppEvents();
        public TutorialEvents Tutorial = new TutorialEvents();
        public MergeEvents Merge = new MergeEvents();
        public DataEvents Data = new DataEvents();
    }
}