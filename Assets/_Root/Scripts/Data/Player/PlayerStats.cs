using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class PlayerStats
    {
        public int Power;
        public int Gold;
        public int Gems;
        public int DiceRolls;
        [Space]
        public bool LastFightWinner = true;
        public bool PowerUpgradeAvailable;
        public bool UpgradeTutorialCompleted;
        [Space]
        public int CurrentCellID;
        [Range(1, 100, order = 1)]
        public int CurrentMergeLevel;
        [Range(1, 100, order = 1)]
        public int CurrentPowerUpgradeLevel;
        //[Space]
        //[SerializeField]
        private const int _powerToHealthMultiplier = 3;
        public int Health => Power * _powerToHealthMultiplier;
    }
}
