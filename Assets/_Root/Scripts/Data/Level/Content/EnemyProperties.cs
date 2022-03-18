using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/EnemyProperties", fileName = "Enemy_name_Properties")]
    internal sealed class EnemyProperties : ContentProperties
    {
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private EnemyStats _enemyStats;

        public EnemyType EnemyType => _enemyType;
        public EnemyStats EnemyStats => _enemyStats;

        public override ContentType GetContentType() => ContentType.Enemy;
    }
}