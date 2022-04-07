using System;
using UnityEngine;

namespace Core.Blocks.Base
{
    public class RendererOrderMono : MonoBehaviour
    {
        public new Renderer renderer;
        public int order;
        public int layerId = -1;
        public string layerName;

        private void Start()
        {
            Order();
            LayerId();
            LayerName();
        }

        public void Order() => renderer.sortingOrder = order;
        public void LayerId()
        {
            if(layerId != -1)
                renderer.sortingLayerID = layerId;
        }

        public void LayerName()
        {
            if(!string.IsNullOrEmpty(layerName))
                renderer.sortingLayerName = layerName;
        }
    }
}