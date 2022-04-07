using DG.Tweening;
using TMPro;
using UnityEngine;
using Utils.DoTween;

namespace Api.Ui.LevelComplete
{
    public class CoinsMono : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public float Duration;
        private int _count;
        
        public void DoCount(int count)
        {
            _count = 0;
            Text.text = "0";
            Text.DoInt(count, Duration);
            // DOTween.To(() => _count, count => SetCount(count), _targetCount, Duration);        
        }
        
    }
}