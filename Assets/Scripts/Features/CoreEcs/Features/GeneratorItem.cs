using System.Collections.Generic;
using Data;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class GeneratorItem : BaseItem, IDoMoveListener, IClickListener, IHighlightListener, IHighlightRemovedListener
    {
        private readonly MergeVisualConfig _visualConfig;
        private readonly LevelConfig _levelConfig;
        private readonly MergeDynamicData _dynamicData;
        private readonly FactoryService _chipFactory;
        private readonly GenerateComp _configGenerator;

        public GeneratorItem(ViewFactoryService viewFactory, MergeItemConfig config, MergeItemProfileData levelData,
            Contexts contexts)
            : base(viewFactory, config, levelData, contexts)
        {
            _levelConfig = _contexts.game.ctx.levelConfig;
            _visualConfig = _contexts.game.ctx.visualConfig;
            _dynamicData = _contexts.game.ctx.dynamicData;
            _chipFactory = _contexts.game.ctx.services.factory;
            
            var entity = base.CreateEntityAddComponents();
            base.CreateView(entity);
            base.UpdateName(entity);
            base.AddListeners(entity, this);
            
            entity.isGenerator = true;
            _configGenerator = _config.Generate.Clone();
        }
        
        public void OnDoMove(ChipsEntity entity, int x, int y, float speed, Ease easing)
            => base.DoMove(entity, x, y, speed, easing);

        public async void OnClick(ChipsEntity entity)
        {
            // await _view.WaitLifeGeneration();
            _viewFactory.CreateParticles("generate", entity.position.x, entity.position.y);
            await _view.AnimateGeneration();
            
            var positions = FindAvailablePosition(entity.position.x, entity.position.y);
            var index = Random.Range(0, positions.Count);
            if (positions.Count > 0)
            {
                var targetPos = positions[index];
                var data = new MergeItemProfileData(_configGenerator.Next(), (int)targetPos.x, (int) targetPos.y, 0);
                var item = _chipFactory.Create(data);
                item.view.AnimateGeneratedDrop(_visualConfig, item.view, entity.position.x, entity.position.y, targetPos);

                if (_configGenerator.IsEmpty())
                {
                    entity.isDead = true;
                    entity.DoRemovePos(_contexts, false);
                    
                    _viewFactory.CreateDisappearParticles(entity.position.x, entity.position.y);
                    await _view.AnimateDisappear();
                    entity.toDelete = true;
                }
            }
        }

        public void OnHighlight(ChipsEntity entity, int x, int y)
            => base.DoHighlight(entity, x, y);

        public void OnHighlightRemoved(ChipsEntity entity)
            => base.DoHighlightRemoved(entity);

        private List<Vector2> FindAvailablePosition(int posX, int posY)
        {
            var res = new List<Vector2>();
            var (width, height) = (_levelConfig.Width, _levelConfig.Height);
            for (int xx = 0; xx < width; xx++)
            for (int yy = 0; yy < height; yy++)
            {
                var e = _dynamicData.GetItem(xx, yy);
                if (e == null && !_contexts.IsHole(xx, yy))
                    res.Add(new Vector2(xx, yy));
            }

            return res;
        }
    }
}