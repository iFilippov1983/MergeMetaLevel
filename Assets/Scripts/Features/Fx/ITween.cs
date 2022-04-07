using System.Collections.Generic;

namespace Core
{
    public interface ITween
    {
        void DoTween(float duration);
    }

    public interface IGetTweens
    {
        void GetTweens(List<DG.Tweening.Tween> list);
    }
}