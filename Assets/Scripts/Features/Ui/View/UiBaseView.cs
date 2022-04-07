using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class UiBaseView : MonoBehaviour
    {
        public static int FADE_IN_DURATION = 400;
        public static int FADE_OUT_DURATION = 400;
        
        // View
        public Canvas Canvas;
        public CanvasGroup CanvasGroup;
        public UiBlurView Blur;
        
        [SerializeField] private int fadeInDuration;
        [SerializeField] private int fadeOutDuration;
        public int BgDelay = 400;
        public event Action OnUpdate;

        public int FadeInDuration => fadeInDuration != 0 ? fadeInDuration : FADE_IN_DURATION;
        public  int FadeOutDuration => fadeOutDuration != 0 ? fadeOutDuration : FADE_OUT_DURATION;
        
        [Button]
        [ShowIf("@CanvasGroup == null || Canvas == null")]
        void Editor_AddComponents()
        {
            if (gameObject.GetComponent<Canvas>() == null)
                gameObject.AddComponent<Canvas>();
            if (gameObject.GetComponent<CanvasGroup>() == null)
                gameObject.AddComponent<CanvasGroup>();
            if (gameObject.GetComponent<GraphicRaycaster>() == null)
                gameObject.AddComponent<GraphicRaycaster>();

            Canvas = gameObject.GetComponent<Canvas>();
            CanvasGroup = gameObject.GetComponent<CanvasGroup>();
        }

        void Update() => OnUpdate?.Invoke();
    }

}