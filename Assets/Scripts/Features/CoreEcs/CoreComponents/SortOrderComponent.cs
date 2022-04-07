using Entitas;
using Entitas.CodeGeneration.Attributes;

[Chips, Event(EventTarget.Self)]
public class SortOrderComponent : IComponent
{
    public int order;
}