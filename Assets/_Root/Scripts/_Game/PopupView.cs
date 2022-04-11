using UnityEngine;
using TMPro;

namespace Game
{
    public class PopupView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _popupText;
        [SerializeField] private Animation _animation;

        public TextMeshPro PopupText => _popupText;
        public Animation Animation => _animation;
    }
}
