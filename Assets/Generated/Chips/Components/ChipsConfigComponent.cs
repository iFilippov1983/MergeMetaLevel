//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ChipsEntity {

    public ConfigComponent config { get { return (ConfigComponent)GetComponent(ChipsComponentsLookup.Config); } }
    public bool hasConfig { get { return HasComponent(ChipsComponentsLookup.Config); } }

    public void AddConfig(Data.MergeItemConfig newValue) {
        var index = ChipsComponentsLookup.Config;
        var component = (ConfigComponent)CreateComponent(index, typeof(ConfigComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceConfig(Data.MergeItemConfig newValue) {
        var index = ChipsComponentsLookup.Config;
        var component = (ConfigComponent)CreateComponent(index, typeof(ConfigComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveConfig() {
        RemoveComponent(ChipsComponentsLookup.Config);
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

    static Entitas.IMatcher<ChipsEntity> _matcherConfig;

    public static Entitas.IMatcher<ChipsEntity> Config {
        get {
            if (_matcherConfig == null) {
                var matcher = (Entitas.Matcher<ChipsEntity>)Entitas.Matcher<ChipsEntity>.AllOf(ChipsComponentsLookup.Config);
                matcher.componentNames = ChipsComponentsLookup.componentNames;
                _matcherConfig = matcher;
            }

            return _matcherConfig;
        }
    }
}