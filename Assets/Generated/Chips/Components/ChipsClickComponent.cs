//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ChipsEntity {

    static readonly ClickComponent clickComponent = new ClickComponent();

    public bool hasClick {
        get { return HasComponent(ChipsComponentsLookup.Click); }
        set {
            if (value != hasClick) {
                var index = ChipsComponentsLookup.Click;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : clickComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class ChipsMatcher {

    static Entitas.IMatcher<ChipsEntity> _matcherClick;

    public static Entitas.IMatcher<ChipsEntity> Click {
        get {
            if (_matcherClick == null) {
                var matcher = (Entitas.Matcher<ChipsEntity>)Entitas.Matcher<ChipsEntity>.AllOf(ChipsComponentsLookup.Click);
                matcher.componentNames = ChipsComponentsLookup.componentNames;
                _matcherClick = matcher;
            }

            return _matcherClick;
        }
    }
}