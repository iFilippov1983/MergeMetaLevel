using Level;
using System.Collections.Generic;
using Tool;
using UnityEngine;

namespace Data
{
    internal class PlayerProfile
    {
        public int CoinsAmount;
        public int GemsAmount;
        public int CurrentCellID;
        //public List<CellProperties> CurrentRoute;
        //public readonly SubscriptionProperty<int> CurrentCellID;
        //public readonly SubscriptionProperty<List<CellProperties>> CurrentRoute;

        public PlayerProfile(int initialCellID)
        {
            CurrentCellID = initialCellID;
            //CurrentRoute = new List<CellProperties>();
            //CurrentRoute = new SubscriptionProperty<List<CellProperties>>();
            //CurrentCellID = new SubscriptionProperty<int>();
            //CurrentCellID.Value = initialCellID;
        }
    }
}