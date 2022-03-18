using TMPro;
using UnityEngine;

namespace Level
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Transform _enemySpawnPoint;
        [SerializeField] private Transform _enemyFightPoint;

        public TextMeshPro TextMeshPro => _textMeshPro;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public ParticleSystem ParticleSystem => _particleSystem;
        public Transform EnemySpawnPoint => _enemySpawnPoint;
        public Transform EnemyFightPoint => _enemyFightPoint;
    }
}

