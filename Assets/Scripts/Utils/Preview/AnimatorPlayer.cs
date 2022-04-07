using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    [ExecuteInEditMode]
    public class AnimatorPlayer : MonoBehaviour
    {
        public Animator Animator;
        // public string Trigger;
        
        [ValueDropdown("AvailableAnimations")]
        public string Animation;
        
        private List<ValueDropdownItem<string>> AvailableAnimations()
        {
            var res = new List<ValueDropdownItem<string>>();
            foreach (AnimationClip clip in Animator.runtimeAnimatorController.animationClips)
                res.Add(new ValueDropdownItem<string>(clip.name, clip.name));
            
            return res;
        }
        
        [Button]
        public void CrossFade()
        {
            // Animator.CrossFade(Animation, 0.2f);
            Animator.CrossFadeInFixedTime(Animation, 0.25f);
        }

        
        
        [Button]
        public void Print()
        {
            foreach(AnimationClip clip in Animator.runtimeAnimatorController.animationClips)
                Debug.Log(clip.name);
        }
        //
        // [Button]
        // public void SetTrigger()
        // {
        //     Animator.SetTrigger(Trigger);
        // }

        // private void Update()
        // {
        //     Animator.Update(Time.deltaTime);
        // }
    }
}