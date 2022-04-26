using Cinemachine;
using Lofelt.NiceVibrations;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCamera
{
    internal class CameraHandler
    {
        private CinemachineVirtualCamera _virtualCamFollow;
        private CinemachineVirtualCamera _virtualCamFight;
        private CinemachineVirtualCamera _virtualCamIdle;
        private CinemachineVirtualCamera _currentCam;
        private CinemachineVirtualCamera _previousCam;
        private Transform _transformToFollow;
        private Transform _fightCamTransform;

        private float _shakeTime = 1f;
        private float _shakeAmount = 3f;
        private float _shakeSpeed = 2f;
        private int PriorityDefault = 10;
        private int PriorityPassive = 0;

        public CameraHandler(CameraContainerView cameraContainerView, Transform targetTransform)
        {
            _transformToFollow = targetTransform;
            _virtualCamFollow = cameraContainerView.VirtualCamFollow;
            _virtualCamFollow.Follow = _transformToFollow;
            _virtualCamFollow.LookAt = _transformToFollow;
            _virtualCamFight = cameraContainerView.VirtualCamFight;
            _fightCamTransform = _virtualCamFight.transform;
            _virtualCamIdle = cameraContainerView.VirtualCamIdle;
            _shakeTime = cameraContainerView.ShakeTime;
            _shakeAmount = cameraContainerView.ShakeAmount;
            _shakeSpeed = cameraContainerView.ShakeSpeed;
            _currentCam = _virtualCamFollow;
        }

        internal async Task SwitchCamera(bool fightMode, bool idleMode, Transform transformToPlace = null)
        {
            if (fightMode)
            {
                _previousCam = _currentCam;
                _currentCam = _virtualCamFight;
            }
            if (idleMode)
            {
                _previousCam = _currentCam;
                _currentCam = _virtualCamIdle;
            }

            if (fightMode || idleMode)
            {
                _currentCam.transform.position = transformToPlace.position;
                _currentCam.transform.rotation = transformToPlace.rotation;
                SetPriorities(_currentCam, _previousCam);

                await Task.Delay(1000);
            }
            else
            {
                _previousCam = _currentCam;
                _currentCam = _virtualCamFollow;
                SetPriorities(_currentCam, _previousCam, _transformToFollow);

                //await Task.Delay(1500);
            }
        }

        internal async void ShakeCamera()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);

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

        public void ChangeCameraFollowStatus(bool isFollowing)
        {
            if (isFollowing)
            {
                _virtualCamFollow.Follow = _transformToFollow;
                _virtualCamFollow.LookAt = _transformToFollow;
            }
            else
            {
                _virtualCamFollow.Follow = null;
                _virtualCamFollow.LookAt = null;
            }

            Debug.Log("follow: " + _virtualCamFollow.Follow);
            Debug.Log("lookAt: " + _virtualCamFollow.LookAt);
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
