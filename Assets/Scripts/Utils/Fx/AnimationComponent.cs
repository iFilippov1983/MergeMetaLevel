using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Core
{
    public abstract class AnimationComponent
    {
        public abstract void Animate();
        public abstract void Stop();
        public abstract Task WaitEvent();
        public abstract Task WaitComplete();
    }
    
    public class SingleAnimationComponent : AnimationComponent
    {
        public Animation animation;
        public AnimatorEvent event1;

        public override void Animate()
        {
            animation.gameObject.SetActive(true);
        }

        public override async Task WaitEvent()
        {
            if(event1 != null)
                await event1.WaitEvent1();
        }

        public override async Task WaitComplete()
        {
            var duration = animation.clip.length;
            await Task.Delay(duration.ToMs());
        }

        public override void Stop()
        {
            animation.gameObject.SetActive(false);
        }
    }
    
    public class AnimatorComponent : AnimationComponent
    {
        public Animator animator;
        public string name;
        public AnimatorEvent event1;

        public override void Animate()
        {
            animator.enabled = true;
            animator.SetTrigger(name);
        }

        public override async Task WaitEvent()
        {
            if(event1 != null)
                await event1.WaitEvent1();
        }

        public override async Task WaitComplete()
        {
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            var duration = clipInfo[0].clip.length;
            await Task.Delay(duration.ToMs());
        }

        public override void Stop()
        {
            animator.enabled = false;
        }
    }
    
    
}