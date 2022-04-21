namespace Data
{
    internal class PlayerProfile
    {
        public PlayerStats Stats;
        //public readonly SubscriptionProperty<int> CurrentPower;

        public PlayerProfile(PlayerStats stats)
        {
            Stats = stats;
            //CurrentPower = new SubscriptionProperty<int>();
        }
    }
}