using UnityEngine;

namespace Game
{
    public class Dice3D : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public Animator Animator => _animator;
    }
}
