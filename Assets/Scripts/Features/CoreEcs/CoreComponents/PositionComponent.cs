using Entitas;
using Entitas.CodeGeneration.Attributes;

[Chips, Event(EventTarget.Self)]
public sealed class PositionComponent : IComponent
{
    public int x; 
    public int y; 
}
