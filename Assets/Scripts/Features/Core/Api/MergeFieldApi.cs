using System;
using System.Threading.Tasks;
using Api.Merge;
using Data;
using DG.Tweening;
using Features._Events;
using UnityEngine;
using Utils;

namespace Core
{
    [Serializable]
    public class MergeFieldApi
    {
        private MergeDynamicData _data;
        
        private ChipsEntity _draggable;
        private Vector2 _lastPos;
        private bool _wasDrag;
        private float _startDragTime;
        private MergeVisualConfig _visualConfig;
        private MergeMouseDragApi _mouseApi;
        private Contexts _contexts;
        
        public Action<ChipsEntity, int, int> OnDragToAnotherCell;
        public Action<ChipsEntity, int, int> OnDragRelease;
        public Action<ChipsEntity, int, int> OnClick;
        private MergeEvents _events;

        public bool hasDraggable => _draggable != null;
        public ChipsEntity Draggable => _draggable;

        public MergeFieldApi(Contexts contexts, MergeDynamicData data, MergeEvents events, MergeVisualConfig visualConfig)
        {
            _events = events;
            _data = data;
            _visualConfig = visualConfig;
            _contexts = contexts;
            
            _draggable = null;
            _lastPos = Vector2.zero;
            _wasDrag = false;
            
            _mouseApi = new MergeMouseDragApi(contexts, contexts.game.ctx.links.Camera, contexts.game.ctx.links.CameraTransform);
            _mouseApi.OnMouseDown = DoMouseDown;
            _mouseApi.OnDrag = DoDrag;
            _mouseApi.OnMouseUp = DoMouseUp;
        }
        
        public void Clear()
        {
            _draggable = null;
            _wasDrag = false;
            _lastPos = Vector2.zero;
        }

        public void OnUpdate()
        {
            _mouseApi.OnUpdate();
        }
        
        private void DoMouseDown(MergeMouseDragApi mouse)
        {
            var (posX, posY) = mouse.WorldPos.Round();
            
            // if(_data.IsClickAllowed?.Invoke(posX, posY) == false)
                // return;
            
            if(_events.StartDragAllowed?.Invoke(posX, posY) == false
                && _events.IsClickAllowed?.Invoke(posX, posY) == false)
                return;

            // Debug.Log("MouseDown");
            DoStartDrag(mouse);
        }
        
        private void DoDrag(MergeMouseDragApi mouse)
        {
            var (posX, posY) = MousePos(mouse);
    
            
            if (TryMoveDraggable(posX, posY))
                OnDragToAnotherCell?.Invoke(Draggable, posX, posY);

            if (Vector2.SqrMagnitude(mouse.WorldPos - mouse.WorldStartPos) > 0.1 * 0.1 && _draggable?.isDead == false)
            {
                _draggable.view.value.Transform.DOKill();
                _draggable.view.value.Transform.DOMove(endValue: mouse.WorldPos, 0.07f).SetEase(_visualConfig.DragFlyEasing);
            }
        }
        
        private void DoMouseUp(MergeMouseDragApi mouse)
        {
            _draggable?.view.value.SetCanMergeFx(false);
            
            if (WasNothing(mouse))
            {
                _draggable?.ReplaceDoMove((int)_lastPos.x, (int)_lastPos.y, _visualConfig.DragFlySpeed, _visualConfig.DragFlyEasing);
                return;
            }
            
            var draggable = _draggable;
            var (posX, posY) = MousePos(mouse);
            var wasClick = WasClick(mouse);
            var wasDrag = WasDrag(mouse);
            // var isInBounds = IsInBounds(posX, posY);
            Clear();

            if (wasClick)
                OnClick?.Invoke(draggable, posX, posY);

            // if (wasDrag && (!isInBounds))
                // await DoReturnDraggable(draggable);
            
            if (wasDrag)
            {
                OnDragRelease?.Invoke(draggable, posX, posY);
            }
        }

        private bool DoStartDrag(MergeMouseDragApi mouse)
        {
            if (_draggable != null)
            {
                Debug.Log("DoStartDrag BUT _dragged != null");
                // DoCancelDragImmediately();
                // _draggable = null;
                return false;
            }
            
            var (posX, posY) = mouse.WorldPos.Round();
            
            var dragged = GetItemAt(posX, posY);

            if(dragged == null || dragged.isLocked)
                return false;
            
            _startDragTime = Time.time; 
            _wasDrag = false;
            _draggable = dragged;
            _lastPos = new Vector2(_draggable.position.x, _draggable.position.y);
            
            dragged.view.value.SetCanMergeFx(true);
                
            // BringToTop(_draggable, true);
            return true;
        }

        public bool MousePositionChanged(int posX, int posY ) => _lastPos.x != posX || _lastPos.y != posY;

