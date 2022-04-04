using TMPro;
using UnityEngine;

namespace Level
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Material _actualMaterial;
        [SerializeField] private MeshRenderer _cellBodyMeshRenderer;
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Transform _enemySpawnPoint;
        [SerializeField] private Transform _fightCameraPositionRight;
        [SerializeField] private Transform _fightCameraPositionLeft;
        [SerializeField] private bool _rightFightCamPosition;

        public Material ActualMaterial => _actualMaterial;
        public MeshRenderer CellBodyMeshRenderer => _cellBodyMeshRenderer;
        public TextMeshPro TextMeshPro => _textMeshPro;
        public ParticleSystem ParticleSystem => _particleSystem;
        public Transform EnemySpawnPoint => _enemySpawnPoint;
        public Transform FightCameraPosition
        {
            get
            {
                if (_rightFightCamPosition) return _fightCameraPositionRight;
                else return _fightCameraPositionLeft;
            }
        }
    }
}

