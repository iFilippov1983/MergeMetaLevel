using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Core;
using Data;
using Features._Events;

[Unique, Game]
public sealed class CtxComponent : IComponent, IClear
{
    public MergePlayerLinks links;
    public CoreServices services;
    // public BoardState board;
    public MergeDynamicData dynamicData;
    public MergeEvents events;
    public MergeVisualConfig visualConfig;
    public MergeRules mergeRules;
    public MergeConfig mergeConfig;
        
    // public int level;
    // public LevelConfig levelConfig;
    public LevelConfig levelConfig;

    private List<IClear> _clearList = new List<IClear>();
    private List<ITearDownSystem> _tearDownList = new List<ITearDownSystem>();
    
    public void Clear()
    {
        _clearList.ForEach(d => d.Clear());
        
        dynamicData.Clear();
        events.Clear();
    }

    public void TearDown()
    {
        _tearDownList.ForEach(d => d.TearDown());
        
        _clearList.Clear();
        _tearDownList.Clear();
    }

    public T AddListener<T>(T t)
    {
        if(t is IClear)
            _clearList.Add(t as IClear);
        if(t is ITearDownSystem)
            _tearDownList.Add(t as ITearDownSystem);
        return t;
    }
}