using System.Linq;
using Api.Ui;
using Components;
using Components.Api.Tutorial;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs.Tutorial
{
    
    public class TutorialStep : SerializedScriptableObject
    {
        public TutorialTrigger Trigger;
        public TutorialAction Action;
        public TutorialUiData UiData;
        public bool SaveAfterComplete;

        public static TutorialStep edited;

        [Button(ButtonSizes.Large)]
        void Edit()
        {
            edited = this;
            var tutUI = Object.FindObjectOfType<UiTutorialView>(true);
            tutUI.Api._camera = Object.FindObjectsOfType<Camera>(true).Where(o => o.CompareTag("MergeCamera")).First();
            tutUI.Api._step = this;
            tutUI.Api.Editor_Redraw();
        }
        [Button(ButtonSizes.Large)]
        void UiLayerOn()
        {
            edited = this;
            var tutUI = Object.FindObjectOfType<UiTutorialView>(true);
            tutUI.Api._camera = Object.FindObjectsOfType<Camera>(true).Where(o => o.CompareTag("MergeCamera")).First();
            tutUI.Api._step = this;
            tutUI.Api.Test_ChageUiTargetLayer();
        }
        [Button(ButtonSizes.Large)]
        void UiLayerOff()
        {
            edited = this;
            var tutUI = Object.FindObjectOfType<UiTutorialView>(true);
            tutUI.Api._camera = Object.FindObjectsOfType<Camera>(true).Where(o => o.CompareTag("MergeCamera")).First();
            tutUI.Api._step = this;
            tutUI.Api.Test_RestoreUiTargetLayer();
        }

        [Button(ButtonSizes.Large)]
        void Play_Tutor() 
            => TutorialApi.instance.Test_PlayTutor(this);
    
        [Button(ButtonSizes.Large)]
        void Stop_Tutor() 
            => TutorialApi.instance.Test_StopTutor(this);
    }

    public class TutorialUiData
    {
        [Space]
        [OnValueChanged("UiRedraw")]
        public bool ShowHand;
        [ShowIf("@ShowHand")]
        [OnValueChanged("UiRedraw")]
        public Vector2Int HandOffset;    
        [ShowIf("@ShowHand")]
        // [PropertyRange(-180, 180)]
        [Range(-180, 180)]
        [OnValueChanged("UiRedraw")]
        public int HandAngle;
        
        [Space]
        [OnValueChanged("UiRedraw")]
        public bool ShowArrow;
        [ShowIf("@ShowArrow")]
        [OnValueChanged("UiRedraw")]
        public Vector2Int ArrowOffset;    
        [ShowIf("@ShowArrow")]
        [OnValueChanged("UiRedraw")]
        [Range(-180, 180)]
        public int ArrowAngle;
        
        [Space]
        public bool ShowText;
        
        [OnValueChanged("UiRedraw")]
        [ShowIf("@ShowText")]
        public Vector2Int TextOffset;    

        [OnValueChanged("UiRedraw")]
        [ShowIf("@ShowText")]
        public OffsetType TextOffsetType;
        
        [OnValueChanged("UiRedraw")]
        [ShowIf("@ShowText")]
        [Multiline(5)]
        public string Text;

        public enum OffsetType
        {
            FromCenter,
            FromTarget,
        }
        public class TutorialText
        {
            public Vector2Int TextOffset;    
            public string Text;
        }

        void UiRedraw()
        {
            var tutUI = Object.FindObjectOfType<UiTutorialView>(true);
            tutUI.Api.Editor_Redraw();
        }
   
    }
    
}