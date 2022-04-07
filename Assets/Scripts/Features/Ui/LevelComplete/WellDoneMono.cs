using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils.Text;

namespace Api.Ui.LevelComplete
{
    public class WellDoneMono : MonoBehaviour
    {
        public Tweened_TextMeshPro TweenText;
        
        private void OnEnable()
        {
            // TweenText.InitClear(); 
            // TweenText.ShowText();        
        }

        [Button]
        public void ShowText()
        {
            // TweenText.InitClear(); 
            TweenText.ShowText();
        }
        
        public void HideText()
        {
            TweenText.HideText();
        }
    }
}