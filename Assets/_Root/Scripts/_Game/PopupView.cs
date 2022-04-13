using UnityEngine;
using TMPro;

namespace Game
{
    public class PopupView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _popupText;
        [SerializeField] private Animation _animation;
        [SerializeField] private AnimationClip _animationClipFly;
        [SerializeField] private AnimationClip _animationClipHide;
        [SerializeField] private Canvas _canvas;

        public TextMeshPro PopupText => _popupText;
        public Animation Animation => _animation;
        public AnimationClip ClipFly => _animationClipFly;
        public AnimationClip ClipHide => _animationClipHide;
        public Canvas Canvas => _canvas;
    }

    public enum PopupType { Damage, Resource, Moves }
}
