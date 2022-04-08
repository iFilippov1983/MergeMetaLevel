//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ChipsEntity {

    public ChipsDeleteListenerComponent chipsDeleteListener { get { return (ChipsDeleteListenerComponent)GetComponent(ChipsComponentsLookup.ChipsDeleteListener); } }
    public bool hasChipsDeleteListener { get { return HasComponent(ChipsComponentsLookup.ChipsDeleteListener); } }

    public void AddChipsDeleteListener(System.Collections.Generic.List<IChipsDeleteListener> newValue) {
        var index = ChipsComponentsLookup.ChipsDeleteListener;
        var component = (ChipsDeleteListenerComponent)CreateComponent(index, typeof(ChipsDeleteListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceChipsDeleteListener(System.Collections.Generic.List<IChipsDeleteListener> newValue) {
        var index = ChipsComponentsLookup.ChipsDeleteListener;
        var component = (ChipsDeleteListenerComponent)CreateComponent(index, typeof(ChipsDeleteListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveChipsDeleteListener() {
        RemoveComponent(ChipsComponentsLookup.ChipsDeleteListener);
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

    static Entitas.IMatcher<ChipsEntity> _matcherChipsDeleteListener;

    public static Entitas.IMatcher<ChipsEntity> ChipsDeleteListener {
        get {
            if (_matcherChipsDeleteListener == null) {
                var matcher = (Entitas.Matcher<ChipsEntity>)Entitas.Matcher<ChipsEntity>.AllOf(ChipsComponentsLookup.ChipsDeleteListener);
                matcher.componentNames = ChipsComponentsLookup.componentNames;
                _matcherChipsDeleteListener = matcher;
            }

            return _matcherChipsDeleteListener;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ChipsEntity {

    public void AddChipsDeleteListener(IChipsDeleteListener value) {
        var listeners = hasChipsDeleteListener
            ? chipsDeleteListener.value
            : new System.Collections.Generic.List<IChipsDeleteListener>();
        listeners.Add(value);
        ReplaceChipsDeleteListener(listeners);
    }

    public void RemoveChipsDeleteListener(IChipsDeleteListener value, bool removeComponentWhenEmpty = true) {
        var listeners = chipsDeleteListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveChipsDeleteListener();
        } else {
            ReplaceChipsDeleteListener(listeners);
        }
    }
}