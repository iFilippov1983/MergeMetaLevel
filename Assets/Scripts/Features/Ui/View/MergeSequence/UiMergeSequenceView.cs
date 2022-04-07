using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class UiMergeSequenceView : UiBaseView
    {
        public UiSequenceInfoItem ItemPrefab;
        public Transform ItemsContainer;
        public List<UiSequenceInfoItem> Items;
        public Button Close;
        

        // Api
        public UiMergeSequenceApi Api;
    }
    
    
    [Serializable]
    public class UiMergeSequenceApi : UiBaseApi
    {
        // private MergeSequenceConfig _config;
        private UiMergeSequenceView _view;
        
        [Button]
        public void SetCtx(UiMergeSequenceView view)
        {
            base.SetCtxBase(view);
            _view = view;
            // _config = sequenceSo;
            SetCloseButton(_view.Close);
        }
        
        [Button]
        public async Task Show(MergeSequenceConfig sequenceSo)
        {
            UpdateView(sequenceSo);
            
            await ShowThenClose();
        }

        [HorizontalGroup]
        [Button(ButtonSizes.Large)]
        private void Editor_Clear()
        {
            _view.Items.ForEach(item =>
            {
                if (item != _view.ItemPrefab) 
                    GameObject.DestroyImmediate(item.gameObject);
            });
            _view.Items.RemoveAll(item => item == null);
        }

        [HorizontalGroup]
        [Button(ButtonSizes.Large)]
        private void UpdateView(MergeSequenceConfig sequenceSo )
        {
            var datas = sequenceSo.Items;
            if(datas.Count > _view.Items.Count )
                while (_view.Items.Count < datas.Count)
                {
                    var newItem = GameObject.Instantiate(_view.ItemPrefab, _view.ItemsContainer);
                    _view.Items.Add(newItem);
                }
            
            _view.Items.ForEach(item => item.gameObject.SetActive(false));

            for (int i = 0; i < datas.Count; i++)
            {
                var item = _view.Items[i];
                var data = datas[i];
                item.Icon.sprite = data.Sprite;
                item.Arrow.gameObject.SetActive((i+1) % 3 != 0 && i != datas.Count -1) ;
                item.gameObject.SetActive(true);
            }
        } 
    }
}