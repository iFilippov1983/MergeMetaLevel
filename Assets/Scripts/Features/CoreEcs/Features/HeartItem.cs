using System.Threading.Tasks;
using Data;
using DG.Tweening;

namespace Core
{
    public class HeartItem : BaseItem, IDoMoveListener, IClickListener, IHighlightListener, IHighlightRemovedListener
    {
        private MergeVisualConfig _visualConfig;

        public HeartItem(ViewFactoryService viewFactory, MergeItemConfig config, MergeItemProfileData levelData,
            Contexts contexts)
            : base(viewFactory, config, levelData, contexts)
        {
            var entity = base.CreateEntityAddComponents();
            base.CreateView(entity);
            base.UpdateName(entity);
            base.AddListeners(entity, this);
            entity.isHeart = true;
            _visualConfig = contexts.game.ctx.visualConfig;
        }

        public void OnDoMove(ChipsEntity entity, int x, int y, float speed, Ease easing)
            => base.DoMove(entity, x, y, speed, easing);
        
        public void OnHighlight(ChipsEntity entity, int x, int y)
            => base.DoHighlight(entity, x, y);

        public void OnHighlightRemoved(ChipsEntity entity)
            => base.DoHighlightRemoved(entity);
        
        public async void OnClick(ChipsEntity entity)
        {
            _view.Transform.DOScale(0.4f, 0.2f).SetEase(Ease.Linear);    
            await Task.Delay(100);
            _viewFactory.CreateFx("bubble_blast", entity.position.x, entity.position.y);
            await Task.Delay(100);
            var lifeFly = _contexts.board.CreateEntity();
            lifeFly.AddLifeFly( _config.count, entity.position.x, entity.position.y);
            
            ReadyToDelete(entity, 0, 0);
            entity.toDelete = true;
        }

    }
}