namespace Game
{
    internal interface IAnimationControler
    {
        public bool GetAppearAnimationFinished();
        public bool GetAttackAnimationFinished();
        public bool GetGotHitAnimationFinished();
        public bool GetDeathAnimationFinished();
        public bool GetFinishOffAnimationFinished();
    }
}
