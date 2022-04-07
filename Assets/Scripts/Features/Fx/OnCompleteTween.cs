using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class OnCompleteTween : MonoBehaviour, ITween
    {
        public UnityEvent Event;
        public float Duration = 0f;
        public float DurationOffset = 0;

        public async void DoTween(float duration)
        {
            var durationOffset = (GetDuration(duration) + DurationOffset);
            if (durationOffset < 0)
                throw new Exception($"{gameObject.name} duration < 0");
            
            await Task.Delay((int) (durationOffset* 1000f));
            Event?.Invoke();
        }
        private float GetDuration(float duration) => Duration != 0 ? Duration : duration;

    }
}