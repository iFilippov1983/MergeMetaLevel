﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CharTween
{
    public static class CharTweenerUtility
    {
        private static readonly Dictionary<TMP_Text, CharTweener> CharModifiers = new Dictionary<TMP_Text, CharTweener>();

        /// <summary>
        /// Returns a <see cref="CharTweener"/> guaranteeing the same instance is used for the same text.
        /// </summary>
        public static CharTweener GetCharTweener(this TMP_Text text, GameObject attachTo)
        {
//            if (CharModifiers.ContainsKey(text))
//                return CharModifiers[text];

            var modifier = CharModifiers[text] = attachTo.AddComponent<CharTweener>();
            modifier.Text = text;
            modifier.Initialize();
            return modifier;
        }
        
        public static CharTweener GetCharTweener(this TMP_Text text)
        {
            return GetCharTweener(text, text.gameObject);
        }

        public static void CharTweenerClear(this TMP_Text text)
        {
            var modifier = text.gameObject.GetComponent<CharTweener>();
            Object.Destroy(modifier);
        }
        
        
    }
}
