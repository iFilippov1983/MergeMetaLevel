using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public class AnimatorEvent : MonoBehaviour
    {
        private bool _event1;
        
        public void CallEvent1()
        {
            _event1 = true;
        }
        
        public async Task WaitEvent1()
        {
            _event1 = false;
            while (!_event1)
                await Task.Yield();
        }
    }
}