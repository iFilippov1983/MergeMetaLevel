using UnityEngine;

namespace Level
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Material _actualMaterial;
        [SerializeField] private MeshRenderer _cellBodyMeshRenderer;
        [SerializeField] private ParticleSystem _cellPassEffect;
        [SerializeField] private Transform _resourcePickupEffectSpawnPoint;
        [SerializeField] private Transform _enemySpawnPoint;
        [SerializeField] private Transform _fightCameraPositionRight;
        [SerializeField] private Transform _fightCameraPositionLeft;
        [SerializeField] private bool _rightFightCamPosition;

        public Material ActualMaterial => _actualMaterial;
        public MeshRenderer CellBodyMeshRenderer => _cellBodyMeshRenderer;
        public ParticleSystem CellPassEffect => _cellPassEffect;
        public Transform ResourcePickupEffectSpawnPoint => _resourcePickupEffectSpawnPoint;
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

