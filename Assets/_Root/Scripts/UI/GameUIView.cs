using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace GameUI
{
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private Button _rollButton;
        [SerializeField] private Text _numberText;

        public Text NumberText => _numberText;

        public void Init(UnityAction rollButtonClicked)
        {
            _numberText = GetComponentInChildren<Text>();
            _rollButton.onClick.AddListener(rollButtonClicked);
        }

        private void OnDestroy()
        {
            _rollButton.onClick.RemoveAllListeners();
        }
    }
}
