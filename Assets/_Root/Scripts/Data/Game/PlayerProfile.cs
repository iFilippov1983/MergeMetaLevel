namespace Data
{
    internal class PlayerProfile
    {
        public PlayerStats Stats;
        //public readonly SubscriptionProperty<int> CurrentCellID;
        //public readonly SubscriptionProperty<List<CellProperties>> CurrentRoute;

        public PlayerProfile(PlayerStats stats)
        {
            Stats = stats;
            //CurrentRoute = new SubscriptionProperty<List<CellProperties>>();
            //CurrentCellID = new SubscriptionProperty<int>();
        }
    }
}