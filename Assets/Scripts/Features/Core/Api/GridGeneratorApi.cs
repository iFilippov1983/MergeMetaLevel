using System;
using Data;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Core
{
    [Serializable]
    public class GridGeneratorApi
    {
        private GridGeneratorLinks _view;

        public GridGeneratorApi(GridGeneratorLinks view)
        {
            _view = view;
            
        }

        [Button]
        public void LoadLevel(LevelConfig level)
        {
            _view.MergeLevel = level;

            _view.Gray.SetActive(true);
            _view.White.SetActive(true);
            
            _view.BgController.Init(_view.BgContainer);
            _view.Container.Clear();
            
            if (_view.Bg != null)
            {
                var bg = Object.Instantiate( _view.Bg, Vector3.zero, Quaternion.identity, _view.Container);
                bg.size = new Vector2(_view.MergeLevel.Width, _view.MergeLevel.Height);
                bg.transform.position = new Vector3((level.Width - 1f)/2, (level.Height - 1f)/ 2, 0);
            }
            
            for (int yy = 0; yy < _view.MergeLevel.Height; yy++)
            for (int xx = 0; xx < _view.MergeLevel.Width; xx++)
            {
                if(level.Holes.Exists(x => x.x == xx && x.y == yy))
                    continue;
                
                var tilePrefab = (xx+yy) %2 == 0 ? _view.Gray : _view.White;
                var tile = Object.Instantiate((Object) tilePrefab, new Vector2(xx, yy), Quaternion.identity, _view.Container);
                tile.name = $"[{yy}][{xx}]";
                _view.BgController.Add(new Vector2(xx,yy), true);
            }

            _view.BgController.Recalc();
            
            _view.Gray.SetActive(false);
            _view.White.SetActive(false);
        }
    }
}