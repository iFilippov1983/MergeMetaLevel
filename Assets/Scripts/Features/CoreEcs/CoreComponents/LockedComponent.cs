 
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Chips, Event(EventTarget.Self), FlagPrefix("is")]
public class LockedComponent : IComponent
{
}