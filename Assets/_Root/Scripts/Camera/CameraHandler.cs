using Cinemachine;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCamera
{
    internal class CameraHandler
    {
        private CinemachineVirtualCamera _virtualCamFollow;
        private CinemachineVirtualCamera _virtualCamFight;
        private Transform _transformToFollow;

        private const int PriorityDefault = 10;
        private const int PriorityPassive = 0;
        private bool _fightMode;

        public CameraHandler(CameraContainerView cameraContainerView, Transform targetTransform)
        {
            _virtualCamFollow = cameraContainerView.VirtualCamFollow;
            _transformToFollow = targetTransform;
            _virtualCamFollow.Follow = _transformToFollow;
            _virtualCamFollow.LookAt = _transformToFollow;
            _virtualCamFight = cameraContainerView.VirtualCamFight;
            _fightMode = false;
        }

        internal async Task SwitchCamera(Transform transformToPlace = null)
        {
            if (_fightMode)
            {
                _fightMode = false;
                SetPriorities(_virtualCamFollow, _virtualCamFight, _transformToFollow);

                await Task.Delay(1500);//??
                //await Task.Delay(2000);
            }
            else
            { 
                _fightMode = true;
                SetPriorities(_virtualCamFight, _virtualCamFollow);
                _virtualCamFight.transform.position = transformToPlace.position;
                _virtualCamFight.transform.rotation = transformToPlace.rotation;

                await Task.Delay(1000);//??
                //await Task.Delay(2000);
            }
        }

        private void SetPriorities
            (
            CinemachineVirtualCamera currentCamera, 
            CinemachineVirtualCamera previousCamera, 
            Transform transformToFollow = null
            )
        {
            previousCamera.Priority = PriorityPassive;
            previousCamera.Follow = null;
            previousCamera.LookAt = null;

            currentCamera.Priority = PriorityDefault;
            currentCamera.Follow = transformToFollow;
            currentCamera.LookAt = transformToFollow;
        }
    }
}
