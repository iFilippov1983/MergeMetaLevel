using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Core;
using Data;
using Entitas;
using UnityEngine;
using Utils;

namespace Api.Merge
{
    [Serializable]
    public class MergeLogicApi
    {
        private Contexts _contexts;
        private MergeDynamicData _data;
        private int _lastHighlightPosX,_lastHighlightPosY;
        private IGroup<ChipsEntity> _highlightGroup;
        private List<ChipsEntity> _entityBuffer = new List<ChipsEntity>();
        private BoardService _board;
        private ViewFactoryService _viewFactory;

        public void SetCtx(Contexts contexts, MergeDynamicData data, ViewFactoryService factory)
        {
            _contexts = contexts;
            _data = data;
            _viewFactory = factory;
            _board = _contexts.game.ctx.services.board;
            _highlightGroup = _contexts.chips.GetGroup(ChipsMatcher.Highlight);
        }

        public bool CanMerge(List<MergeItemView> siblings)
        {
            return siblings.Count > 2;
        }

        public bool CanMerge(MergeItemView itemOnTile, MergeItemView dragged)
        {
            var passByConfig = itemOnTile.Config == dragged.Config && dragged.Config.Next() != null;
            if (!passByConfig)
                return false;

            return true;
        }

        public async Task StopHighlight()
        {
            var highlighted = _highlightGroup.GetEntities(_entityBuffer);
            foreach (var e in highlighted)
            {
                Debug.Log("Remove HL");
                e.RemoveHighlight();
            }
            
            await Task.Delay(200);
        }

        public async void HighlightSiblings(ChipsEntity item, int posX, int posY)
        {
            _lastHighlightPosX = posX;
            _lastHighlightPosY = posY;
            await StopHighlight();
            
            if(_lastHighlightPosX != posX || _lastHighlightPosY != posY) // Someone already call again HighlightSiblings() while await StopHighlight()
                return;

            if(_contexts.IsHole(posX, posY))
                return;
            
            if(IsLocked(posX, posY))
                return;
                
            var siblings = _board.GetSiblings(item, posX, posY);
            if (siblings.Count < 2)
                return;
            
            foreach (var sibling in siblings)
                sibling.ReplaceHighlight(posX, posY);
        }

        public async Task ProcessMerge(ChipsEntity dragged, int posX, int posY, List<ChipsEntity> siblings)
        {
            List<Vector2> availablePositions = GetAvailablePositions(siblings, posX, posY, dragged);
            await _board.DOMerge(dragged.config.value, posX, posY, siblings, availablePositions);
        }

        public List<Vector2> GetAvailablePositions(List<ChipsEntity> siblings, int posX, int posY, ChipsEntity exclude)
        {
            var availablePositions = _board.GetAvailablePositions(siblings, posX, posY, exclude);
            return availablePositions;
        }

        public List<ChipsEntity> GetSiblings(ChipsEntity dragged, int posX, int posY) 
            => _board.GetSiblings(dragged, posX, posY);

        public List<ChipsEntity> GetSiblingsIncludeSelf(ChipsEntity dragged, int posX, int posY)
        {
            var siblings = _board.GetSiblings(dragged, posX, posY);
            siblings.Add(dragged);
            return siblings;
        }

        public bool ReadyForMerge(int posX, int posY, int itemsCount, MergeItemConfig config)
        {
            if (itemsCount < 3)
                return false;
            
            var next = config.Next();
            if (next == null)
            {
                _viewFactory.CreateFx2("max_level", posX, posY);
                return false;
            }
            
            return true;
        }

        private bool IsLocked(int posX, int posY)
        {
            if (_contexts.IsHole(posX, posY))
                return true;
            
            var underchip = _contexts.ChipByPos(posX, posY);
            return underchip != null 
                   && underchip.data.value.locked;
        }

        private List<Vector2> FindAvailablePosition(int posX, int posY)
        {
            var res = new List<Vector2>();
            for (int xx = 0; xx < _data.Level.Width; xx++)
            for (int yy = 0; yy < _data.Level.Height; yy++)
            {
                if (_data.GetItem(xx, yy) == null)
                    res.Add(new Vector2(xx, yy));
            }

            return res;
        }

        private void BringToTop(MergeItemLifeView entity, bool onTop)
        {
            entity.gameObject.layer = onTop ? 2: 0;
            entity.Child.GetComponent<SpriteRenderer>().sortingOrder = onTop ? 10 : 0;
        }
    }
}