using System;

namespace Data
{
    [Serializable]
    public struct EnemyStats
    {
        public int Power;
        public int Health;
    }

    public enum EnemyType { Barbarian, Rogue, Witch }
}
