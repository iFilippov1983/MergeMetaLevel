//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ChipsEntity {

    static readonly DeleteComponent deleteComponent = new DeleteComponent();

    public bool toDelete {
        get { return HasComponent(ChipsComponentsLookup.Delete); }
        set {
            if (value != toDelete) {
                var index = ChipsComponentsLookup.Delete;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : deleteComponent;

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
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ChipsEntity : IDeleteEntity { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class ChipsMatcher {

    static Entitas.IMatcher<ChipsEntity> _matcherDelete;

    public static Entitas.IMatcher<ChipsEntity> Delete {
        get {
            if (_matcherDelete == null) {
                var matcher = (Entitas.Matcher<ChipsEntity>)Entitas.Matcher<ChipsEntity>.AllOf(ChipsComponentsLookup.Delete);
                matcher.componentNames = ChipsComponentsLookup.componentNames;
                _matcherDelete = matcher;
            }

            return _matcherDelete;
        }
    }
}