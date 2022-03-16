using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

namespace GameCamera
{
    internal class VirtualCameraHandler
    {
        private CinemachineVirtualCamera _virtualCamFollow;
        private CinemachineVirtualCamera _virtualCamEvent;

        public VirtualCameraHandler(Transform targetTransform)
        {
            _virtualCamFollow = Object.FindObjectOfType<CinemachineVirtualCamera>();
            _virtualCamFollow.Follow = targetTransform;
            _virtualCamFollow.LookAt = targetTransform;
        }
    }
}
