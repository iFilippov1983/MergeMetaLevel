using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class OnStartTween : MonoBehaviour, ITween
    {
        public UnityEvent Event;

        public void DoTween(float duration)
        {
            Event?.Invoke();
        }
    }
}