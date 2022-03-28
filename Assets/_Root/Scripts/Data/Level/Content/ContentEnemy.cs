using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using UnityEngine;

namespace Data
{
    [Serializable]
    internal sealed class ContentEnemy : ContentData
    {
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private EnemyStats _enemyStats;
        [SerializeField] private List<ResourceProperties> _rewardList;
        [SerializeField] private string _infoPrefabName;

        private GameObject _infoPrefab;

        public EnemyType EnemyType => _enemyType;
        public EnemyStats Stats => _enemyStats;
        public List<ResourceProperties> Reward => _rewardList;

        public override ContentType GetContentType() => ContentType.Enemy;

        public GameObject InfoPrefab
        {
            get
            {
                if (_infoPrefab == null) _infoPrefab =
                             Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _infoPrefabName));
                return _infoPrefab;
            }
        }
    }
}
