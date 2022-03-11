using TMPro;
using UnityEngine;

namespace Level
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private GameObject _textMeshProObject;
        [SerializeField] private GameObject _spriteObject;
        [SerializeField] private GameObject _particleSystemObject;

        public TextMeshPro TextMeshPro => _textMeshProObject.GetComponent<TextMeshPro>();
        public SpriteRenderer SpriteRenderer => _spriteObject.GetComponent<SpriteRenderer>();
        public ParticleSystem ParticleSystem => _particleSystemObject.GetComponent<ParticleSystem>();
    }
}

