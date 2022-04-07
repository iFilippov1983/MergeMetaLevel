using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique, Event(EventTarget.Self), FlagPrefix("do"), Cleanup(CleanupMode.DestroyEntity)]
public sealed class CreateLevelComponent : IComponent
{
    
}