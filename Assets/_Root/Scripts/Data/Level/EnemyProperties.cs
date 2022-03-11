using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/EnemyProperties", fileName = "Enemy_name_Properties")]
    public class EnemyProperties : ScriptableObject, ICellContentProperties
    {
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private EnemyStats _enemyStats;

        public EnemyType EnemyType => _enemyType;
        public EnemyStats EnemyStats => _enemyStats;
        public ContentType GetContentType() => ContentType.Enemy;
    }

    [Serializable]
    public struct EnemyStats
    {
        public int Health;
        public int Armor;
        public int Power;
    }
}