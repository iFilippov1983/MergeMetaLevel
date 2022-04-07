using Data;
using DG.Tweening;

namespace Core
{
    public class MergeItem : BaseItem, IDoMoveListener, IHighlightListener, IHighlightRemovedListener
    {
        public MergeItem(ViewFactoryService viewFactory, MergeItemConfig config, MergeItemProfileData levelData,
            Contexts contexts)
            : base(viewFactory, config, levelData, contexts)
        {
            var entity = base.CreateEntityAddComponents();
            base.CreateView(entity);
            base.UpdateName(entity);
            base.AddListeners(entity, this);
        }

        public void OnDoMove(ChipsEntity entity, int x, int y, float speed, Ease easing)
            => base.DoMove(entity, x, y, speed, easing);
        public void OnHighlight(ChipsEntity entity, int x, int y)
            => base.DoHighlight(entity, x, y);

        public void OnHighlightRemoved(ChipsEntity entity)
            => base.DoHighlightRemoved(entity);
        
        // private async void AnimateDestroyThenDelete(ChipsEntity entity)
        // // {
        // //     _view.SetImageActive(false);
        // //     entity.doCallNearMatch = true;
        // //     
        // //     ReadyToDelete(entity, 0, 200);
        // //     await _view.Animate("block_explosion");
        //     entity.toDelete = true;
        // }
    }
}