using System.Collections.Generic;
using Data;
using Entitas;
using UnityEngine;

namespace Core
{
    public class LifeFlySystem : ReactiveSystem<BoardEntity>
    {
        private readonly Contexts _context;
        private readonly ViewFactoryService _viewFactory;
        private readonly MergeVisualConfig _visualConfig;
        private readonly IGroup<ChipsEntity> _lockedGroup;
        private readonly List<ChipsEntity> _groupBuffer = new List<ChipsEntity>();

        public LifeFlySystem(Contexts context) : base(context.board)
        {
            _context = context;
            _viewFactory = _context.game.ctx.services.viewFactory;
            _visualConfig = _context.game.ctx.visualConfig;

            _lockedGroup = _context.chips.GetGroup(ChipsMatcher.Locked);
        }

        protected override ICollector<BoardEntity> GetTrigger(IContext<BoardEntity> context) 
            => context.CreateCollector(BoardMatcher.LifeFly.Added());

        protected override bool Filter(BoardEntity entity)
            => true;
        
        protected override void Execute(List<BoardEntity> entities)
        {
            var lockedList = _lockedGroup.GetEntities(_groupBuffer);
            foreach (var entity in entities)
            {
                var lifesCount = entity.lifeFly.lifesContain;
                var (posX, posY) = (entity.lifeFly.x, entity.lifeFly.y);
                var rand = Random.Range(0, 1);
                for (int i = 0; i < lifesCount; i++)
                    CreateLifeAndFly(lockedList, posX, posY, i, rand);
                
                entity.Destroy();
            }
        }
        
        public async void CreateLifeAndFly(List<ChipsEntity> lockedList, int posX, int posY, int index, int rand)
        {
            var target = lockedList.Find(v => v.chipInfo.data.lifeTargetCount < v.chipInfo.data.RestLocks);
            if(target == null)
                return;
                
            // _createApi.CreatePlusOne(posX, posY);
            target.chipInfo.data.lifeTargetCount++;
            
            var life = _viewFactory.CreateLifeFlyParticles(posX, posY);
            life.transform.position = new Vector3(posX, posY, 0);
            // _dynamicData.FlyingLifes.Add(life);

            while (target != null)
            {
                var (tarPosX, tarPosY) = (target.position.x, target.position.y);
                // BringToTop(life, true);
                // target.Data.isLifeTarget = true;
                await life.FlyTo( tarPosX, tarPosY, _visualConfig, index, rand);
                // target.Data.isLifeTarget = false;
                if (target.chipInfo.data.locked)
                {
                    _viewFactory.Destroy(life);
                    target.chipInfo.data.lifeTargetCount--;
                    target.chipInfo.data.unlockCount++;
                    target.isLocked = target.chipInfo.data.locked;
                    AnimateUnlock(target.feature.value.view, tarPosX, tarPosY);

                    target = null;
                    life = null;
                }
                else
                {
                    var lockedList2 = _lockedGroup.GetEntities(_groupBuffer);
                    target = lockedList2.Find(v => v.chipInfo.data.lifeTargetCount < v.chipInfo.data.RestLocks);
                    // target = _data.Items.Find(v => v.Data.locked && v.Data.lifeTargetCount < v.Data.RestLocks);
                    // target = _data.Items.Find(v => v.Data.locked && !v.Data.isLifeTarget);
                }
            }
            
            if(life != null)                    
                _viewFactory.Destroy(life);
        }

        private async void AnimateUnlock(MergeItemView target, int tarPosX, int tarPosY)
        {
            _viewFactory.CreateLifeExplosionParticles(tarPosX, tarPosY);
            await target.AnimateUnlocks(AnimateFxIfUnlocked);

            void AnimateFxIfUnlocked()
            {
                if (!target.Data.locked)
                    _viewFactory.CreateUnlockParticles(tarPosX, tarPosY);
            }
        }
    }
}