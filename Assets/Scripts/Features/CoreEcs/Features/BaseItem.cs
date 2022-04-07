using System.Threading.Tasks;
using Core;
using Data;
using DG.Tweening;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public abstract class BaseItem 
{
    protected MergeItemView _view;
    protected ViewFactoryService _viewFactory;
    protected Contexts _contexts;
    protected MergeItemProfileData _data;
    protected MergeItemConfig _config;
    
    public MergeItemView view => _view;

    public BaseItem(ViewFactoryService viewFactory, MergeItemConfig config, MergeItemProfileData levelData, Contexts contexts)
    {
        _viewFactory = viewFactory;
        _config = config;
        _data = levelData;
        _contexts = contexts;
    }

    protected void DoHighlight(ChipsEntity entity, int x, int y) 
        => view.StartHighlight(new Vector3(x, y));

    protected void DoHighlightRemoved(ChipsEntity entity) 
        => view.StopHighlight();

    protected void DoMove(ChipsEntity entity, int x, int y, float speed, Ease easing)
    {
        var dist = Vector2.Distance(_view.Transform.position, new Vector2(x, y));
        var duration = dist * speed;
            
        // entity.ReplaceSortOrder(10);
        // _view.SetSortOrder(CoreConstants.sortOrderTop);
        _view.Transform.DOKill();
        _view.Transform.DOMove(new Vector2(x, y), 0.4f).SetEase(easing).OnComplete(() =>
        {
            // entity.ReplaceSortOrder(0);
            // _view.SetSortOrder(0);
        });
    }
    

    protected void CreateView(ChipsEntity entity)
    {
        _view = _viewFactory.CreateItemView(_data);
        _view.gameObject.Link(entity);
        entity.OnDestroyEntity += DeleteView;

        entity.AddView(_view);
    }

    protected virtual void DeleteView(IEntity en)
    {
        var e = en as ChipsEntity;
        _view.gameObject.Unlink();
        _viewFactory.Destroy(_view);
        e.OnDestroyEntity -= DeleteView;
    }

    protected ChipsEntity CreateEntityAddComponents()
    {
        var e = _contexts.chips.CreateEntity();
        e.AddFeature(this);
        e.AddChipInfo(_data, _config);
        e.AddData(_data);
        e.AddConfig(_config);
        e.DoAddPos(_contexts, _data.x, _data.y); // В конфигах LUA индексы начинаются с 1 (единицы)
        e.isLocked = _data.locked;

        // e.isFirstTime = true; // Маркер что объект только что создан
        // e.needSortingY = true;
        // e.isBaseLayer = true; // Основные чипы, не ковёр, не подложки
        return e;
    }

    protected void AddListeners(ChipsEntity entity, BaseItem item)
    {
        // switch (item)
        // {
            // case (item is IPositionListener) : entity.AddPositionListener(item as IPositionListener); break;
            // IHighlightListener _ => entity.AddHighlightListener(item as IHighlightListener)
        // };
        
        if(item is IPositionListener)
            entity.AddPositionListener(item as IPositionListener);
        // if(item is IEndMoveListener)
            // entity.AddEndMoveListener(chip as IEndMoveListener);
        if(item is IClickListener)
            entity.AddClickListener(item as IClickListener);
        if(item is IHighlightListener)
            entity.AddHighlightListener(item as IHighlightListener);
        if(item is IHighlightRemovedListener)
            entity.AddHighlightRemovedListener(item as IHighlightRemovedListener);
        if(item is IDamageListener)
            entity.AddDamageListener(item as IDamageListener);
        if(item is IDoMoveListener)
            entity.AddDoMoveListener(item as IDoMoveListener);
        if(item is ISortOrderListener)
            entity.AddSortOrderListener(item as ISortOrderListener);
    }

    protected void UpdateName(ChipsEntity entity) 
        => _view.gameObject.name = $"_{entity.position.x}_{entity.position.y}";
    // => _view.gameObject.name = $"{_data.block}_{entity.position.x}_{entity.position.y}";

    public async void ReadyToDelete(ChipsEntity entity, int lives, int delay)
    {
        if (lives == 0)
        {
            await Task.Delay(delay);
            entity.isDead = true;
            entity.DoRemovePos(_contexts, false);
        }
    }
            
    protected void DoFall(int y)
    {
        if(_view.Transform.position.y == y)
            return;
        
        _view.Transform.DOKill();
        _view.Transform.DOMoveY(y, (_view.Transform.position.y - y) * 0.15f).SetEase(Ease.Linear);
    }
}