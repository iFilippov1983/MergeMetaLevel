using UnityEngine;
using TMPro;

namespace Game
{
    public class PopupView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _popupText;

        public TextMeshPro PopupText => _popupText;
    }
}
