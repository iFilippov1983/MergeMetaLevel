using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct PlayerStats
    {
        [SerializeField] private int _power;
        [SerializeField] private int _health;
        [SerializeField] private int _powerToHealthMultiplyer;

        public int Power
        {
            get => _power;
            set 
            {
                _power = value;
                _health = _power * _powerToHealthMultiplyer;
            }
        }

        public int Health => _health;

        public int CurrentCellID;
        public int Coins;
        public int Gems;
        public int DiceRolls;
        public bool LastFightWinner;
    }
}
