using UnityEngine;

namespace Core.Blocks.Base
{
    public abstract class SortingBy
    {
        public abstract void Order(int order);
        public abstract void Layer(int layer);
    }

    public class RendererSorting : SortingBy
    {
        public Renderer renderer;

        public override void Order(int order) => renderer.sortingOrder = order;
        public override void Layer(int layer) => renderer.sortingLayerID = layer;
    }
}