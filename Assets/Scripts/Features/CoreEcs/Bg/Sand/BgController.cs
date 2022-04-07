using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Tutorial.Game
{
    public class BgController : ScriptableObject
    {
        [Serializable]
        public class MaskSpriteMapItem
        {
            [HideLabel]
            [HorizontalGroup("1")]
            [PreviewField( ObjectFieldAlignment.Right )]
            public Sprite Sprite;
            [HideLabel]
            [HorizontalGroup("1")]
            [PreviewField( ObjectFieldAlignment.Left )]
            public Sprite Mask;
        }

        public class CellPos
        {
            public float x;
            public float y;

            public CellPos(Vector2 v)
            {
                x = v.x;
                y = v.y;
            }

            public bool Equal(Vector2 v)
            {
                return x == v.x && y == v.y;
            }

            static public CellPos From( Vector2 v2)
            {
                return new CellPos(v2);
            }
        }
        
        public Transform dotPrefab;
        public BgItem bgPrefab;
        public BgItem bgMaskPrefab;
        
        public List<RuleItem> Rules;
        public List<MaskSpriteMapItem> MaskSpriteMap;
        
        private List<CellPos> Items;
        private Transform DrawContainer;

        private Vector2 Min05;
        private Vector2 Max05;

        public void Init(Transform drawContainer)
        {
            DrawContainer = drawContainer;
            Clear();
            if(Items == null)
                Items = new List<CellPos>();
        }

        public void Clear()
        {
            Items?.Clear();
            foreach (Transform child in DrawContainer)
                DoDestroy(child.gameObject);
        }

        public void Add(Vector2 pos, bool noRecalc = false)
        {
            var alreadyHave = Items.Find(v => v.Equal(pos) ) != null;
            if (alreadyHave)
                return;
            
            Items.Add(CellPos.From(pos));
            if(!noRecalc)
                Recalc();
        }

        public void Delete(Vector2 pos)
        {
            var item = Items.Find(v =>v.Equal(pos));
            if (item == null)
                return;

            Items.Remove(item);
            Recalc();
        }
        
        public void Recalc()
        {
            (Min05, Max05) = RecalcMinMax(Items);
            
            foreach (Transform child in DrawContainer)
                DoDestroy(child.gameObject);
            
            DrawBounds();
            DrawBg();
        }
        
        private void DoDestroy(GameObject obj)
        {
            if(Application.isPlaying)
                Destroy(obj);
            else
                DestroyImmediate(obj);
        }

    
        private void DrawBg()
        {
            for (float y = Min05.y; y <= Max05.y; y++)
            {
                for (float x = Min05.x; x <= Max05.x; x++)
                    CreateBg(x, y);
            }
        }
    
        private void CreateBg(float x, float y)
        {
            var mask = GetNeigboursMask(x, y);
            var rule = Rules.Find(v => v.RuleEnum == mask);
            if (rule == null)
            {
                Debug.LogWarning($"NO RULE found {mask}");
                return;
            }
            
            var bg = Instantiate(bgPrefab, new Vector3(x, y), Quaternion.identity, DrawContainer);
            bg.InitBy(rule, sprite => sprite);

            if (bgMaskPrefab != null)
            {
                var bgMask = Instantiate(bgMaskPrefab, new Vector3(x, y), Quaternion.identity, DrawContainer);
                bgMask.InitBy(rule , sprite => MaskSpriteMap.Find((kv) => kv.Sprite == sprite)?.Mask);
            }

            var cellY = Mathf.RoundToInt(y - 0.5f);
            // var chipSortingLayer = (cellY - 100);
//          var chipSortingLayer =  cellY * (int) ChipCellSortOrderType.Max + (int) ChipCellSortOrderType.DevourSand; // Later we will use ChipCellSortOrderType
            
            // sort "bg" on chip layer
            // ForEachSpriteRenderer(bg.gameObject, renderer => renderer.sortingOrder += chipSortingLayer);
            
            // bgMask still over all
            
        }

        void ForEachSpriteRenderer(GameObject go, UnityAction<SpriteRenderer> cb)
        {
            var renderers = go.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renderers)
                cb(r);
        }
        
        private RuleEnum GetNeigboursMask(float x, float y)
        {
            RuleEnum res = RuleEnum.None;
            res |= HasBgOnLeftTop(x, y) ? RuleEnum.lt : 0;
            res |= HasBgOnRightTop(x, y) ? RuleEnum.rt : 0;
            res |= HasBgOnRightBottom(x, y) ? RuleEnum.rb : 0;
            res |= HasBgOnLeftBottom(x, y) ? RuleEnum.lb : 0;
    
            return res;
        }
        
        private bool HasBg(float x, float y) => Items.Find(v => v.x == x && v.y == y ) != null;
        
        private bool HasBgOnLeftTop(float x, float y) => HasBg(x-0.5f, y+0.5f); 
        private bool HasBgOnLeftBottom(float x, float y) => HasBg(x-0.5f, y-0.5f); 
        private bool HasBgOnRightTop(float x, float y) => HasBg(x+0.5f, y+0.5f); 
        private bool HasBgOnRightBottom(float x, float y) => HasBg(x+0.5f, y-0.5f);
    
        private void DrawBounds()
        {
//            Instantiate(dotPrefab, Min05, Quaternion.identity, DrawContainer);
//            Instantiate(dotPrefab, Max05, Quaternion.identity, DrawContainer);
        }
    
        private (Vector2 min05, Vector2 max05) RecalcMinMax(List<CellPos> items)
        {
            Vector2 min, min05, max, max05;
            min.x = min05.x = min.y = min05.y = 1000;
            max.x = max05.x = max.y = max05.y = -1000;
    
            foreach (var pos in items)
            {
                if (pos.x < min.x)
                {
                    min.x = pos.x;
                    min05.x = pos.x - 0.5f;
                }
    
                if (pos.y < min.y)
                {
                    min.y = pos.y;
                    min05.y = pos.y - 0.5f;
                }
    
                if (pos.x > max.x)
                {
                    max.x = pos.x;
                    max05.x = pos.x + 0.5f;
                }
    
                if (pos.y > max.y)
                {
                    max.y = pos.y;
                    max05.y = pos.y + 0.5f;
                }
            }
    
            return (min05, max05);
        }
    }
}