using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Components.Main
{
    [Serializable]
    public class UiMainScreenPointData
    {
        public int LevelOffset;
        public Transform Point;
        public UiMainScreenPointData(int levelOffset, Transform point)
        {
            LevelOffset = levelOffset;
            Point = point;
        }
    }
    public class UiMainScreenLevelsMove : MonoBehaviour
    {
        public List<RectTransform> Points;
        public UiMainScreenLevel Prefab;
        
        [ReadOnly,ShowInInspector] private List<UiMainScreenPointData> _pointsData = new List<UiMainScreenPointData>();
        private List<UiMainScreenLevel> _items = new List<UiMainScreenLevel>();
        
        private float _offset;
        private int _curLevel;
        private static readonly int CURRENT_LEVEL_POSITION = 2;
        private bool ready;

        [Button]
        public void Init(int level)
        {
            if(ready)
                return;
            ready = true;
            
            _curLevel = level;
            _offset = Points[1].anchoredPosition.y - Points[0].anchoredPosition.y;

            FillPointsData();
            FillItems();
        }

        [Button]
        public async Task Move()
        {
            await AnimateLevelDone();

            foreach (var item in _items)
            {
                var curPoint = item.PointData;
                var nextPoint = GetNextPoint(curPoint);

                if(nextPoint == null)
                    continue;
                
                item.PointData = nextPoint;
            }

            _curLevel++;
            var newItem = CreateItem(_pointsData[0], _curLevel);
            newItem.transform.localPosition = new Vector2(0, -_offset);
            
            foreach (var item in _items)
            {
                if(item.PointData == null)
                    continue;

                ChangeParent(item);
                // ChangeColor(item);
                MoveItem(item, () => CheckToDelete(item));
            }
            
            await Task.Delay(500);
            foreach (var item in _items)
               ChangeColor(item);
            
            await Task.Delay(400);
            FindCurLevel().Container.DOShakeScale(0.4f, 0.2f, 4);
            
        }

        private UiMainScreenPointData GetNextPoint(UiMainScreenPointData curPoint)
        {
            var i = _pointsData.IndexOf(curPoint);
            var nextPoint = _pointsData.SaveGet(i + 1);
            return nextPoint;
        }

        private UiMainScreenLevel FindCurLevel()
            => _items.Find(item => item.PointData?.LevelOffset == 0);
        
        private async Task AnimateLevelDone()
        {
            await FindCurLevel().ToChecked();
            
            // foreach (var item in _items)
            // {
            //     if (item.PointData == null)
            //         continue;
            //
            //     if (item.PointData.LevelOffset == 0)
            //     {
            //         await item.ToChecked();
            //         break;
            //     }
            // }
        }

        private void FillPointsData()
        {
            _pointsData.Clear();
            var levelOffset = CURRENT_LEVEL_POSITION;
            foreach (var point in Points)
            {
                _pointsData.Add(new UiMainScreenPointData(levelOffset, point));
                levelOffset--;
            }
        }

        private void FillItems()
        {
            foreach (var item in _items)
                Destroy(item);

            _items.Clear();
            for (int i = 0; i < _pointsData.Count; i++)
            {
                var pointData = _pointsData[i];
                var item = CreateItem(pointData, _curLevel);
                SetColor(item);
                
                HideIfLessZero(pointData, item);
            }
        }

        private void HideIfLessZero(UiMainScreenPointData pointData, UiMainScreenLevel item)
        {
            if (_curLevel + pointData.LevelOffset <= 0)
                item.SetHidden();
        }

        private UiMainScreenLevel CreateItem(UiMainScreenPointData pointData, int curLevel)
        {
            var item = Instantiate(Prefab, pointData.Point);
            item.transform.localPosition = Vector3.zero;
            item.PointData = pointData;
            
            ChangeRoadSize(item);

            InitText(item, pointData, curLevel);
            
            _items.Add(item);
            return item;
        }

        private void ChangeRoadSize(UiMainScreenLevel item)
        {
            // item.Road.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(_offset));
            var roadSize = item.Road.sizeDelta;
            item.Road.sizeDelta = new Vector2(roadSize.x, Math.Abs(_offset));
        }

        private static void ChangeColor(UiMainScreenLevel item)
        {
            if(item.PointData == null)
                return;
            
            if (item.PointData.LevelOffset > 0)
                item.ToGray();
            if (item.PointData.LevelOffset == 0)
                item.ToGreen();
            if (item.PointData.LevelOffset < 0)
                item.ToBlue();
        }
        private static void SetColor(UiMainScreenLevel item)
        {
            if(item.PointData == null)
                return;
            
            if (item.PointData.LevelOffset > 0)
                item.SetGray();
            if (item.PointData.LevelOffset == 0)
                item.SetGreen();
            if (item.PointData.LevelOffset < 0)
                item.SetBlue();
        }

        private void InitText(UiMainScreenLevel item, UiMainScreenPointData pointData, int curLevel)
        {
            item.Level.text = (curLevel + pointData.LevelOffset).ToString();
            item.name = item.Level.text;
        }

        private static void ChangeParent(UiMainScreenLevel item)
        {
            if (item.PointData != null)
                item.transform.SetParent(item.PointData.Point, true);
        }

        private void MoveItem(UiMainScreenLevel item, TweenCallback onComplete)
        {
            item.transform
                .DOMoveY(item.PointData.Point.position.y, 1)
                .SetEase(Ease.InOutSine)
                .OnComplete(onComplete);
        }

        private void CheckToDelete(UiMainScreenLevel item)
        {
            var isLast = item.PointData == _pointsData.Last(); 
            if (isLast)
            {
                _items.Remove(item);
                Destroy(item.gameObject);
            }
        }
    }
}