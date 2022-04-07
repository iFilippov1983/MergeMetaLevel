using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Merge;
using Data;
using Entitas;
using Features._Events;
using UnityEngine;
using Utils;

namespace Core
{
    public class DragLogicSystem : IExecuteSystem, IClear
    {
        private readonly Contexts _contexts;
        private readonly Camera _camera;
        private readonly MergeFieldApi _fieldApi;
        private readonly MergeLogicApi _logicApi;
        private readonly MergeDynamicData _data;
        private readonly MergeEvents _events;
        private ViewFactoryService _viewFactory;

        public DragLogicSystem(Contexts contexts, CtxComponent ctx)
        {
            _contexts = contexts;
            _camera = ctx.links.Camera;
            _logicApi = ctx.services.logicApi;
            _data = ctx.dynamicData;
            _events = ctx.events;
            _viewFactory = ctx.services.viewFactory;

            _fieldApi = new MergeFieldApi(contexts, contexts.game.ctx.dynamicData, contexts.game.ctx.events, contexts.game.ctx.visualConfig);

            ctx.events.UseTool = UseTool;
            _fieldApi.OnDragToAnotherCell += OnItemDrag;
            _fieldApi.OnClick += OnItemClick;
            _fieldApi.OnDragRelease += DoEndDrag;
        }

        public void Clear()
        {
            _fieldApi.OnClick -= OnItemClick;
            _fieldApi.OnDragToAnotherCell -= OnItemDrag;
            _fieldApi.OnDragRelease -= DoEndDrag;
        }

        public void Execute()
        {
            _fieldApi.OnUpdate();
            if (Input.GetMouseButtonDown(1))
            {
                var boosterName = GetBoosterName();
                if(boosterName != null)
                    UseTool(boosterName);
            }
        }

        private void UseTool(string boosterName)
        {
            var (posX, posY) = _camera.ScreenToWorldPoint(Input.mousePosition).Round();
            _contexts.game.CreateEntity().AddApplyBooster(DamageInfo.FromBooster(posX, posY, boosterName));
        }

        private static string GetBoosterName()
        {
            string boosterName = null;
            if (Input.GetKey(KeyCode.Alpha1))
                boosterName = "one";
            if (Input.GetKey(KeyCode.Alpha2))
                boosterName = "lineX";
            if (Input.GetKey(KeyCode.Alpha3))
                boosterName = "lineY";
            if (Input.GetKey(KeyCode.Alpha4))
                boosterName = "cross";
            if (Input.GetKey(KeyCode.Alpha5))
                boosterName = "nItems";
            if (Input.GetKey(KeyCode.Alpha6))
                boosterName = "clone";
            if (Input.GetKey(KeyCode.Alpha7))
                boosterName = "automergeX";
           if (Input.GetKey(KeyCode.Alpha8))
                boosterName = "upgradeOne";
            return boosterName;
        }

        private void OnItemClick(ChipsEntity entity, int posX, int posY)
        {
            if(entity.isDead || entity.isLocked)
                return;
            
            if(!(entity.feature.value is IClickListener))
                return;
            
            var clickBlocked = _events.IsClickAllowed?.Invoke(posX, posY) == false;
            if(clickBlocked)
                return;
            
            entity.hasClick = true;

            _events.OnClick?.Invoke(posX, posY);
            _events.OnMoveSpend?.Invoke();
            _events.OnStepEnd?.Invoke();
        }

        private void OnItemDrag(ChipsEntity draggable, int posX, int posY)
        {
            // draggable.view.value.SetSortOrder(CoreConstants.sortOrderTop);
            
            _logicApi.HighlightSiblings(draggable, posX, posY);
            draggable.view.value.SetSortOrder(CoreConstants.sortOrderTop);
        }