        public bool TryMoveDraggable(int posX, int posY)
        {
            if (!IsInBounds(posX, posY)
                || !hasDraggable 
                || !MousePositionChanged(posX, posY))
                return false;

            DoDragToMouseCell(posX, posY);
            return true;
        }
        
        public void DoDragToMouseCell(int posX, int posY)
        {
            _wasDrag = true;
            _lastPos.x  = posX;
            _lastPos.y  = posY;
            
            // _draggable.ReplaceDoMove((int)_lastPos.x, (int)_lastPos.y, _visualConfig.DragFlySpeed, _visualConfig.DragFlyEasing);
            // _draggable.view.value.Transform.DOKill();
            // _draggable.view.value.Transform.DOMove(endValue: _lastPos, _visualConfig.DragDuration).SetEase(_visualConfig.DragEasing);
        }

        public bool IsMouseCellChanged(int posX, int posY)
        {
            if (_lastPos.x == posX && _lastPos.y == posY)
                return false;
            
            return true;
        }

        public bool WasClick(MergeMouseDragApi mouse)
        {
            return _draggable != null && !_wasDrag && !ClickTimeElapsed();
        }

        public bool WasDrag(MergeMouseDragApi mouse)
        {
            return _draggable != null && _wasDrag;
        }

        public bool WasNothing(MergeMouseDragApi mouse)
        {
            return _draggable == null || (!_wasDrag && ClickTimeElapsed());
        }

        public async Task DoReturnDraggable(ChipsEntity dragged)
        {
            var (posX, posY) = MouseStartPos();
            
            dragged.DoReplacePos(_contexts, posX, posY);
            dragged.ReplaceDoMove(posX, posY, _visualConfig.CancelDragFlySpeed, _visualConfig.CancelDragFlyEase);
            dragged.view.value.SetSortOrder(CoreConstants.sortOrderTop);

            await Task.Delay(400);
            dragged.view.value.SetSortOrder(CoreConstants.sortOrderDefaul);
        }

        public (int, int) MouseStartPos() 
            => _mouseApi.WorldStartPos.Round();

        public async Task DoSwap(ChipsEntity dragged, int targetPosX, int targetPosY)
        {
            var source = dragged;
            var target = GetItemAt(targetPosX, targetPosY);
            var (sourceX, sourceY) = (dragged.position.x, dragged.position.y);
            source.DoRemovePos(_contexts);
            target.DoRemovePos(_contexts);
            dragged.view.value.SetSortOrder(CoreConstants.sortOrderTop);
            
            source.DoAddPos(_contexts, targetPosX, targetPosY);
            target.DoAddPos(_contexts, sourceX, sourceY);
            
            source.ReplaceDoMove(targetPosX, targetPosY, 0.3f, Ease.Linear);
            target.ReplaceDoMove(sourceX, sourceY, _visualConfig.CancelDragFlySpeed, _visualConfig.CancelDragFlyEase);
            await Task.Delay(200);
            dragged.view.value.SetSortOrder(CoreConstants.sortOrderDefaul);
        }
        
        public async Task DoPlace(ChipsEntity dragged, int posX, int posY)
        {
            dragged.view.value.SetSortOrder(CoreConstants.sortOrderDefaul);
            dragged.DoReplacePos(_contexts, posX, posY);
            dragged.ReplaceDoMove(posX, posY, 0.2f, Ease.Linear);
        }

        public (int,int) MousePos(MergeMouseDragApi mouse) => 
            mouse.WorldPos.Round();

        private bool ClickTimeElapsed()
        {
            var maxClickTime = 0.6f;
            return Time.time - _startDragTime >= maxClickTime;
        }

        // private static void BringToTop(ChipsEntity entity, bool onTop) 
            // => entity.ReplaceSortOrder(onTop ? 10 : 0);

        private void DoCancelDragImmediately()
        {
            _draggable.ReplaceDoMove((int)_lastPos.x, (int)_lastPos.y, 0.01f, Ease.Linear);
            // _draggable.transform.position = _lastPos;
            // BringToTop(_draggable, false);
        }

        public bool SameConfig(ChipsEntity entity, MergeItemConfig config)
            => entity?.config.value == config;
        
        public bool HasItemAt(int posX, int posY)
            => _data.GetItem(posX, posY) != null;

        public bool HasHoleAt(int posX, int posY)
            => _data.Level.Holes.Exists(v => v.x == posX && v.y == posY);

        public ChipsEntity GetItemAt(int posX, int posY) 
            => _data.GetItem(posX, posY);

        public bool IsLocked(ChipsEntity item)
            => item != null
                ? item.data.value.locked
                : false;

        public bool IsOutOfBounds(int posX, int posY) 
            => !IsInBounds(posX, posY);
        
        public bool IsInBounds(int posX, int posY) =>
            posX >= 0 && posX < _data.Level.Width
            && posY >= 0 && posY < _data.Level.Height;

    }
}