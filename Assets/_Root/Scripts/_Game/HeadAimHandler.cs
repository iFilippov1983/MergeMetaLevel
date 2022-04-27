using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Game
{
    internal class HeadAimHandler
    {
        private MultiAimConstraint _headAim;
        private Transform _headAimTarget;
        private float _aimSpeed = 0.075f;
        public HeadAimHandler(Transform target, MultiAimConstraint headAim)
        {
            _headAim = headAim;
            _headAimTarget = target;
        }

        public async Task LookAt(Transform target)
        { 
            _headAimTarget.position = target.position;
            while (_headAim.weight < 1)
            {
                _headAim.weight += _aimSpeed;
                await Task.Yield();
                _headAimTarget.position = target.position;
            }
        }

        public async void StopLooking()
        {
            while (_headAim.weight > 0)
            {
                _headAim.weight -= (_aimSpeed * 5f);
                await Task.Yield();
            }
        }
    }
}
