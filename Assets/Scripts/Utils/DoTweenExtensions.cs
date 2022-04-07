using System;
using DG.Tweening;
using DG.Tweening.Core;
using TMPro;

namespace Utils.DoTween
{
    public static class DoTweenExtensions
    {
        public static Tween TweenTo(this float from, float to, DOSetter<float> setter, float duration) =>
            DOTween.To(() => from, setter,  to, duration);
        public static Tween TweenTo(this int from, int to, DOSetter<int> setter, float duration) =>
            DOTween.To(() => from, setter,  to, duration);

        public class TextSetter
        {
            public int count;

            public TextSetter(int to)
                => count = to;
        }

        public static Tween DoInt(this TextMeshProUGUI text, int to, float duration)
        {
            var helper = new TextSetter( int.Parse(text.text) );
            
            void SetCount(int count)
            {
                helper.count = count;
                text.text = count.ToString();
            }
            
            return DOTween.To(() => helper.count, count => SetCount(count), to, duration);        
        }
        
        public static Tween DoInt(this int from, int to, float duration, Action<int> setText)
        {
            var helper = new TextSetter( from );
            
            void SetCount(int count)
            {
                helper.count = count;
                setText(count);
            }
            
            return DOTween.To(() => helper.count, count => SetCount(count), to, duration);        
        }
            
        
    }
}