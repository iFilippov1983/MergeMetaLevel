using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CharTween.Examples
{
    public class SppearTest : MonoBehaviour
    {
        public GameObject StarFx;
        public Image[] Stars;
        public float Interval;
        public float Interval2 = 0.2f;
        
        void OnEnable()
        {

            for (int i = 0; i < Stars.Length; i++)
            {
                StartCoroutine(Star(i));
            }
        }

        IEnumerator Star(int i)
        {
            yield return new WaitForSeconds(i * Interval);
            var uiStar = Stars[i];
            var s = DOTween.Sequence();
            s.Join(uiStar.DOFade(1, 0.5f).From());
            s.Join(uiStar.transform.DOScale(0, 0.5f).From().SetEase(Ease.OutBack, 5));
            s.InsertCallback(Interval2, () => Instantiate(StarFx, uiStar.transform));
        }
    }
}