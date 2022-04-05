using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct EnemyStats
    {
        private const int PowerToHealthMiltiplyer = 5;
        [SerializeField] private int _power;

        public int Power => _power;

        public int Health => _power * PowerToHealthMiltiplyer;
    }

    public enum EnemyType { Pigoblin, Bandit, BanditGirl}
}
