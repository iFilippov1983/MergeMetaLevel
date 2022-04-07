using UnityEngine;

namespace Core
{
    public class DestroyByAnimator : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}