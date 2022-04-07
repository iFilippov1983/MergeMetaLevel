
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Chips, Event(EventTarget.Self), Cleanup(CleanupMode.RemoveComponent), FlagPrefix("has")]
public class ClickComponent : IComponent
{
    
}