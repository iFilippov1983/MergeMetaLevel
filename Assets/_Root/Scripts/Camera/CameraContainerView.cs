using Cinemachine;
using Game;
using UnityEngine;

namespace GameCamera
{
    public class CameraContainerView : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamFollow;
        [SerializeField] private CinemachineVirtualCamera _virtualCamFight;
        [SerializeField] private CinemachineVirtualCamera _virtualCamIdle;
        [SerializeField] private Dice3D _dice;
        [Space]
        [Range(0f, 10f)]
        [SerializeField] private float _shakeTime = 1f;
        [Range(0f, 10f)]
        [SerializeField] private float _shakeAmount = 3f;
        [Range(0f, 10f)]
        [SerializeField] private float _shakeSpeed = 2f;

        public Camera MainCamera => _mainCamera;
        public CinemachineVirtualCamera VirtualCamFollow => _virtualCamFollow;
        public CinemachineVirtualCamera VirtualCamFight => _virtualCamFight;
        public CinemachineVirtualCamera VirtualCamIdle => _virtualCamIdle;
        public Dice3D Dice => _dice;
        public float ShakeTime => _shakeTime;
        public float ShakeAmount => _shakeAmount;
        public float ShakeSpeed => _shakeSpeed;

    }
}
