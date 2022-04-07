using System.Collections.Generic;
using Core;
using Data;
using Entitas;
using UnityEngine;

public class CreateLevelSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly Transform _cameraTransform;
    private readonly FactoryService _factory;

    public CreateLevelSystem(Contexts contexts, CtxComponent ctx) : base(contexts.game)
    {
        _contexts = contexts;
        _factory = ctx.services.factory;
        _cameraTransform = ctx.links.Camera.transform;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        => context.CreateCollector(GameMatcher.CreateLevel);

    protected override bool Filter(GameEntity entity)
        => true;
    
    protected override void Execute(List<GameEntity> entities)
    {
        var levelConfig = _contexts.game.ctx.levelConfig;
        CreateBg(levelConfig);
        SetCameraPos(levelConfig);
        CreateChips(levelConfig);
    }

    private void CreateBg(LevelConfig levelConfig)
    {
        var gridView = _contexts.game.ctx.links.GridGeneratorLinks;
        var api = new GridGeneratorApi(gridView);
        api.LoadLevel(levelConfig);
    }

    private void CreateChips(LevelConfig level)
    {
            foreach (var levelItemData in level.Items)
            {
                Debug.Log($"Create at {levelItemData.x}.{levelItemData.y} {levelItemData.config.name}");
                var itemData = levelItemData.Clone();
                CreateItem(itemData);
            }
    }

    private void CreateItem(MergeItemProfileData itemData)
    {
        _factory.Create(itemData);
    }
    
    private void SetCameraPos(LevelConfig levelConfig)
    {
        var halfCell = 0.5f;
        _cameraTransform.position = new Vector3(
            levelConfig.Width / 2f - halfCell,
            levelConfig.Height / 2f - halfCell,
             _cameraTransform.transform.position.z);
    }
}