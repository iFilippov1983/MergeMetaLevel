using Data;
using UnityEngine;

namespace Core
{
    public class FactoryService
    {
        private readonly Contexts _contexts;
        private readonly MergeConfig _config;
        private readonly ViewFactoryService _viewFactory;

        public FactoryService(Contexts contexts, MergeConfig config, ViewFactoryService viewFactory)
        {
            _contexts = contexts;
            _config = config;
            _viewFactory = viewFactory;
        }

        public BaseItem Create(MergeItemProfileData data)
        {
            var config = data.config;
            if(config.isLife)
                return new HeartItem(_viewFactory, config, data, _contexts);
            if(config.isGenerator)
                return new GeneratorItem(_viewFactory, config, data, _contexts);
            else
                return new MergeItem(_viewFactory, config, data, _contexts);
        }
    }
}