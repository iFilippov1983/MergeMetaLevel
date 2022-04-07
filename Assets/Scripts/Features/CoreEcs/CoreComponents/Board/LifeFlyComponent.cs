using Entitas;
using Entitas.CodeGeneration.Attributes;

[Board, Event(EventTarget.Self)]
public class LifeFlyComponent : IComponent
{
     public int lifesContain;
     public int x;
     public int y;
}