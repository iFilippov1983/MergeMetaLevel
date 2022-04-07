using System;
using UnityEngine;

namespace Tutorial.Game
{
    public class BgItemMask : MonoBehaviour
    {
        public SpriteMask SpriteRenderer;
        public SpriteMask lt;
        public SpriteMask rt;
        public SpriteMask rb;
        public SpriteMask lb;
        
        public void InitBy(RuleItem rule, Func<Sprite, Sprite> cbGetSprite)
        {
            SpriteRenderer.sprite = cbGetSprite(rule.Sprite);
            lt.sprite = cbGetSprite(rule.ltSprite);
            rt.sprite = cbGetSprite(rule.rtSprite);
            rb.sprite = cbGetSprite(rule.rbSprite);
            lb.sprite = cbGetSprite(rule.lbSprite);
            
            name = rule.name;
            
            SpriteRenderer.gameObject.SetActive( rule.Sprite != null);
            lt.gameObject.SetActive( rule.ltSprite != null);
            rt.gameObject.SetActive( rule.rtSprite != null);
            rb.gameObject.SetActive( rule.rbSprite != null);
            lb.gameObject.SetActive( rule.lbSprite != null);
        }
    }
}