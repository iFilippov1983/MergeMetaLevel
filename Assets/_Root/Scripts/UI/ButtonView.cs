using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _fadeColor;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _unactiveStateSprite;
        [SerializeField] private Sprite _defaultImageSprite;
        [SerializeField] private Sprite _fightImageSprite;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private List<TextMeshProUGUI> _textList;
        [SerializeField] private List<Image> _imageList;

        public Button Button => _button;
        public Image ButtonImage => _buttonImage;
        public Color DefaultColor => _defaultColor;
        public Color FadeColor => _fadeColor;
        public Sprite DefaultSprite => _defaultSprite;
        public Sprite UnactiveStateSprite => _unactiveStateSprite;
        public Sprite ImageSpriteDefault => _defaultImageSprite;
        public Sprite ImageSpriteFight => _fightImageSprite;
        public TextMeshProUGUI ButtonText => _buttonText; 
        public List<TextMeshProUGUI> TextList => _textList;
        public List<Image> ImageList => _imageList;
    }
}
