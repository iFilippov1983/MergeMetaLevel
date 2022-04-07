using Sirenix.OdinInspector;
using UnityEngine;

namespace Tutorial.Game
{
    public class RuleItem : MonoBehaviour
    {
        public RuleEnum RuleEnum;

        [HideLabel]
        [OnValueChanged("SetSprite")]
        [PreviewField(100, ObjectFieldAlignment.Center )]
        public Sprite Sprite;
        
        [HideLabel]
        [OnValueChanged("SetSprite")]
        [HorizontalGroup("1")]
        [PreviewField( ObjectFieldAlignment.Right )]
        public Sprite ltSprite;
        
        [HideLabel]
        [OnValueChanged("SetSprite")]
        [HorizontalGroup("1")]
        [PreviewField( ObjectFieldAlignment.Left )]
        public Sprite rtSprite;
        
        [HideLabel]
        [OnValueChanged("SetSprite")]
        [HorizontalGroup("2")]
        [PreviewField( ObjectFieldAlignment.Right )]
        public Sprite rbSprite;
        
        [HideLabel]
        [OnValueChanged("SetSprite")]
        [HorizontalGroup("2")]
        [PreviewField( ObjectFieldAlignment.Left )]
        public Sprite lbSprite;
        
        public SpriteRenderer Preview;
        public SpriteRenderer ltPreview;
        public SpriteRenderer rtPreview;
        public SpriteRenderer rbPreview;
        public SpriteRenderer lbPreview;
        
        void SetSprite()
        {
            Debug.Log("Value Changed");
            Preview.sprite = Sprite;
            ltPreview.sprite = ltSprite;
            rtPreview.sprite = rtSprite;
            rbPreview.sprite = rbSprite;
            lbPreview.sprite = lbSprite;
            
            Preview.gameObject.SetActive(Sprite != null);
            ltPreview.gameObject.SetActive(ltSprite != null);
            rtPreview.gameObject.SetActive(rtSprite != null);
            rbPreview.gameObject.SetActive(rbSprite != null);
            lbPreview.gameObject.SetActive(lbSprite != null);
        }
    }
}