using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public class DestroyOnDelay : MonoBehaviour
    {
        public int DelayMs = 1000;

        private async void Start()
        {
            await Task.Delay(DelayMs);
            Destroy(gameObject);
        }
    }
}