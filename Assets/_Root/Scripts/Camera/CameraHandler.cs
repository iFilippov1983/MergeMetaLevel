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
        private Transform _fightCamTransform;

        private float _shakeTime = 1f;
        private float _shakeAmount = 3f;
        private float _shakeSpeed = 2f;
        private int PriorityDefault = 10;
        private int PriorityPassive = 0;
        private bool _fightMode;

        public CameraHandler(CameraContainerView cameraContainerView, Transform targetTransform)
        {
            _virtualCamFollow = cameraContainerView.VirtualCamFollow;
            _transformToFollow = targetTransform;
            _virtualCamFollow.Follow = _transformToFollow;
            _virtualCamFollow.LookAt = _transformToFollow;
            _virtualCamFight = cameraContainerView.VirtualCamFight;
            _fightCamTransform = _virtualCamFight.transform;
            _shakeTime = cameraContainerView.ShakeTime;
            _shakeAmount = cameraContainerView.ShakeAmount;
            _shakeSpeed = cameraContainerView.ShakeSpeed;
            _fightMode = false;
        }

        internal async Task SwitchCamera(Transform transformToPlace = null)
        {
            if (_fightMode)
            {
                _fightMode = false;
                SetPriorities(_virtualCamFollow, _virtualCamFight, _transformToFollow);

                await Task.Delay(1500);
            }
            else
            { 
                _fightMode = true;
                SetPriorities(_virtualCamFight, _virtualCamFollow);
                _virtualCamFight.transform.position = transformToPlace.position;
                _virtualCamFight.transform.rotation = transformToPlace.rotation;

                await Task.Delay(1000);
            }
        }

        internal async void ShakeCamera()
        { 
            Vector3 originPosition = _fightCamTransform.localPosition;
            float timeElapsed = 0f;
            while (timeElapsed < _shakeTime)
            {
                Vector3 randomPoint = originPosition + Random.insideUnitSphere * _shakeAmount;
                _fightCamTransform.localPosition = Vector3.Lerp(_fightCamTransform.localPosition, randomPoint, Time.deltaTime * _shakeSpeed);
                await Task.Yield();

                timeElapsed += Time.deltaTime;
            }
            _fightCamTransform.localPosition = originPosition;
        }

        private void SetPriorities
            (
            CinemachineVirtualCamera toCamera, 
            CinemachineVirtualCamera fromCamera, 
            Transform transformToFollow = null
            )
        {
            fromCamera.Priority = PriorityPassive;
            fromCamera.Follow = null;
            fromCamera.LookAt = null;

            toCamera.Priority = PriorityDefault;
            toCamera.Follow = transformToFollow;
            toCamera.LookAt = transformToFollow;
        }
    }
}
