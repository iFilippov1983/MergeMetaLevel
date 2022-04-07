using DG.Tweening;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Chips, Event(EventTarget.Self), Cleanup(CleanupMode.RemoveComponent), FlagPrefix("has")]
public sealed class DoMoveComponent : IComponent
{
    public int x, y;
    public float speed;
    public Ease Easing;
}