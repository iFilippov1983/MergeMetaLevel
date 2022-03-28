using Cinemachine;
using UnityEngine;

namespace GameCamera
{
    internal class CameraHandler
    {
        private Camera _mainCamera;
        private CinemachineVirtualCamera _virtualCamFollow;

        public CameraHandler(CameraContainerView cameraView, Transform targetTransform)
        {
            _mainCamera = cameraView.MainCamera;
            _virtualCamFollow = cameraView.VirtualCamFollow;
            _virtualCamFollow.Follow = targetTransform;
            _virtualCamFollow.LookAt = targetTransform;
        }

        internal void StopLookAndFollow()
        {
            _virtualCamFollow.Follow = null;
            _virtualCamFollow.LookAt = null;
        }

        internal void LookAndFollow(Transform targetTransform)
        {
            _virtualCamFollow.Follow = targetTransform;
            _virtualCamFollow.LookAt = targetTransform;
        }
    }
}
