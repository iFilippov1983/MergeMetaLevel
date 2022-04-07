using Api.Ui.LevelComplete;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Text;

namespace Components
{
    public class UiLevelCompleteView : UiBaseView
    {
        public RectTransform CoinPrefabFx;
        public Tweened_TextMeshPro LevelCompleteHeader;
        public TextMeshProUGUI CoinsCount;
        public TextMeshProUGUI HeartsCount;
        public RectTransform RewardForLevel;
        public RectTransform RewardForMoves;
        public CoinsMono CoinsForLevel;
        public CoinsMono CoinsForMoves;
        public Button ContinueButton;

        public float ButtonDelay = 1;
        
        public float ShakeDuration = 0.8f;
        public float ShakeStrength = 0.2f;
        public int ShakeVibrato = 4;

        // Api
        public UiLevelCompleteNewApi Api;
        
    }
}