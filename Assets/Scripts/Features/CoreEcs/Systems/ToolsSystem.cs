using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configs;
using Core;
using Data;
using DG.Tweening;
using Entitas;
using Sirenix.Utilities;
using UnityEngine;
using Utils;

public class ToolsSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly ViewFactoryService _viewFactory;
    private readonly FactoryService _factory;
    private readonly BoardService _board;
    private MergeVisualConfig _visualConfig;

    public ToolsSystem(Contexts contexts, CtxComponent ctx) : base(contexts.game)
    {
        _contexts = contexts;
        _viewFactory = ctx.services.viewFactory;
        _factory = ctx.services.factory;
        _board = ctx.services.board;
        _visualConfig = ctx.visualConfig;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        => context.CreateCollector(GameMatcher.ApplyBooster);

    protected override bool Filter(GameEntity entity)
        => true;
    
    protected override async void Execute(List<GameEntity> entities)
    {
        var infos = entities.ConvertAll(e => e.applyBooster.value);
        foreach (var info in infos)
        {
            // var info = applyBoosterE.applyBooster.value;
            switch (info.attacker)
            {
                case "one" :
                    // await FinalClear(_ui.Merge.View.CoinsImage, new Vector3(info.fromX, info.fromY));
                        
                    Debug.Log($"Process booster1 {info.fromX} {info.fromY}");
                    await _viewFactory
                        .CreateTool("one", info.fromX, info.fromY)
                        .WaitEvent1();
                    UnlockView(info.fromX, info.fromY, 0 * 120f);
                    break;
                
                case "lineX" :
                    // await ToCoinsAll(info.fromX, info.fromY);
                    
                    await _viewFactory
                        .CreateTool("lineX", info.fromX, info.fromY)
                        .WaitEvent1();
                    
                    for (int xx = info.fromX; xx < 15; xx++)
                        UnlockView(xx, info.fromY, Math.Abs(xx - info.fromX)  * 120f);
                    
                    for (int xx = info.fromX-1; xx >=0 ; xx--)
                        UnlockView(xx, info.fromY, Math.Abs(xx - info.fromX) * 120f);
                    break;
                case "lineY" :
                    await _viewFactory
                        .CreateTool("lineY", info.fromX, info.fromY)
                        .WaitEvent1();
                    
                    for (int yy = info.fromY; yy < 15; yy++)
                        UnlockView(info.fromX, yy, Math.Abs(yy - info.fromY) * 120f);
                    
                    for (int yy = info.fromY-1; yy >= 0; yy--)
                        UnlockView(info.fromX, yy, Math.Abs(yy - info.fromY) * 120f);
                    break;
                case "cross" :
                    Debug.Log($"Process booster2 {info.fromX} {info.fromY}");
                    await _viewFactory
                        .CreateTool("cross", info.fromX, info.fromY)
                        .WaitEvent1();

                    for (int xx = info.fromX; xx < 15; xx++)
                        UnlockView(xx, info.fromY, Math.Abs(xx - info.fromX)  * 120f);
                    
                    for (int xx = info.fromX-1; xx >=0 ; xx--)
                        UnlockView(xx, info.fromY, Math.Abs(xx - info.fromX) * 120f);
                    
                    for (int yy = info.fromY; yy < 15; yy++)
                        UnlockView(info.fromX, yy, Math.Abs(yy - info.fromY) * 120f);
                    
                    for (int yy = info.fromY-1; yy >= 0; yy--)
                        UnlockView(info.fromX, yy, Math.Abs(yy - info.fromY) * 120f);
                    
                    break;
                
                case "nItems" :
                    Debug.Log($"Process NItems {info.fromX} {info.fromY}");
                    
                    await _viewFactory
                        .CreateTool("one", info.fromX, info.fromY)
                        .WaitEvent1();
                    const int N_COUNT = 7;
                    var locked = _contexts.game.ctx.dynamicData.FindAll(v => v.isLocked);
                    locked.Shuffle();
                    var max = Math.Min(locked.Count, N_COUNT);
                    for (int i = 0; i < max; i++)
                    {
                        var view = locked[i];
                        var dist = Vector2.Distance(new Vector2(info.fromX, info.fromY),
                            new Vector2(view.position.x, view.position.y));
                        TrailTo( info.fromX, info.fromY, view.position.x, view.position.y, i * 100 , (int) (dist * 120f * 1.3f ) , "n_items_trail");
                        UnlockView(view.position.x, view.position.y, dist * 1.3f * 120f + i * 100);
                    }
                    break;
                
                case "clone" :
                    Debug.Log($"Process clone {info.fromX} {info.fromY}");
                    await _viewFactory
                        .CreateTool("clone", info.fromX, info.fromY)
                        .WaitEvent1();
                    CloneItem(info.fromX, info.fromY);
                    break;
                
                case "automergeX" :
                    Debug.Log($"Process automergeX {info.fromX} {info.fromY}");
                    await _viewFactory
                        .CreateTool("automergeX", info.fromX, info.fromY)
                        .WaitEvent1();
                    AutomergeX(info.fromX, info.fromY);
                break;
                case "upgradeOne" :
                    Debug.Log($"Process upgradeOne {info.fromX} {info.fromY}");
                    await _viewFactory
                        .CreateTool("upgradeOne", info.fromX, info.fromY)
                        .WaitEvent1();
                    UpgradeOne(info.fromX, info.fromY);
                break;
            }
        }
    }

    public async Task FinalClear(RectTransform to, Action cbFly)
    {
        var star = FindStarPos();
        await UnlockAll(star.x, star.y);
        var newCoins = new List<MergeItemView>();
        await ToCoinsAll(star.x, star.y, newCoins);
        // await CoinsFly ( (int)flyPos.x, (int)flyPos.y);
        await CoinsFly( newCoins, to, cbFly );
    }

    public ChipsEntity FindStar()
        => _contexts.game.ctx.dynamicData.FindAll(v => v.config.value.resourceName == "star").FirstOrDefault();

    // public ChipsEntity StarView()
        // => FindStar().;

    
    public (int x, int y) FindStarPos()
    {
        var star2 = FindStar();
        if (star2 != null)
            return (0,0);
        return (star2.position.x, star2.position.y);
    }
    
    public async Task UnlockAll(int fromX, int fromY)
    {
        var input = _contexts.game.ctx.dynamicData.Input;
        
        var star = FindStar();
        var locked = _contexts.game.ctx.dynamicData.FindAll(v => v.isLocked);
        locked = locked.OrderBy(v => -v.position.y + v.position.x*100 ).ToList();
        // nonResources.Shuffle();

        if (locked.Count > 0)
        {
            _viewFactory.CreateParticles("generate", star.position.x, star.position.y);
            await star.view.value.AnimateGeneration();
            // await Task.Delay(200);
        }

        var max = locked.Count;
        for (int i = 0; i < max; i++)
        {
            var view = locked[i];
            var dist = Vector2.Distance(new Vector2(fromX, fromY),
                new Vector2(view.position.x, view.position.y));
            // UnlockView(view.position.x, view.position.y, i * 200);
            UnlockView(view.position.x, view.position.y, i * 0);
        }
        // await Task.Delay((max - 1) * 0);
        if(locked.Count > 0 && !input.SkipPressed)
            await Task.Delay(600);
    }

    public async Task CoinsFly(List<MergeItemView> newCoins, RectTransform to, Action cb)
    {
        var coins = _contexts.game.ctx.dynamicData.FindAll(v => v.config.value.resourceName == "coin" );

        var tasks = new List<Task>();
        var _camera = _contexts.game.ctx.links.Camera;
        foreach (var entity in coins)
        {
            var view = entity.view.value;
            var task = AnimateCoinFly(to, cb, entity, view, _camera);
            tasks.Add(task);
            
            await Task.Delay(40);
        }

        await Task.WhenAll(tasks);
    }

    private async Task AnimateCoinFly(RectTransform to, Action cb, ChipsEntity entity, MergeItemView view, Camera _camera)
    {
        _viewFactory.CreateParticles("generate", entity.position.x, entity.position.y);
        await view.AnimateGeneration();
        view.SpriteRenderer.color = new Color(1, 1, 1, 0);

        await view.SpriteRenderer.DoFxFly(0.7f, _camera, to, true, cb);
    }

    private async void DelayedDestroy(ChipsEntity entity)
    {
        await Task.Delay(400);
        entity.toDelete = true;
    }

    public async Task ToCoinsAll(int fromX, int fromY, List<MergeItemView> newCoins)
    {
        var input = _contexts.game.ctx.dynamicData.Input;
        // input.SkipPressed = false;
        
        var star = FindStar();
        (fromX, fromY) = (star.position.x, star.position.y);
        
        var nonResources = _contexts.game.ctx.dynamicData.FindAll(v => v.config.value.resourceName.IsNullOrWhitespace() );
        nonResources = nonResources.OrderBy(v => -v.position.y + v.position.x * 100 ).ToList();
        nonResources.Shuffle();

        if (nonResources.Count > 0)
        {
            _viewFactory.CreateParticles("generate", star.position.x, star.position.y);
            await star.view.value.AnimateGeneration();
        }

        var tasks = new List<Task>();
        foreach (var e in nonResources)
        {
            var (posX, posY) = (e.position.x, e.position.y);
            
            // TrailTo( fromX, fromY, posX, posY, 0, (int) (dist * 120f * 1.3f ) );
            var flyTime = (int) (500* 1.3f );
            TrailTo( fromX, fromY, posX, posY, 0, flyTime , "coins_trail");
            var task = ReplaceWithCoin(e, posX, posY, flyTime, newCoins);
            tasks.Add(task);

            var delay = 40; 
            await Task.Delay(delay);
        }   

        // if(nonResources.Count > 0)
            // await Task.Delay(500);
        await Task.WhenAll(tasks);
    }

    private async Task ReplaceWithCoin(ChipsEntity e, int posX, int posY, int delay, List<MergeItemView> newChips)
    {
        await Task.Delay(delay);
        
        e.isDead = true;
        e.DoRemovePos(_contexts, false);
        e.toDelete = true;

        var coinsConfig = _contexts.game.ctx.mergeConfig.CoinsSequence.Items[0];

        _viewFactory.CreateParticles("appear", posX, posY);
        var itemData = new MergeItemProfileData(coinsConfig, (int) posX, (int) posY, 0);
        var item = Contexts.sharedInstance.game.ctx.services.factory.Create(itemData);
        newChips.Add(item.view);
    }

    private void AutomergeX(int x, int y)
    {
        var levelConfig = _contexts.game.ctx.levelConfig;

        var items = new Dictionary<MergeItemConfig, List<ChipsEntity>>();

        void AddItem(ChipsEntity e)
        {
            if (items.ContainsKey(e.config.value))
                items[e.config.value].Add(e);
            else
                items[e.config.value] = new List<ChipsEntity>() {e};
        }
        
        for (int i = 0; i < levelConfig.Width; i++)
        {
            var item = GetItem(i, y);
            if(item == null ||  item.isLocked)
                continue;
            AddItem(item);
        }

        foreach (var kv in items)
        {
            MergeSiblings(kv);
        }
    }
    private void UpgradeOne(int x, int y)
    {
        var items = new Dictionary<MergeItemConfig, List<ChipsEntity>>();
        void AddItem(ChipsEntity e)
        {
            if (items.ContainsKey(e.config.value))
                items[e.config.value].Add(e);
            else
                items[e.config.value] = new List<ChipsEntity>() {e};
        }
        
        var item = GetItem(x, y);
        if(item == null ||  item.isLocked)
            return;
        AddItem(item);

        foreach (var kv in items)
            UpgradeSiblings(kv);
    }

    private void UpgradeSiblings(KeyValuePair<MergeItemConfig, List<ChipsEntity>> kv)
    {
        var siblings = kv.Value;
        var first = siblings[0];

        _board.DOUpgrade(first, first.position.x, first.position.y, siblings);
    }

    private async void MergeSiblings(KeyValuePair<MergeItemConfig, List<ChipsEntity>> kv)
    {
        var siblings = kv.Value;
        var first = siblings[0];
        var (posX, posY) = (first.position.x, first.position.y);
            
        var availablePositions = _board.GetAvailablePositions(siblings, posX, posY, null);
        availablePositions.Insert(0, new Vector2(posX, posY));

        await _board.DOMerge(first.config.value, first.position.x, first.position.y, siblings, availablePositions);
    }
    
    private void CloneItem(int x, int y)
    {
        var origView = GetItem(x, y);
        if (origView == null || origView.data.value.locked)
            return;
        
        var empty = new List<(int, int)>();
        BordForEach((x, y, v) => v == null, (x, y, v) => empty.Add((x, y)));
        empty.Shuffle();
        if (empty.Count == 0)
            return;
        var (posX, posY) = empty[0];

        var cloneData = origView.data.value.Clone();
        cloneData.x = posX;
        cloneData.y = posY;
        var item = _factory.Create(cloneData);
        item.view.AnimateDropItem(_contexts.game.ctx.visualConfig, item.view, x,y, new Vector2(posX, posY), 0).DoAsync();
    }

    private void BordForEach(Func<int,int, ChipsEntity, bool> match, Action<int,int, ChipsEntity> cb)
    {
        var levelConfig = _contexts.game.ctx.levelConfig;
        
        for (int yy = 0; yy < levelConfig.Height; yy++)
        for (int xx = 0; xx < levelConfig.Width; xx++)
        {
            var view = GetItem(xx,yy);
            if (match(xx, yy, view))
                cb(xx, yy, view);
        }
    }
    // private List<ChipsEntity> Find(Predicate<ChipsEntity> match) 
        // => _contexts.game.ctx.dynamicData.Items.FindAll(match);

    private async void UnlockView(int x, int y, float delay)
    {
        var e = _contexts.ChipByPos(x, y);
        // var view = GetItemView(x, y);
        if (e == null || !e.data.value.locked)
            return;
        
        e.data.value.unlockCount = e.data.value.lockCount;
        e.isLocked = e.data.value.locked;
        
        await Task.Delay((int)(delay));
        e.view.value.ApplyFromData();
        _viewFactory.CreateUnlockParticles(x, y);
    }
    
    private async void BubbleTo(int fromX, int fromY, int toX, int toY, int delay, int duration)
    {
        await Task.Delay(delay);
        
        var fx = _viewFactory.CreateParticles("bubble", fromX, fromY);
        fx.transform.DOMoveX(toX,duration/1000f ).SetEase(Ease.InSine);
        fx.transform.DOMoveY(toY, duration/1000f).SetEase(Ease.Linear);
        
        await Task.Delay(duration + 300);
        _viewFactory.Destroy(fx);
    }

    private async void TrailTo(int fromX, int fromY, int toX, int toY, int delay, int duration, string particleName )
    {
        await Task.Delay(delay);
        
        var fx = _viewFactory.CreateParticles(particleName, fromX, fromY);
        fx.transform.DOMoveX(toX,duration/1000f ).SetEase(Ease.OutQuad);
        fx.transform.DOMoveY(toY, duration/1000f).SetEase(Ease.Linear);
        
        await Task.Delay(duration + 300);
        _viewFactory.Destroy(fx);
    }

    // private ChipsEntity GetItemView(int xx, int yy) 
        // => _data.GetItem(xx, yy);
    
    private ChipsEntity GetItem(int xx, int yy) 
        => _contexts.ChipByPos(xx, yy);

    private bool IsEmpty(int xx, int yy) 
        => _contexts.ChipByPos(xx, yy) == null;

}