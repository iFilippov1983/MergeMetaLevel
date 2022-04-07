using Entitas;
using Entitas.CodeGeneration.Attributes;

[Event(EventTarget.Self), Chips, Game, Cleanup(CleanupMode.DestroyEntity)]
[FlagPrefix("to")]
public sealed class DeleteComponent : IComponent
{
    
}