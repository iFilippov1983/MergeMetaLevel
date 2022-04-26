using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Lofelt.NiceVibrations;
using UnityEngine;
using Utils;

namespace Core
{
    public class BoardService
    {
        private readonly Contexts _contexts;
        private readonly MergeConfig _config;
        private readonly FactoryService _factory;
        private readonly ViewFactoryService _viewFactory;
        private readonly MergeDynamicData _data;
        private readonly MergeRules _rules;
        private readonly MergeVisualConfig _visualConfig;

        public BoardService(Contexts contexts, MergeConfig config, FactoryService factory, ViewFactoryService viewFactory)
        {
            _contexts = contexts;
            _config = config;
            _factory = factory;
            _rules = _config.Rules;
            _viewFactory = viewFactory;
            _data = _contexts.game.ctx.dynamicData;
            _visualConfig = _config.VisualConfig;
        }

        public async Task DOMerge(MergeItemConfig draggedConfig,
            int posX,
            int posY,
            List<ChipsEntity> siblings, List<Vector2> availablePositions)
        {
            var current = draggedConfig;
            var next = draggedConfig.Next();

            foreach (var e in siblings)
            {
                e.isDead = true;
                e.DoRemovePos(_contexts, false);
            }

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

            await AnimateCollapse(posX, posY, siblings, _visualConfig);
            await Task.Delay(_visualConfig.DelayBeforeCreate);
            _viewFactory.CreateParticles("appear", posX, posY);
            
            foreach (var item in siblings)
                item.toDelete = true;

            
            var rule = _rules.GenerateRulesGet(siblings.Count);

            if (availablePositions.Count < rule.GeneratedTypes.Count)
                throw new Exception(
                    $"Смотри конфиг Merge Rules. Генериться должно меньше, чем схлопнулось. MergeCount [{siblings.Count}] ");

            var tasks = new List<Task>();
            var generatedTypesCount = rule.GeneratedTypes.Count;
            for (int i = 0; i < generatedTypesCount; i++)
            {
                var generatedType = rule.GeneratedTypes[i];
                var itemConfig = generatedType == MergeRules.MergeGenerateRuleType.Next ? next : current;
                var targetPos = availablePositions[i];
                
                // TODO : хак. Если предмет последнего уроня (а ведь премет уже удалён) создаём такой же
                if (itemConfig == null)
                    itemConfig = current;
            
                var itemData = new MergeItemProfileData(itemConfig, (int) targetPos.x, (int) targetPos.y, 0);
                var item = Contexts.sharedInstance.game.ctx.services.factory.Create(itemData);
                var task = item.view.AnimateDropItem(_visualConfig, item.view, posX, posY, targetPos, i);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            
            // if(generatedTypesCount > 0)
            // await Task.Delay(100); // AnimateDropItem duration
        }
        public void DOUpgrade(
            ChipsEntity dragged, 
            int posX,
            int posY, 
            List<ChipsEntity> siblings)
        {
            var current = dragged.config.value;
            var next = dragged.config.value.Next();
            // if(next == null)
            //     return;

            foreach (var e in siblings)
            {
                e.isDead = true;
                e.DoRemovePos(_contexts, false);
            }
            
            // await AnimateCollapse(posX, posY, siblings, _visualConfig);
            // await Task.Delay(_visualConfig.DelayBeforeCreate);

            foreach (var item in siblings)
                item.toDelete = true;
            
            var availablePositions = GetAvailablePositions( siblings, posX, posY, null);
            var rule = _rules.GenerateRulesGet(siblings.Count);

            if (availablePositions.Count < rule.GeneratedTypes.Count)
                throw new Exception(
                    $"Смотри конфиг Merge Rules. Генериться должно меньше, чем схлопнулось. MergeCount [{siblings.Count}] ");
            
            for (int i = 0; i < rule.GeneratedTypes.Count; i++)
            {
                var generatedType = rule.GeneratedTypes[i];
                var itemConfig = generatedType == MergeRules.MergeGenerateRuleType.Next ? next : current;
                var targetPos = availablePositions[i];
            
                // TODO : хак. Если предмет последнего уроня (а ведь премет уже удалён) создаём такой же
                if (itemConfig == null)
                    itemConfig = current;
                
                var itemData = new MergeItemProfileData(itemConfig, (int) targetPos.x, (int) targetPos.y, 0);
                var item = Contexts.sharedInstance.game.ctx.services.factory.Create(itemData);
                item.view.AnimateDropItem(_visualConfig, item.view, posX, posY, targetPos, i).DoAsync();
            }
            
            _viewFactory.CreateParticles("appear", posX, posY);
        }
        

        private void PrintVectors(List<Vector2> availablePositions, string before)
        {
            var res = "";
            foreach (var v in availablePositions)
            res += $"{v.x}.{v.y}   ";
            Debug.Log($"{before} => {res}");

        }

        private static List<Vector2> GetPositions(List<ChipsEntity> items)
        {
            var availablePositions = new List<Vector2>();
            foreach (var item in items)
                availablePositions.Add(new Vector2(item.position.x, item.position.y));
            
            return availablePositions;
        }

        
        public List<ChipsEntity> GetSiblings(ChipsEntity item, int posX, int posY)
        {
            var visited = new HashSet<ChipsEntity>();
            visited.Add(item);
            
            var res = new List<ChipsEntity>();
            
            var origConfig = item.config.value;
            AddSiblings(_data, origConfig, posX, posY, visited, res);
            AddSiblings(_data, origConfig, posX + 1, posY + 0, visited, res);
            AddSiblings(_data, origConfig, posX - 1, posY + 0, visited, res);
            AddSiblings(_data, origConfig, posX + 0, posY + 1, visited, res);
            AddSiblings(_data, origConfig, posX + 0, posY - 1, visited, res);
            
            return res;
        }

        private static void AddSiblings(MergeDynamicData _data, MergeItemConfig origConfig, int posX, int posY, HashSet<ChipsEntity> visited, List<ChipsEntity> res)
        {
            // Note : check if empty
            var sibling = _data.GetItem(posX, posY);
            if(sibling == null)
                return;
            
            // Note : Check if already visited
            if( visited.Contains(sibling))
                return;

            // Note : Check if same type
            if(origConfig != sibling.config.value)
                return;

            res.Add(sibling);
            visited.Add(sibling);
            
            AddSiblings(_data, origConfig, posX + 1, posY + 0, visited, res);
            AddSiblings(_data, origConfig, posX - 1, posY + 0, visited, res);
            AddSiblings(_data, origConfig, posX + 0, posY + 1, visited, res);
            AddSiblings(_data, origConfig, posX + 0, posY - 1, visited, res);
        }
        
        private async Task AnimateCollapse(int posX, int posY, List<ChipsEntity> siblings, MergeVisualConfig visualConfig)
        {
            void AnimateClear(ChipsEntity item)
            {
                if (item.isLocked)
                    _viewFactory.CreateUnlockParticles(item.position.x, item.position.y);
            }
            
            foreach (var item in siblings)
                item.view.value.CollapseTo(new Vector3(posX, posY), visualConfig, _viewFactory,() => { } );

            await Task.Delay(visualConfig.CollapseScaleDelay - 100);
            for (int i = 0; i < siblings.Count; i++)
            {
                var item = siblings[i];
                item.view.value.AnimateClear(i * visualConfig.CollapseScaleDuration/siblings.Count, () => AnimateClear(item));
            }
            await Task.Delay(100);
            
            await Task.Delay(visualConfig.CollapseScaleDuration );
        }

        public List<Vector2> GetAvailablePositions(List<ChipsEntity> siblings, int posX, int posY, ChipsEntity exclude)
        {
            if (exclude != null)
                siblings = siblings.Clone().DoRemove(exclude);
            
            var availablePositions = siblings.ConvertAll(item => new Vector2(item.position.x, item.position.y));
            
            if(availablePositions.FindIndex(v => v.x == posX && v.y == posY) == -1)
                availablePositions.Add( new Vector2(posX, posY));
            
            var targetPos = new Vector2(posX, posY);
            availablePositions = availablePositions
                .OrderBy(v => (targetPos - v).sqrMagnitude)
                .Distinct()
                .ToList();
            
            return availablePositions;
        }

    }
}