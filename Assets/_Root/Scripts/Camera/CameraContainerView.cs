using Cinemachine;
using UnityEngine;

namespace GameCamera
{
    public class CameraContainerView : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamFollow;
        [SerializeField] private CinemachineVirtualCamera _virtualCamFight;

        public Camera MainCamera => _mainCamera;
        public CinemachineVirtualCamera VirtualCamFollow => _virtualCamFollow;
        public CinemachineVirtualCamera VirtualCamFight => _virtualCamFight;
    }
}
