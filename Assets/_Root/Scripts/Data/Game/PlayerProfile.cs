﻿using Level;
using System;
using System.Collections.Generic;
using Tool;
using UnityEngine;

namespace Data
{
    internal class PlayerProfile
    {
        public PlayerStats Stats;

        //public List<CellProperties> CurrentRoute;
        //public readonly SubscriptionProperty<int> CurrentCellID;
        //public readonly SubscriptionProperty<List<CellProperties>> CurrentRoute;

        public PlayerProfile(PlayerStats initialStats)
        {
            Stats = initialStats;

            //CurrentRoute = new List<CellProperties>();
            //CurrentRoute = new SubscriptionProperty<List<CellProperties>>();
            //CurrentCellID = new SubscriptionProperty<int>();
            //CurrentCellID.Value = initialCellID;
        }
    }
}