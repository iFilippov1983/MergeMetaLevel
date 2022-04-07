using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Main
{
    public class UiMainScreenLevel : MonoBehaviour
    {
        public Color Gray;
        public Color Green;
        public Color Blue;
        public Color TextGray;
        public Color TextGreen;
        public Color TextBlue;
        
        public Image Color;
        public TextMeshProUGUI Level;
        public RectTransform Road;
        public RectTransform Bg;
        public RectTransform Checker;
        public RectTransform Container;

        public UiMainScreenPointData PointData;

        private const float ChangeColorTime = 0.4f;

        public void SetGreen()
        {
            Color.color = Green;
            Level.color = TextGreen;
            Checker.gameObject.SetActive(false);
        }
        public void SetBlue()
        {
            Color.color = Blue;
            Level.gameObject.SetActive(false);
            Checker.gameObject.SetActive(true);
        }
        
        public void SetGray()
        {
            Color.color = Gray;
            Level.color = TextGray;
            Checker.gameObject.SetActive(false);
        }

        public void SetHidden()
        {
            Bg.gameObject.SetActive(false);
            Color.gameObject.SetActive(false);
            Level.gameObject.SetActive(false);
            Checker.gameObject.SetActive(false);
        }

        public void ToGreen()
        {
            Color.DOColor(Green, ChangeColorTime);
            Level.DOColor(TextGreen, ChangeColorTime);
            Checker.gameObject.SetActive(false);
        }

        public async Task ToChecked()
        {
            Level.DOScale(0, 0.2f).OnComplete(() => Level.gameObject.SetActive(false));
            await Task.Delay(200);
            Checker.gameObject.SetActive(true);
            await Task.Delay(600);
        }
        
        public void ToBlue()
        {
            Color.DOColor(Blue, ChangeColorTime);
            Level.DOColor(TextBlue, ChangeColorTime);
        }
        
        public void ToGray()
        {
            Color.DOColor(Gray, ChangeColorTime);
            Level.DOColor(TextGray, ChangeColorTime);
            Checker.gameObject.SetActive(false);
        }
        
    }
}