using System;

namespace Data
{
    [Serializable]
    public struct PlayerStats
    {
        public int CurrentCellID;
        public int Power;
        public int Health;
        public int Coins;
        public int Gems;
        public int DiceRolls;
        public bool LastFightWinner;
    }
}
