using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _fadeColor;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _unactiveStatePrite;
        [SerializeField] private List<TextMeshProUGUI> _textList;
        [SerializeField] private List<Image> _imageList;

        public Button Button => _button;
        public Color DefaultColor => _defaultColor;
        public Color FadeColor => _fadeColor;
        public Sprite DefaultSprite => _defaultSprite;
        public Sprite UnactiveStatePrite => _unactiveStatePrite;
        public List<TextMeshProUGUI> TextList => _textList;
        public List<Image> ImageList => _imageList;
    }
}
