using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class MoneyTriggerController : MonoBehaviour
    {
        [SerializeField] private Transform referenceTransform;
        private Camera _camera;
        private Transform _thisTransform;

        private void Start()
        {
            _camera = Camera.main;
            _thisTransform = transform;
        }

        private void Update()
        {
            var pos = _camera.ViewportToWorldPoint(
                new Vector3(
                    0.1f,
                    0.94f,
                    _camera.transform.position.GetDistanceTo(referenceTransform.position))
            );

            _thisTransform.position = pos;
        }
    }

    public static class VectorExtention
    {
        public static float GetDistanceTo(this Vector3 pos1, Vector3 pos2)
        {
            Vector3 heading;

            heading.x = pos1.x - pos2.x;
            heading.y = pos1.y - pos2.y;
            heading.z = pos1.z - pos2.z;

            var distanceSquared = heading.x * heading.x + heading.y * heading.y + heading.z * heading.z;

            return Mathf.Sqrt(distanceSquared);
        }
    }
}