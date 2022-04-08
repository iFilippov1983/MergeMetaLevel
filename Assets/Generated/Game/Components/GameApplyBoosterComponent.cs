//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ApplyBoosterComponent applyBooster { get { return (ApplyBoosterComponent)GetComponent(GameComponentsLookup.ApplyBooster); } }
    public bool hasApplyBooster { get { return HasComponent(GameComponentsLookup.ApplyBooster); } }

    public void AddApplyBooster(DamageInfo newValue) {
        var index = GameComponentsLookup.ApplyBooster;
        var component = (ApplyBoosterComponent)CreateComponent(index, typeof(ApplyBoosterComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceApplyBooster(DamageInfo newValue) {
        var index = GameComponentsLookup.ApplyBooster;
        var component = (ApplyBoosterComponent)CreateComponent(index, typeof(ApplyBoosterComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveApplyBooster() {
        RemoveComponent(GameComponentsLookup.ApplyBooster);
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
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherApplyBooster;

    public static Entitas.IMatcher<GameEntity> ApplyBooster {
        get {
            if (_matcherApplyBooster == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ApplyBooster);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherApplyBooster = matcher;
            }

            return _matcherApplyBooster;
        }
    }
}