        private async void DoEndDrag(ChipsEntity dragged, int posX, int posY)
        {
            (int x, int y) startPos = _fieldApi.MouseStartPos();
            
            // draggable.view.value.SetSortOrder(0);
            dragged.view.value.gameObject.layer =  0;
            dragged.view.value.SpriteRenderer.sortingOrder = -1;
            
            var _ = _logicApi.StopHighlight();
            var data =_contexts.game.ctx.dynamicData;

            var siblings = _logicApi.GetSiblings(dragged, posX, posY);
            
            var readyForMerge = _logicApi.ReadyForMerge(posX, posY, siblings.Count+1, dragged.config.value); 
            var itemAt = _fieldApi.GetItemAt(posX, posY);
            var isLocked = _fieldApi.IsLocked(itemAt);
            var hasHole = _fieldApi.HasHoleAt(posX, posY);
            var isOutOfBounds = _fieldApi.IsOutOfBounds(posX, posY);
            var hasMergeable = _fieldApi.HasItemAt(posX, posY);
            var isSameType = _fieldApi.SameConfig(itemAt, dragged.config.value);
            var dragAllowed = _events.EndDragAllowed?.Invoke(posX, posY) != false;
            var samePos = posX == startPos.x && posY == startPos.y;
            var mergeAllowed = readyForMerge && (!hasMergeable || isSameType);
            
            var canMerge = mergeAllowed && !hasHole && !isLocked && !isOutOfBounds && dragAllowed;
            var canPlace = !hasHole && !hasMergeable && !isOutOfBounds && dragAllowed;
            var canSwap = !samePos && !readyForMerge && !hasHole && hasMergeable && !isLocked && !isOutOfBounds && dragAllowed;
            if (hasMergeable && !mergeAllowed && !isLocked && !samePos && dragAllowed)
                canSwap = true; // swap then automerge
            
            if(canMerge || canPlace || canSwap)
                _events.OnMoveSpend?.Invoke();
            
            if(canMerge || canPlace || canSwap)
                _events.OnEndDrag?.Invoke(posX, posY);
            
            siblings.Add(dragged);
            Task t;

            var autoCheckPositions = new List<(int, int)>();
            if(canSwap || canMerge || canPlace)
                autoCheckPositions.Add((posX, posY));
            if(canSwap)
                autoCheckPositions.Add((startPos.x, startPos.y));
            
            t = (canSwap, canMerge, canPlace) switch
            {
                (true, _, _) => _fieldApi.DoSwap(dragged, posX, posY),
                (false, true, _) => _logicApi.ProcessMerge(dragged, posX, posY, siblings),
                (false, false, true) => _fieldApi.DoPlace(dragged, posX, posY),
                (false, false, false) => _fieldApi.DoReturnDraggable(dragged)
            };

            await t;
            
            await Automerge(autoCheckPositions);
            
            _events.OnStepEnd?.Invoke();
        }

        private async Task Automerge(List<(int x, int y)> autoCheckPositions)
        {
            if(autoCheckPositions.Count == 0)
                return;

            var list = new List<string>() {"Beautiful!", "Great!", "Amazing!", "Amazing!"};
            var readyForMerge = true;
            while (readyForMerge)
            {
                foreach (var checkPosition in autoCheckPositions)
                {
                    if (CheckForAutomerge(checkPosition.x, checkPosition.y, out var dragged, out var siblings))
                    {
                        await _logicApi.ProcessMerge(dragged, checkPosition.x, checkPosition.y, siblings);
                        _viewFactory.FxTextGreat(checkPosition.x, checkPosition.y, list.Random());
                    }
                }

                readyForMerge = false;
                foreach (var checkPosition in autoCheckPositions)
                {
                    if (readyForMerge == false)
                        readyForMerge = CheckForAutomerge(checkPosition.x, checkPosition.y, out var _, out var _);
                }
            }
          
        }

        private bool CheckForAutomerge(int posX, int posY, out ChipsEntity dragged, out List<ChipsEntity> siblings)
        {
            dragged = _fieldApi.GetItemAt(posX, posY);
            siblings = null;
            
            if (dragged == null)
                return false;

            siblings = _logicApi.GetSiblingsIncludeSelf(dragged, posX, posY);
            
            return _logicApi.ReadyForMerge(posX, posY, siblings.Count, dragged.config.value);
        }
    }
}