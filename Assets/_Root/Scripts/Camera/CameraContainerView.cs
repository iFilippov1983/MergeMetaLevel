using Cinemachine;
using UnityEngine;

namespace GameCamera
{
    public class CameraContainerView : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamFollow;

        public Camera MainCamera => _mainCamera;
        public CinemachineVirtualCamera VirtualCamFollow => _virtualCamFollow;
    }
}
