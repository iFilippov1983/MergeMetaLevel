using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Chips, Event(EventTarget.Self)]
public sealed class DamageComponent : IComponent
{
    public List<DamageInfo> values;
}