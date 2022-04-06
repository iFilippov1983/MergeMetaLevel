using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct PlayerStats
    {
        public int Power;
        public int Gold;
        public int Gems;
        public int DiceRolls;
        [Space]
        public bool LastFightWinner;
        public bool PowerUpgradeAvailable;
        public bool UpgradeTutorialCompleted;
        [Space]
        public int CurrentCellID;
        [Range(1, 100, order = 1)]
        public int CurrentMergeLevel;
        [Range(1, 100, order = 1)]
        public int CurrentPowerUpgradeLevel;
        [Space]
        [SerializeField]
        private int _powerToHealthMultiplier;
        public int Health => Power * _powerToHealthMultiplier;
    }
}
