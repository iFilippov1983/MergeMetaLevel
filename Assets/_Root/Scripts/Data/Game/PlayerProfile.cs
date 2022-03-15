using Level;
using System.Collections.Generic;
using Tool;
using UnityEngine;

namespace Data
{
    internal class PlayerProfile
    {
        public readonly SubscriptionProperty<int> CurrentCellID;
        public readonly SubscriptionProperty<List<CellEntity>> CurrentRoute;

        public PlayerProfile(int initialCellID)
        {
            CurrentRoute = new SubscriptionProperty<List<CellEntity>>();
            CurrentCellID = new SubscriptionProperty<int>();
            CurrentCellID.Value = initialCellID;
        }
    }
}