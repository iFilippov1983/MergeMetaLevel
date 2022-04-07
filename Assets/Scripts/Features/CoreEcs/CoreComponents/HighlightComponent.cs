using Entitas;
using Entitas.CodeGeneration.Attributes;

[Chips, FlagPrefix("has")]
[Event( EventTarget.Self, EventType.Added)]
[Event(EventTarget.Self, EventType.Removed)]
public class HighlightComponent : IComponent
{
    public int x, y;
}